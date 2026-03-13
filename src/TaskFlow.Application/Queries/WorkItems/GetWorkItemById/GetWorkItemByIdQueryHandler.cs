using MediatR;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Queries.WorkItems.GetWorkItemById;

public class GetWorkItemByIdQueryHandler : IRequestHandler<GetWorkItemByIdQuery, WorkItemDto?>
{
    private readonly IWorkItemRepository _repository;

    public GetWorkItemByIdQueryHandler(IWorkItemRepository repository)
        => _repository = repository;

    public async Task<WorkItemDto?> Handle(GetWorkItemByIdQuery request, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        return entity == null ? null : new WorkItemDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            Order = entity.Order
        };
    }
}
