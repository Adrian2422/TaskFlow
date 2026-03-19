using Microsoft.EntityFrameworkCore;
using Shouldly;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.AppDbContext;
using TaskFlow.Infrastructure.Services;

namespace TaskFlow.Infrastructure.UnitTests.Services;

public class OrderNormalizationServiceTests
{
    private ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task NormalizeWorkItemsIfNeeded_ShouldNormalize_WhenGapIsTooSmall()
    {
        // Arrange
        using var context = CreateDbContext();
        var boardId = Guid.NewGuid();
        var columnId = Guid.NewGuid();
        
        var userId = Guid.NewGuid();
        
        var items = new List<WorkItem>
        {
            WorkItem.Create("Task 1", null, boardId, columnId, 1.0, userId),
            WorkItem.Create("Task 2", null, boardId, columnId, 1.1, userId)
        };
        
        context.WorkItems.AddRange(items);
        await context.SaveChangesAsync();
        
        var service = new OrderNormalizationService(context);
        
        // Act
        await service.NormalizeWorkItemsIfNeeded(0.5);
        
        // Assert
        var updatedItems = await context.WorkItems
            .Where(x => x.ColumnId == columnId)
            .OrderBy(x => x.Order)
            .ToListAsync();
            
        updatedItems[0].Order.ShouldBe(1000.0);
        updatedItems[1].Order.ShouldBe(2000.0);
    }

    [Fact]
    public async Task NormalizeWorkItemsIfNeeded_ShouldNotNormalize_WhenGapIsLargeEnough()
    {
        // Arrange
        using var context = CreateDbContext();
        var boardId = Guid.NewGuid();
        var columnId = Guid.NewGuid();
        
        var userId = Guid.NewGuid();
        
        var items = new List<WorkItem>
        {
            WorkItem.Create("Task 1", null, boardId, columnId, 1000.0, userId),
            WorkItem.Create("Task 2", null, boardId, columnId, 2000.0, userId)
        };
        
        context.WorkItems.AddRange(items);
        await context.SaveChangesAsync();
        
        var service = new OrderNormalizationService(context);
        
        // Act
        await service.NormalizeWorkItemsIfNeeded(100.0);
        
        // Assert
        var updatedItems = await context.WorkItems
            .Where(x => x.ColumnId == columnId)
            .OrderBy(x => x.Order)
            .ToListAsync();
            
        updatedItems[0].Order.ShouldBe(1000.0);
        updatedItems[1].Order.ShouldBe(2000.0);
    }

    [Fact]
    public async Task NormalizeColumnsIfNeeded_ShouldNormalize_WhenGapIsTooSmall()
    {
        // Arrange
        using var context = CreateDbContext();
        var boardId = Guid.NewGuid();
        
        var columns = new List<BoardColumn>
        {
            BoardColumn.Create("Col 1", 1.0, boardId),
            BoardColumn.Create("Col 2", 1.05, boardId)
        };
        
        context.Columns.AddRange(columns);
        await context.SaveChangesAsync();
        
        var service = new OrderNormalizationService(context);
        
        // Act
        await service.NormalizeColumnsIfNeeded(0.1);
        
        // Assert
        var updatedColumns = await context.Columns
            .Where(x => x.BoardId == boardId)
            .OrderBy(x => x.Order)
            .ToListAsync();
            
        updatedColumns[0].Order.ShouldBe(1000.0);
        updatedColumns[1].Order.ShouldBe(2000.0);
    }
}
