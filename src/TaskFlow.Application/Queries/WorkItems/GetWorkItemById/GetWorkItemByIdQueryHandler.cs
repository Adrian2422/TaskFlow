using MediatR;
using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Errors;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Queries.WorkItems.GetWorkItemById;

public class GetWorkItemByIdQueryHandler : IRequestHandler<GetWorkItemByIdQuery, Result<WorkItemDto>>
{
    private readonly IWorkItemRepository _repository;

    public GetWorkItemByIdQueryHandler(IWorkItemRepository repository)
        => _repository = repository;

    public async Task<Result<WorkItemDto>> Handle(GetWorkItemByIdQuery request, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            return Result.Failure<WorkItemDto>(WorkItemErrors.NotFound(request.Id));
        }

        return new WorkItemDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            Order = entity.Order
        };
    }
}
