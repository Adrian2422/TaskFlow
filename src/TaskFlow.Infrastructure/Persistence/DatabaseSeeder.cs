using Bogus;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.AppDbContext;

namespace TaskFlow.Infrastructure.Persistence;

public class DatabaseSeeder
{
    private readonly ApplicationDbContext _context;

    public DatabaseSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (await _context.Boards.AnyAsync())
        {
            return;
        }

        var boardFaker = new Faker<Board>()
            .RuleFor(b => b.Name, f => f.Commerce.ProductName())
            .RuleFor(b => b.Description, f => f.Lorem.Sentence());

        var workItemFaker = new Faker<WorkItem>()
            .RuleFor(w => w.Title, f => f.Lorem.Sentence(3))
            .RuleFor(w => w.Description, f => f.Lorem.Paragraph())
            .RuleFor(w => w.Order, f => f.Random.Double(0, 100));

        var board = boardFaker.Generate();

        var columns = new List<BoardColumn>
        {
            new() { Name = "New", Order = 0, IsProtected = true },
            new() { Name = "In progress", Order = 1000, IsProtected = false },
            new() { Name = "Done", Order = 2000, IsProtected = false }
        };

        var totalWorkItems = new Random().Next(50, 101);
        var workItems = workItemFaker.Generate(totalWorkItems);

        foreach (var item in workItems)
        {
            var randomColumn = columns[new Random().Next(columns.Count)];
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
