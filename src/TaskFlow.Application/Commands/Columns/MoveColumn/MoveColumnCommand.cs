using MediatR;
using TaskFlow.Application.Common;

namespace TaskFlow.Application.Commands.Columns.MoveColumn;

public record MoveColumnCommand(Guid BoardId, Guid ColumnId, double? PrevPosition, double? NextPosition) : IRequest<Result>;
