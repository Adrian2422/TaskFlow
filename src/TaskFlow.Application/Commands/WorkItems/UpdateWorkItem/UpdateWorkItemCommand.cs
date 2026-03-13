using MediatR;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Common;

namespace TaskFlow.Application.Commands.WorkItems.UpdateWorkItem;

public record UpdateWorkItemCommand(Guid Id, string? Title, string? Description) : IRequest<Result<WorkItemDto>>;
