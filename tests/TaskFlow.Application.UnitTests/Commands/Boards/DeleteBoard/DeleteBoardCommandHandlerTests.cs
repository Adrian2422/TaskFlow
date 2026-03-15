using Moq;
using Shouldly;
using TaskFlow.Application.Commands.Boards.DeleteBoard;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Errors;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.UnitTests.Commands.Boards.DeleteBoard;

public class DeleteBoardCommandHandlerTests
{
    private readonly Mock<IBoardRepository> _boardRepoMock;
    private readonly DeleteBoardCommandHandler _handler;

    public DeleteBoardCommandHandlerTests()
    {
        _boardRepoMock = new Mock<IBoardRepository>();
        _handler = new DeleteBoardCommandHandler(_boardRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenBoardExistsAndIsArchived()
    {
        // Arrange
        var existingBoard = new Board() { Name = "TaskFlow", Description = "Desc", IsArchived = true };
        var boardId = existingBoard.Id;
        var command = new DeleteBoardCommand(boardId);
        
        _boardRepoMock.Setup(x => x.GetByIdAsync(boardId)).ReturnsAsync(existingBoard);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.ShouldBeTrue();
        
        _boardRepoMock.Verify(x => x.DeleteAsync(It.IsAny<Board>()), Times.Once);
        _boardRepoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenBoardExistsAndIsNotArchived()
    {
        // Arrange
        var existingBoard = new Board() { Name = "TaskFlow", Description = "Desc", IsArchived = false };
        var boardId = existingBoard.Id;
        var command = new DeleteBoardCommand(boardId);
        
        _boardRepoMock.Setup(x => x.GetByIdAsync(boardId)).ReturnsAsync(existingBoard);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(BoardErrors.DeleteNotArchived);
        
        _boardRepoMock.Verify(x => x.DeleteAsync(It.IsAny<Board>()), Times.Never);
        _boardRepoMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenBoardDoesNotExists()
    {
        // Arrange
        var boardId = Guid.NewGuid();
        var command = new DeleteBoardCommand(boardId);
        
        _boardRepoMock.Setup(x => x.GetByIdAsync(boardId)).ReturnsAsync((Board?)null);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(BoardErrors.NotFound(boardId));
        
        _boardRepoMock.Verify(x => x.DeleteAsync(It.IsAny<Board>()), Times.Never);
        _boardRepoMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
}
