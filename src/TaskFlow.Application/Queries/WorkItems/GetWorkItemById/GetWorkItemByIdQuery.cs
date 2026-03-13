using MediatR;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Queries.WorkItems.GetWorkItemById;

public record GetWorkItemByIdQuery(Guid Id) : IRequest<WorkItemDto?>;
