using Moq;
using Shouldly;
using TaskFlow.Application.Commands.Columns.MoveColumn;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.UnitTests.Commands.Columns.MoveColumn;

public class MoveColumnCommandHandlerTests
{
    private readonly Mock<IBoardColumnRepository> _boardColumnRepoMock;
    private readonly Mock<IBoardRepository> _boardRepoMock;
    private readonly MoveColumnCommandHandler _handler;

    public MoveColumnCommandHandlerTests()
    {
        _boardColumnRepoMock = new Mock<IBoardColumnRepository>();
        _boardRepoMock = new Mock<IBoardRepository>();
        _handler = new MoveColumnCommandHandler(_boardColumnRepoMock.Object);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenBoardExists()
    {
        // Arrange
        var boardId = Guid.NewGuid();
        var existingBoardColumn = new BoardColumn() { Name = "New", Order = 2000, BoardId = boardId };
        var boardColumnId = existingBoardColumn.Id;

        _boardColumnRepoMock.Setup(x => x.GetByIdAsync(boardColumnId)).ReturnsAsync(existingBoardColumn);

        var command = new MoveColumnCommand(boardId, boardColumnId, 0, 1000);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        _boardColumnRepoMock.Verify(x => x.UpdateAsync(It.Is<BoardColumn>(bc => 
            bc.BoardId == boardId && 
            bc.Order.Equals(500)
        )), Times.Once);
        _boardColumnRepoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
}
