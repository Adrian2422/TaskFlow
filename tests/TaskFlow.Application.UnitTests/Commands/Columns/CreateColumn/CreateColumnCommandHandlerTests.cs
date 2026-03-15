using Moq;
using FluentAssertions;
using TaskFlow.Application.Commands.Columns.CreateColumn;
using TaskFlow.Application.Commands.WorkItems.CreateWorkItem;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Errors;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.UnitTests.Commands.Columns.CreateColumn;

public class CreateColumnCommandHandlerTests
{
    private readonly Mock<IBoardColumnRepository> _columnRepoMock;
    private readonly Mock<IBoardRepository> _boardRepoMock;
    private readonly CreateColumnCommandHandler _handler;

    public CreateColumnCommandHandlerTests()
    {
        _columnRepoMock = new Mock<IBoardColumnRepository>();
        _boardRepoMock = new Mock<IBoardRepository>();
        _handler = new CreateColumnCommandHandler(_columnRepoMock.Object, _boardRepoMock.Object);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenDataIsValidAndBoardExists()
    {
        var existingBoard = new Board(){ Name = "TaskFlow", Description = "Desc" };
        var boardId = existingBoard.Id;
        
        _boardRepoMock.Setup(x => x.GetByIdAsync(boardId)).ReturnsAsync(existingBoard);
        
        // Arrange
        var command = new CreateColumnCommand(boardId, "New");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("New");
        result.Value.Order.Should().Be(0);
        _columnRepoMock.Verify(x => x.CreateAsync(It.IsAny<BoardColumn>()), Times.Once);
        _columnRepoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenDataIsValidButBoardDoesNotExist()
    {
        var boardId = Guid.NewGuid();

        _boardRepoMock.Setup(x => x.GetByIdAsync(boardId)).ReturnsAsync((Board?)null);
        
        // Arrange
        var command = new CreateColumnCommand(boardId, "New");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BoardErrors.NotFound(boardId));
        
        _columnRepoMock.Verify(x => x.CreateAsync(It.IsAny<BoardColumn>()), Times.Never);
        _columnRepoMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
}
