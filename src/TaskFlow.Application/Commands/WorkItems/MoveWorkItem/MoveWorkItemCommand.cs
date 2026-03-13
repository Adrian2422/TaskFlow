using MediatR;

namespace TaskFlow.Application.Commands.WorkItems.MoveWorkItem;

public record MoveWorkItemCommand(Guid Id, Guid ColumnId, double? PrevPosition, double? NextPosition) : IRequest;
