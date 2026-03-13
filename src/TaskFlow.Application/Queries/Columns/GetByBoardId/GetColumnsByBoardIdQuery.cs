using MediatR;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Queries.Columns.GetByBoardId;

public record GetColumnsByBoardIdQuery(Guid BoardId) : IRequest<List<ColumnDto>>;
