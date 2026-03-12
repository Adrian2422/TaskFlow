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
        string[] columnNames = ["New", "In progress", "Done"];

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

        var boards = boardFaker.Generate(3);

        foreach (var board in boards)
        {
            var columns = columnNames.Select((name, index) => new BoardColumn
            {
                Name = name,
                Order = index,
                WorkItems = workItemFaker.Generate(new Random().Next(2, 5))
            });
            foreach (var column in columns)
            {
                board.Columns.Add(column);
            }
        }

        await _context.Boards.AddRangeAsync(boards);
        await _context.SaveChangesAsync();
    }
}
