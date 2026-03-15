using Moq;
using Shouldly;
using TaskFlow.Application.Queries.WorkItems.GetWorkItemById;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.UnitTests.Queries.WorkItems.GetWorkItemById;

public class GetWorkItemByIdQueryHandlerTests
{
    private readonly Mock<IWorkItemRepository> _workItemRepoMock;
    private readonly GetWorkItemByIdQueryHandler _handler;

    public GetWorkItemByIdQueryHandlerTests()
    {
        _workItemRepoMock = new Mock<IWorkItemRepository>();
        _handler = new GetWorkItemByIdQueryHandler(_workItemRepoMock.Object);
    }
}
