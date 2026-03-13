using MediatR;

namespace TaskFlow.Application.Commands.Columns.DeleteColumn;

public record DeleteColumnCommand(Guid BoardId, Guid ColumnId) : IRequest<bool>;
