using MediatR;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Queries.Boards.GetAllBoards;

public record GetAllBoardsQuery : IRequest<List<BoardDto>>;
