using MediatR;
using TaskFlow.Application.Common;

namespace TaskFlow.Application.Commands.Boards.RestoreBoard;

public record RestoreBoardCommand(Guid Id) : IRequest<Result>;
