using Moq;
using Shouldly;
using TaskFlow.Application.Commands.Boards.UpdateBoard;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Errors;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.UnitTests.Commands.Boards.UpdateBoard;

public class UpdateBoardCommandHandlerTests
{
    private readonly Mock<IBoardRepository> _boardRepoMock;
    private readonly UpdateBoardCommandHandler _handler;

    public UpdateBoardCommandHandlerTests()
    {
        _boardRepoMock = new Mock<IBoardRepository>();
        _handler = new UpdateBoardCommandHandler(_boardRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenDataIsValid()
    {
        // Arrange
        var existingBoard = new Board() { Name = "TaskFlow", Description = "Desc" };
        var boardId = existingBoard.Id;
        
        _boardRepoMock.Setup(x => x.GetByIdAsync(boardId)).ReturnsAsync(existingBoard);
        
        var command = new UpdateBoardCommand(boardId, "TaskFlow", "Desc new");
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Name.ShouldBe(command.Name);
        result.Value.Description.ShouldBe(command.Description);

        existingBoard.Name.ShouldBe(command.Name);
        existingBoard.Description.ShouldBe(command.Description);
        
        _boardRepoMock.Verify(x => x.UpdateAsync(It.IsAny<Board>()), Times.Once);
        _boardRepoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenBoardDoesNotExist()
    {
        // Arrange
        var boardId = Guid.NewGuid();
        var command = new UpdateBoardCommand(boardId, "TaskFlow", "Desc new");
        
        _boardRepoMock.Setup(x => x.GetByIdAsync(boardId)).ReturnsAsync((Board?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(BoardErrors.NotFound(boardId));
        
        _boardRepoMock.Verify(x => x.UpdateAsync(It.IsAny<Board>()), Times.Never);
        _boardRepoMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenSomeFieldsAreNull()
    {
        // Arrange
        var existingBoard = new Board() { Name = "TaskFlow", Description = "Desc" };
        var boardId = existingBoard.Id;
        _boardRepoMock.Setup(x => x.GetByIdAsync(boardId)).ReturnsAsync(existingBoard);
        
        var command = new UpdateBoardCommand(boardId, null, "Desc new");
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        existingBoard.Name.ShouldBe("TaskFlow");
        existingBoard.Description.ShouldBe(command.Description);
        
        _boardRepoMock.Verify(x => x.UpdateAsync(It.IsAny<Board>()), Times.Once);
        _boardRepoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
}
