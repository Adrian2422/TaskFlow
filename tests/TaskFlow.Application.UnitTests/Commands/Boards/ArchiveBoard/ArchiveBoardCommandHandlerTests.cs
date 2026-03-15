using Moq;
using FluentAssertions;
using TaskFlow.Application.Commands.Boards.ArchiveBoard;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Errors;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.UnitTests.Commands.Boards.ArchiveBoard;

public class ArchiveBoardCommandHandlerTests
{
    private readonly Mock<IBoardRepository> _boardRepoMock;
    private readonly ArchiveBoardCommandHandler _handler;

    public ArchiveBoardCommandHandlerTests()
    {
        _boardRepoMock = new Mock<IBoardRepository>();
        _handler = new ArchiveBoardCommandHandler(_boardRepoMock.Object);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenBoardExistAndIsNotArchived()
    {
        // Arrange
        var existingBoard = new Board() { Name = "TaskFlow", Description = "Desc", IsArchived = false};
        var boardId = existingBoard.Id;
        _boardRepoMock.Setup(x => x.GetByIdAsync(boardId)).ReturnsAsync(existingBoard);
        
        var command = new ArchiveBoardCommand
            (boardId);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        existingBoard.IsArchived.Should().BeTrue();
        
        _boardRepoMock.Verify(x => x.UpdateAsync(It.IsAny<Board>()), Times.Once);
        _boardRepoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenBoardExistsAndIsArchived()
    {
        // Arrange
        var existingBoard = new Board() { Name = "TaskFlow", Description = "Desc", IsArchived = true };
        var boardId = existingBoard.Id;
        var command = new ArchiveBoardCommand
            (boardId);
        
        _boardRepoMock.Setup(x => x.GetByIdAsync(boardId)).ReturnsAsync(existingBoard);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BoardErrors.AlreadyArchived(boardId));
        
        _boardRepoMock.Verify(x => x.DeleteAsync(It.IsAny<Board>()), Times.Never);
        _boardRepoMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenBoardDoesNotExist()
    {
        // Arrange
        var boardId = Guid.NewGuid();
        var command = new ArchiveBoardCommand
            (boardId);
        
        _boardRepoMock.Setup(x => x.GetByIdAsync(boardId)).ReturnsAsync((Board?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BoardErrors.NotFound(boardId));
        
        _boardRepoMock.Verify(x => x.UpdateAsync(It.IsAny<Board>()), Times.Never);
        _boardRepoMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
}
