using MediatR;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Queries.WorkItems.GetAllWorkItems;

public class GetAllWorkItemsQueryHandler : IRequestHandler<GetAllWorkItemsQuery, List<WorkItemDto>>
{
    private readonly IWorkItemRepository _repository;

    public GetAllWorkItemsQueryHandler(IWorkItemRepository repository)
        => _repository = repository;

    public async Task<List<WorkItemDto>> Handle(GetAllWorkItemsQuery request, CancellationToken ct)
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(x => new WorkItemDto
        {
            Id = x.Id,
            Title = x.Title,
            Description = x.Description,
            Order = x.Order
        }).ToList();
    }
}
