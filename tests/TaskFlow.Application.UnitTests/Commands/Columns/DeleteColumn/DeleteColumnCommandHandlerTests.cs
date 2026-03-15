using Moq;
using Shouldly;
using TaskFlow.Application.Commands.Columns.DeleteColumn;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Errors;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.UnitTests.Commands.Columns.DeleteColumn;

public class DeleteColumnCommandHandlerTests
{
    private readonly Mock<IBoardColumnRepository> _columnRepoMock;
    private readonly Mock<IWorkItemRepository> _workItemRepoMock;
    private readonly DeleteColumnCommandHandler _handler;

    public DeleteColumnCommandHandlerTests()
    {
        _columnRepoMock = new Mock<IBoardColumnRepository>();
        _workItemRepoMock = new Mock<IWorkItemRepository>();
        _handler = new DeleteColumnCommandHandler(_columnRepoMock.Object, _workItemRepoMock.Object);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenColumnExists()
    {
        // Arrange
        var boardId = Guid.NewGuid();
        var existingColumn = new BoardColumn { Name = "New", IsProtected = false, BoardId = boardId };
        var columnId = existingColumn.Id;
        
        _columnRepoMock.Setup(x => x.GetByIdWithWorkItemsAsync(columnId)).ReturnsAsync(existingColumn);
        _workItemRepoMock.Setup(x => x.GetMaxOrderInBacklogAsync(boardId)).ReturnsAsync((double?)null);
        
        var command = new DeleteColumnCommand(boardId, columnId);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.ShouldBeTrue();
        
        _columnRepoMock.Verify(x => x.DeleteAsync(existingColumn), Times.Once);
        _columnRepoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenColumnDoesNotExists()
    {
        // Arrange
        var boardId = Guid.NewGuid();
        var columnId = Guid.NewGuid();
        var command = new DeleteColumnCommand(boardId, columnId);
        
        _columnRepoMock.Setup(x => x.GetByIdWithWorkItemsAsync(columnId)).ReturnsAsync((BoardColumn?)null);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(ColumnErrors.NotFound(columnId, boardId));
        
        _columnRepoMock.Verify(x => x.DeleteAsync(It.IsAny<BoardColumn>()), Times.Never);
        _columnRepoMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenColumnIsProtected()
    {
        // Arrange
        var boardId = Guid.NewGuid();
        var existingColumn = new BoardColumn { Name = "New", IsProtected = true, BoardId = boardId };
        var columnId = existingColumn.Id;

        _columnRepoMock.Setup(x => x.GetByIdWithWorkItemsAsync(columnId)).ReturnsAsync(existingColumn);

        var command = new DeleteColumnCommand(boardId, columnId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(ColumnErrors.Protected);

        _columnRepoMock.Verify(x => x.DeleteAsync(It.IsAny<BoardColumn>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldMoveWorkItemsToBacklog_WhenColumnIsDeleted()
    {
        // Arrange
        var boardId = Guid.NewGuid();
        var workItem = new WorkItem { Title = "Task 1", BoardId = boardId, Order = 100 };
        var existingColumn = new BoardColumn 
        { 
            Name = "New", 
            IsProtected = false, 
            BoardId = boardId,
            WorkItems = new List<WorkItem> { workItem }
        };
        var columnId = existingColumn.Id;
        workItem.ColumnId = columnId;

        _columnRepoMock.Setup(x => x.GetByIdWithWorkItemsAsync(columnId)).ReturnsAsync(existingColumn);
        _workItemRepoMock.Setup(x => x.GetMaxOrderInBacklogAsync(boardId)).ReturnsAsync(500.0);

        var command = new DeleteColumnCommand(boardId, columnId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        workItem.ColumnId.ShouldBeNull();
        workItem.Order.ShouldBe(1500.0);

        _workItemRepoMock.Verify(x => x.UpdateAsync(workItem), Times.Once);
        _columnRepoMock.Verify(x => x.DeleteAsync(existingColumn), Times.Once);
        _columnRepoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        _workItemRepoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
}
