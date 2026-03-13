using MediatR;
using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Queries.Boards.GetBoardById;

public record GetBoardByIdQuery(Guid Id) : IRequest<Result<BoardDetailDto>>;
