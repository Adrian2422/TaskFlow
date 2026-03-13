using MediatR;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Commands.WorkItems.CreateWorkItem;

public record CreateWorkItemCommand(string Title, string? Description, Guid ColumnId) : IRequest<WorkItemDto>;
