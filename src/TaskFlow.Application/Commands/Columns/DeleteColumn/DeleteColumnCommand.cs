using MediatR;
using TaskFlow.Application.Common;

namespace TaskFlow.Application.Commands.Columns.DeleteColumn;

public record DeleteColumnCommand(Guid BoardId, Guid ColumnId) : IRequest<Result>;
