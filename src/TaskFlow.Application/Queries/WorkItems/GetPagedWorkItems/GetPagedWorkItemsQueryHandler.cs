using MediatR;
using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Queries.WorkItems.GetPagedWorkItems;

public class GetPagedWorkItemsQueryHandler : IRequestHandler<GetPagedWorkItemsQuery, PagedResult<WorkItemDto>>
{
    private readonly IWorkItemRepository _repository;

    public GetPagedWorkItemsQueryHandler(IWorkItemRepository repository)
        => _repository = repository;

    public async Task<PagedResult<WorkItemDto>> Handle(GetPagedWorkItemsQuery request, CancellationToken ct)
    {
        var (items, totalCount) = await _repository.GetPagedAsync(request.PageNumber, request.PageSize);

        return new PagedResult<WorkItemDto>
        {
            Items = items.Select(x => new WorkItemDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Order = x.Order
            }).ToList(),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
