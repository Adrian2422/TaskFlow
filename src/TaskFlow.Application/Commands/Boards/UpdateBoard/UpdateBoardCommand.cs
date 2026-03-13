using MediatR;
using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Commands.Boards.UpdateBoard;

public record UpdateBoardCommand(Guid Id, string? Name, string? Description) : IRequest<Result<BoardDto>>;
