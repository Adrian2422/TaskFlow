using Shouldly;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Domain.UnitTests.Entities;

public class BoardColumnTests
{
    [Fact]
    public void Create_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var name = "To Do";
        var order = 1.0;
        var boardId = Guid.NewGuid();
        var isProtected = true;

        // Act
        var column = BoardColumn.Create(name, order, boardId, isProtected);

        // Assert
        column.Name.ShouldBe(name);
        column.Order.ShouldBe(order);
        column.BoardId.ShouldBe(boardId);
        column.IsProtected.ShouldBe(isProtected);
        column.Id.ShouldNotBe(Guid.Empty);
        column.WorkItems.ShouldBeEmpty();
    }
}
