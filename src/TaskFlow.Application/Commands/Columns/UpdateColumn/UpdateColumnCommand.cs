using MediatR;
using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Commands.Columns.UpdateColumn;

public record UpdateColumnCommand(Guid BoardId, Guid ColumnId, string? Name, double? Order) : IRequest<Result<ColumnDto>>;
