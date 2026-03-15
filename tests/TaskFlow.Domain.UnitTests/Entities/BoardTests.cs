using Shouldly;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Domain.UnitTests.Entities;

public class BoardTests
{
    [Fact]
    public void Create_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var name = "Test Board";
        var description = "Test Description";

        // Act
        var board = Board.Create(name, description);

        // Assert
        board.Name.ShouldBe(name);
        board.Description.ShouldBe(description);
        board.Id.ShouldNotBe(Guid.Empty);
        board.CreatedAt.ShouldBeInRange(DateTime.UtcNow.AddSeconds(-5), DateTime.UtcNow.AddSeconds(5));
        board.Columns.ShouldBeEmpty();
        board.BacklogItems.ShouldBeEmpty();
        board.IsArchived.ShouldBeFalse();
    }

    [Fact]
    public void Archive_ShouldSetIsArchivedToTrueAndArchiveAllRelatedItems()
    {
        // Arrange
        var board = Board.Create("Board", "Desc");
        var column = BoardColumn.Create("To Do", 1, board.Id);
        var workItemInColumn = WorkItem.Create("Task 1", "Desc", board.Id, column.Id, 1);
        var backlogItem = WorkItem.Create("Task 2", "Desc", board.Id, null, 1);

        column.WorkItems.Add(workItemInColumn);
        board.Columns.Add(column);
        board.BacklogItems.Add(backlogItem);

        // Act
        board.Archive();

        // Assert
        board.IsArchived.ShouldBeTrue();
        backlogItem.IsArchived.ShouldBeTrue();
        workItemInColumn.IsArchived.ShouldBeTrue();
    }

    [Fact]
    public void Restore_ShouldSetIsArchivedToFalse()
    {
        // Arrange
        var board = Board.Create("Board", "Desc");
        board.Archive();

        // Act
        board.Restore();

        // Assert
        board.IsArchived.ShouldBeFalse();
    }
}
