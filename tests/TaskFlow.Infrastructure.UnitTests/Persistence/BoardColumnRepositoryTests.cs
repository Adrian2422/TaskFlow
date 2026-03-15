using Microsoft.EntityFrameworkCore;
using Shouldly;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.AppDbContext;
using TaskFlow.Infrastructure.Persistence;

namespace TaskFlow.Infrastructure.UnitTests.Persistence;

public class BoardColumnRepositoryTests
{
    private ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task GetMaxOrderInBoardAsync_ShouldReturnMaxOrder_WhenColumnsExist()
    {
        // Arrange
        using var context = CreateDbContext();
        var boardId = Guid.NewGuid();
        
        context.Columns.AddRange(
            BoardColumn.Create("Col 1", 10.0, boardId),
            BoardColumn.Create("Col 2", 20.0, boardId),
            BoardColumn.Create("Col 3", 15.0, boardId)
        );
        await context.SaveChangesAsync();
        
        var repository = new BoardColumnRepository(context);
        
        // Act
        var maxOrder = await repository.GetMaxOrderInBoardAsync(boardId);
        
        // Assert
        maxOrder.ShouldBe(20.0);
    }

    [Fact]
    public async Task GetByIdWithWorkItemsAsync_ShouldIncludeWorkItems()
    {
        // Arrange
        using var context = CreateDbContext();
        var boardId = Guid.NewGuid();
        var column = BoardColumn.Create("Col 1", 1.0, boardId);
        context.Columns.Add(column);
        
        var workItem = WorkItem.Create("Task 1", null, boardId, column.Id, 1.0);
        context.WorkItems.Add(workItem);
        
        await context.SaveChangesAsync();
        
        var repository = new BoardColumnRepository(context);
        
        // Act
        var result = await repository.GetByIdWithWorkItemsAsync(column.Id);
        
        // Assert
        result.ShouldNotBeNull();
        result.WorkItems.Count.ShouldBe(1);
        result.WorkItems.First().Title.ShouldBe("Task 1");
    }
}
