using MediatR;
using TaskFlow.Application.Common;

namespace TaskFlow.Application.Commands.WorkItems.ArchiveWorkItem;

public record ArchiveWorkItemCommand(Guid Id) : IRequest<Result>;
