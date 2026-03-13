using MediatR;

namespace TaskFlow.Application.Commands.Boards.DeleteBoard;

public record DeleteBoardCommand(Guid Id) : IRequest<bool>;
