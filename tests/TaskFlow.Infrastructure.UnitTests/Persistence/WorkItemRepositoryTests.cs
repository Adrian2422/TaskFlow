using Microsoft.EntityFrameworkCore;
using Shouldly;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.AppDbContext;
using TaskFlow.Infrastructure.Persistence;

namespace TaskFlow.Infrastructure.UnitTests.Persistence;

public class WorkItemRepositoryTests
{
    private ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task GetMaxOrderInColumnAsync_ShouldReturnMaxOrder_WhenItemsExist()
    {
        // Arrange
        using var context = CreateDbContext();
        var boardId = Guid.NewGuid();
        var columnId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        
        context.WorkItems.AddRange(
            WorkItem.Create("Task 1", null, boardId, columnId, 100.0, userId),
            WorkItem.Create("Task 2", null, boardId, columnId, 500.0, userId),
            WorkItem.Create("Task 3", null, boardId, columnId, 300.0, userId)
        );
        await context.SaveChangesAsync();
        
        var repository = new WorkItemRepository(context);
        
        // Act
        var maxOrder = await repository.GetMaxOrderInColumnAsync(columnId);
        
        // Assert
        maxOrder.ShouldBe(500.0);
    }

    [Fact]
    public async Task GetMaxOrderInColumnAsync_ShouldReturnNull_WhenNoItemsExist()
    {
        // Arrange
        using var context = CreateDbContext();
        var columnId = Guid.NewGuid();
        var repository = new WorkItemRepository(context);
        
        // Act
        var maxOrder = await repository.GetMaxOrderInColumnAsync(columnId);
        
        // Assert
        maxOrder.ShouldBeNull();
    }

    [Fact]
    public async Task GetMaxOrderInBacklogAsync_ShouldReturnMaxOrder_WhenItemsInBacklogExist()
    {
        // Arrange
        using var context = CreateDbContext();
        var boardId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        
        context.WorkItems.AddRange(
            WorkItem.Create("Task 1", null, boardId, null, 10.0, userId),
            WorkItem.Create("Task 2", null, boardId, null, 50.0, userId),
            WorkItem.Create("Task 3", null, boardId, Guid.NewGuid(), 100.0, userId) // In some column
        );
        await context.SaveChangesAsync();
        
        var repository = new WorkItemRepository(context);
        
        // Act
        var maxOrder = await repository.GetMaxOrderInBacklogAsync(boardId);
        
        // Assert
        maxOrder.ShouldBe(50.0);
    }
}
