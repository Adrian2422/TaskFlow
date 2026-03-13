using MediatR;
using TaskFlow.Application.Common;

namespace TaskFlow.Application.Commands.Boards.ArchiveBoard;

public record ArchiveBoardCommand(Guid Id) : IRequest<Result>;
