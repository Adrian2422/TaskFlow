using MediatR;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Commands.WorkItems.UpdateWorkItem;

public record UpdateWorkItemCommand(Guid Id, string? Title, string? Description) : IRequest<WorkItemDto?>;
