using Moq;
using FluentAssertions;
using TaskFlow.Application.Commands.Boards.CreateBoard;
using TaskFlow.Domain.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.UnitTests.Commands.Boards;

public class CreateBoardCommandHandlerTests
{
    private readonly Mock<IBoardRepository> _boardRepoMock;
    private readonly Mock<IBoardColumnRepository> _columnRepoMock;
    private readonly CreateBoardCommandHandler _handler;

    public CreateBoardCommandHandlerTests()
    {
        _boardRepoMock = new Mock<IBoardRepository>();
        _columnRepoMock = new Mock<IBoardColumnRepository>();
        _handler = new CreateBoardCommandHandler(_boardRepoMock.Object, _columnRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenDataIsValid()
    {
        // Arrange
        var command = new CreateBoardCommand("TaskFlow", "Desc");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Value.Name.Should().Be("TaskFlow");
        _boardRepoMock.Verify(x => x.CreateAsync(It.IsAny<Board>()), Times.Once);
    }
}