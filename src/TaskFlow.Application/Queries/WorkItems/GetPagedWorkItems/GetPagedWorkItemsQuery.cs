using MediatR;
using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Queries.WorkItems.GetPagedWorkItems;

public record GetPagedWorkItemsQuery(int PageNumber, int PageSize) : IRequest<PagedResult<WorkItemDto>>;
