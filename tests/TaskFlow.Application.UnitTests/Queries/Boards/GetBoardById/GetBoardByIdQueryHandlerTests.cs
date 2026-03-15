using Moq;
using FluentAssertions;
using TaskFlow.Application.Queries.Boards.GetBoardById;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.UnitTests.Queries.Boards.GetBoardById;

public class GetBoardByIdQueryHandlerTests
{
    private readonly Mock<IBoardRepository> _boardRepoMock;
    private readonly GetBoardByIdQueryHandler _handler;

    public GetBoardByIdQueryHandlerTests()
    {
        _boardRepoMock = new Mock<IBoardRepository>();
        _handler = new GetBoardByIdQueryHandler(_boardRepoMock.Object);
    }
}
