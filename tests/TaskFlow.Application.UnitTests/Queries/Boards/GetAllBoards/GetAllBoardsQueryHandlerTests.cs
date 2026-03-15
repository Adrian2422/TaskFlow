using Moq;
using FluentAssertions;
using TaskFlow.Application.Queries.Boards.GetAllBoards;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.UnitTests.Queries.Boards.GetAllBoards;

public class GetAllBoardsQueryHandlerTests
{
    private readonly Mock<IBoardRepository> _boardRepoMock;
    private readonly GetAllBoardsQueryHandler _handler;

    public GetAllBoardsQueryHandlerTests()
    {
        _boardRepoMock = new Mock<IBoardRepository>();
        _handler = new GetAllBoardsQueryHandler(_boardRepoMock.Object);
    }
}
