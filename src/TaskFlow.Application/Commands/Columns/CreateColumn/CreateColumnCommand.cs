using MediatR;
using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Commands.Columns.CreateColumn;

public record CreateColumnCommand(Guid BoardId, string Name) : IRequest<Result<ColumnDto>>;
