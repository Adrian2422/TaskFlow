using MediatR;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Queries.Boards.GetBoardById;

public record GetBoardByIdQuery(Guid Id) : IRequest<BoardDetailDto?>;
