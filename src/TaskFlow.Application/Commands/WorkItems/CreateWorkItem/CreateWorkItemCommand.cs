using MediatR;
using TaskFlow.Application.DTOs;

using TaskFlow.Application.Common;

namespace TaskFlow.Application.Commands.WorkItems.CreateWorkItem;

public record CreateWorkItemCommand(string Title, string? Description, Guid BoardId, Guid? ColumnId) : IRequest<Result<WorkItemDto>>;
