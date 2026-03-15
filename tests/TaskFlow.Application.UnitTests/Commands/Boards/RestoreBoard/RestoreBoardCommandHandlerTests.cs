using Moq;
using Shouldly;
using TaskFlow.Application.Commands.Boards.RestoreBoard;
using TaskFlow.Application.Commands.Boards.UpdateBoard;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Errors;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.UnitTests.Commands.Boards.RestoreBoard;

public class RestoreBoardCommandHandlerTests
{
    private readonly Mock<IBoardRepository> _boardRepoMock;
    private readonly RestoreBoardCommandHandler _handler;

    public RestoreBoardCommandHandlerTests()
    {
        _boardRepoMock = new Mock<IBoardRepository>();
        _handler = new RestoreBoardCommandHandler(_boardRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenBoardExistAndIsArchived()
    {
        // Arrange
        var existingBoard = new Board() { Name = "TaskFlow", Description = "Desc", IsArchived = true};
        var boardId = existingBoard.Id;
        _boardRepoMock.Setup(x => x.GetByIdAsync(boardId)).ReturnsAsync(existingBoard);
        
        var command = new RestoreBoardCommand(boardId);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        existingBoard.IsArchived.ShouldBeFalse();
        
        _boardRepoMock.Verify(x => x.UpdateAsync(It.IsAny<Board>()), Times.Once);
        _boardRepoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenBoardExistsAndIsNotArchived()
    {
        // Arrange
        var existingBoard = new Board() { Name = "TaskFlow", Description = "Desc", IsArchived = false };
        var boardId = existingBoard.Id;
        var command = new RestoreBoardCommand(boardId);
        
        _boardRepoMock.Setup(x => x.GetByIdAsync(boardId)).ReturnsAsync(existingBoard);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(BoardErrors.RestoreNotArchived);
        
        _boardRepoMock.Verify(x => x.DeleteAsync(It.IsAny<Board>()), Times.Never);
        _boardRepoMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenBoardDoesNotExist()
    {
        // Arrange
        var boardId = Guid.NewGuid();
        var command = new RestoreBoardCommand(boardId);
        
        _boardRepoMock.Setup(x => x.GetByIdAsync(boardId)).ReturnsAsync((Board?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(BoardErrors.NotFound(boardId));
        
        _boardRepoMock.Verify(x => x.UpdateAsync(It.IsAny<Board>()), Times.Never);
        _boardRepoMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
}
