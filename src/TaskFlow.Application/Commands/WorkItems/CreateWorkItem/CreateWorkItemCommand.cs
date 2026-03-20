using MediatR;
using TaskFlow.Application.DTOs;

using TaskFlow.Application.Common;
using TaskFlow.Application.Common.Attributes;

namespace TaskFlow.Application.Commands.WorkItems.CreateWorkItem;

[Authorize]
public record CreateWorkItemCommand(string Title, string? Description, Guid BoardId, Guid? ColumnId, Guid? AssignedToId = null) : IRequest<Result<WorkItemDto>>;
