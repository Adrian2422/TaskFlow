using MediatR;
using TaskFlow.Application.Common;
using TaskFlow.Application.Common.Attributes;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Commands.Boards.CreateBoard;

[Authorize]
public record CreateBoardCommand(string Name, string? Description) : IRequest<Result<BoardDto>>;
