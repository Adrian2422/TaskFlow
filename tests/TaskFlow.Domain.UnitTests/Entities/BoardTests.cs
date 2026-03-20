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
        var userId = Guid.NewGuid();

        // Act
        var board = Board.Create(name, description, userId);

        // Assert
        board.Name.ShouldBe(name);
        board.Description.ShouldBe(description);
        board.CreatedById.ShouldBe(userId);
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
        var userId = Guid.NewGuid();
        var board = Board.Create("Board", "Desc", userId);
        var column = BoardColumn.Create("To Do", 1, board.Id);
        var workItemInColumn = WorkItem.Create("Task 1", "Desc", board.Id, column.Id, 1, userId);
        var backlogItem = WorkItem.Create("Task 2", "Desc", board.Id, null, 1, userId);

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
        var userId = Guid.NewGuid();
        var board = Board.Create("Board", "Desc", userId);
        board.Archive();

        // Act
        board.Restore();

        // Assert
        board.IsArchived.ShouldBeFalse();
    }
}
