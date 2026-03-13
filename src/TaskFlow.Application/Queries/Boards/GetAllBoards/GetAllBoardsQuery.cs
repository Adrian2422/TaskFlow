using MediatR;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Common;

namespace TaskFlow.Application.Queries.Boards.GetAllBoards;

public record GetAllBoardsQuery : IRequest<Result<List<BoardDto>>>;
