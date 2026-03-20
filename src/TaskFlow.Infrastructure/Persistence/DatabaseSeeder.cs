using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.AppDbContext;

namespace TaskFlow.Infrastructure.Persistence;

public class DatabaseSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public DatabaseSeeder(ApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task SeedAsync()
    {
        if (await _context.Boards.AnyAsync())
        {
            return;
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "admin@taskflow.com",
            Email = "admin@taskflow.com",
            FullName = "Administrator",
        };
        await _userManager.CreateAsync(user, "test123");

        var boardFaker = new Faker<Board>()
            .RuleFor(b => b.Name, f => f.Commerce.ProductName())
            .RuleFor(b => b.Description, f => f.Lorem.Sentence())
            .RuleFor(b => b.CreatedById, user.Id);

        var workItemFaker = new Faker<WorkItem>()
            .RuleFor(w => w.Title, f => f.Lorem.Sentence(3))
            .RuleFor(w => w.Description, f => f.Lorem.Paragraph())
            .RuleFor(w => w.Order, f => f.Random.Double(0, 100))
            .RuleFor(w => w.CreatedById, user.Id);

        var board = boardFaker.Generate();
        
        board.Members.Add(new BoardMember
        {
            UserId = user.Id,
            BoardId = board.Id,
            Role = BoardRole.Owner
        });

        var columns = new List<BoardColumn>
        {
            new() { Name = "New", Order = 0, IsProtected = true, BoardId = board.Id },
            new() { Name = "In progress", Order = 1000, IsProtected = false, BoardId = board.Id },
            new() { Name = "Done", Order = 2000, IsProtected = false, BoardId = board.Id }
        };

        var totalWorkItems = new Random().Next(50, 101);
        var workItems = workItemFaker.Generate(totalWorkItems);

        foreach (var item in workItems)
        {
            var randomColumn = columns[new Random().Next(columns.Count)];
            item.BoardId = board.Id; 
            randomColumn.WorkItems.Add(item);
        }

        foreach (var column in columns)
        {
            board.Columns.Add(column);
        }

        await _context.Boards.AddAsync(board);
        await _context.SaveChangesAsync();
    }
}
