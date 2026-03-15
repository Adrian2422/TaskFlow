using Moq;
using FluentAssertions;
using TaskFlow.Application.Commands.Columns.UpdateColumn;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Errors;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.UnitTests.Commands.Columns.UpdateColumn;

public class UpdateColumnCommandHandlerTests
{
    private readonly Mock<IBoardColumnRepository> _columnRepoMock;
    private readonly Mock<IBoardRepository> _boardRepoMock;
    private readonly UpdateColumnCommandHandler _handler;

    public UpdateColumnCommandHandlerTests()
    {
        _columnRepoMock = new Mock<IBoardColumnRepository>();
        _boardRepoMock = new Mock<IBoardRepository>();
        _handler = new UpdateColumnCommandHandler(_columnRepoMock.Object);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenDataIsValid()
    {
        // Arrange
        var existingBoard = new Board(){ Name = "TaskFlow", Description = "Desc" };
        var boardId = existingBoard.Id;
        
        var existingBoardColumn = new BoardColumn() { Name = "New", Order = 0, BoardId = boardId};
        var columnId = existingBoardColumn.Id;
        
        _boardRepoMock.Setup(x => x.GetByIdAsync(boardId)).ReturnsAsync(existingBoard);
        _columnRepoMock.Setup(x => x.GetByIdAsync(columnId)).ReturnsAsync(existingBoardColumn);
        
        var command = new UpdateColumnCommand(boardId, columnId, "New 1", null);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be(command.Name);
        result.Value.Order.Should().Be(existingBoardColumn.Order);

        existingBoardColumn.Name.Should().Be(command.Name);
        
        _columnRepoMock.Verify(x => x.UpdateAsync(It.IsAny<BoardColumn>()), Times.Once);
        _columnRepoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenColumnDoesNotExist()
    {
        // Arrange
        var existingBoard = new Board(){ Name = "TaskFlow", Description = "Desc" };
        var boardId = existingBoard.Id;
        
        var existingBoardColumn = new BoardColumn() { Name = "New 1", Order = 0};
        var columnId = existingBoardColumn.Id;
        
        var command = new UpdateColumnCommand(boardId, columnId, "Task 1", null);
        
        _columnRepoMock.Setup(x => x.GetByIdAsync(columnId)).ReturnsAsync((BoardColumn?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ColumnErrors.NotFound(columnId, boardId));
        
        _columnRepoMock.Verify(x => x.UpdateAsync(It.IsAny<BoardColumn>()), Times.Never);
        _columnRepoMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenSomeFieldsAreNull()
    {
        // Arrange
        var existingBoard = new Board(){ Name = "TaskFlow", Description = "Desc" };
        var boardId = existingBoard.Id;
        
        var existingBoardColumn = new BoardColumn() { Name = "Task 1", Order = 0, BoardId = boardId};
        var columnId = existingBoardColumn.Id;
        
        _columnRepoMock.Setup(x => x.GetByIdAsync(columnId)).ReturnsAsync(existingBoardColumn);
        
        var command = new UpdateColumnCommand(boardId, columnId,"New 1", null);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Name.Should().Be("New 1");
        result.Value.Order.Should().Be(existingBoardColumn.Order);

        
        _columnRepoMock.Verify(x => x.UpdateAsync(It.IsAny<BoardColumn>()), Times.Once);
        _columnRepoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
}
