using Moq;
using Shouldly;
using TaskFlow.Application.Commands.Boards.CreateBoard;
using TaskFlow.Domain.Interfaces;
using TaskFlow.Domain.Entities;

using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.UnitTests.Commands.Boards.CreateBoard;

public class CreateBoardCommandHandlerTests
{
    private readonly Mock<IBoardRepository> _boardRepoMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly CreateBoardCommandHandler _handler;

    public CreateBoardCommandHandlerTests()
    {
        _boardRepoMock = new Mock<IBoardRepository>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _handler = new CreateBoardCommandHandler(_boardRepoMock.Object, _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenDataIsValid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _currentUserServiceMock.Setup(x => x.UserId).Returns(userId);
        var command = new CreateBoardCommand("TaskFlow", "Desc");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Value.Name.ShouldBe("TaskFlow");
        _boardRepoMock.Verify(x => x.CreateAsync(It.IsAny<Board>()), Times.Once);
    }
}
