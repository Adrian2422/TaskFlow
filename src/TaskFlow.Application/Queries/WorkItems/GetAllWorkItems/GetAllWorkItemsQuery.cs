using MediatR;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Queries.WorkItems.GetAllWorkItems;

public record GetAllWorkItemsQuery : IRequest<List<WorkItemDto>>;
