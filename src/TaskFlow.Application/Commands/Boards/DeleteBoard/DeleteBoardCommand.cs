using MediatR;
using TaskFlow.Application.Common;

namespace TaskFlow.Application.Commands.Boards.DeleteBoard;

public record DeleteBoardCommand(Guid Id) : IRequest<Result>;
