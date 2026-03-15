using Shouldly;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Domain.UnitTests.Entities;

public class WorkItemTests
{
    [Fact]
    public void Create_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var title = "Task Title";
        var description = "Task Description";
        var boardId = Guid.NewGuid();
        var columnId = Guid.NewGuid();
        var order = 1.0;

        // Act
        var workItem = WorkItem.Create(title, description, boardId, columnId, order);

        // Assert
        workItem.Title.ShouldBe(title);
        workItem.Description.ShouldBe(description);
        workItem.BoardId.ShouldBe(boardId);
        workItem.ColumnId.ShouldBe(columnId);
        workItem.Order.ShouldBe(order);
        workItem.Id.ShouldNotBe(Guid.Empty);
        workItem.IsArchived.ShouldBeFalse();
    }

    [Fact]
    public void Archive_ShouldSetIsArchivedToTrue()
    {
        // Arrange
        var workItem = WorkItem.Create("Title", null, Guid.NewGuid(), null, 1);

        // Act
        workItem.Archive();

        // Assert
        workItem.IsArchived.ShouldBeTrue();
    }

    [Fact]
    public void Restore_ShouldSetIsArchivedToFalse()
    {
        // Arrange
        var workItem = WorkItem.Create("Title", null, Guid.NewGuid(), null, 1);
        workItem.Archive();

        // Act
        workItem.Restore();

        // Assert
        workItem.IsArchived.ShouldBeFalse();
    }
}
