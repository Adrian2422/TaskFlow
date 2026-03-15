using Moq;
using FluentAssertions;
using TaskFlow.Application.Commands.WorkItems.MoveWorkItem;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.UnitTests.Commands.WorkItems.MoveWorkItem;

public class MoveWorkItemCommandHandlerTests
{
    private readonly Mock<IWorkItemRepository> _workItemRepoMock;
    private readonly Mock<IBoardColumnRepository> _boardColumnRepoMock;
    private readonly MoveWorkItemCommandHandler _handler;

    public MoveWorkItemCommandHandlerTests()
    {
        _workItemRepoMock = new Mock<IWorkItemRepository>();
        _boardColumnRepoMock = new Mock<IBoardColumnRepository>();
        _handler = new MoveWorkItemCommandHandler(_workItemRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenColumnExists()
    {
        // Arrange
        var existingBoardColumn = new BoardColumn() { Name = "New" };
        var columnId = existingBoardColumn.Id;
        
        var existingWorkItem = new WorkItem() { Title = "Task 1", Description = "Desc", Order = 2000};
        var workItemId = existingWorkItem.Id;

        _workItemRepoMock.Setup(x => x.GetByIdAsync(workItemId)).ReturnsAsync(existingWorkItem);
        _boardColumnRepoMock.Setup(x => x.GetByIdAsync(columnId)).ReturnsAsync(existingBoardColumn);

        var command = new MoveWorkItemCommand(workItemId, columnId, 0, 1000);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _workItemRepoMock.Verify(x => x.UpdateAsync(It.Is<WorkItem>(wi => 
            wi.ColumnId == columnId && 
            wi.Order.Equals(500)
            )), Times.Once);
        _workItemRepoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
}
