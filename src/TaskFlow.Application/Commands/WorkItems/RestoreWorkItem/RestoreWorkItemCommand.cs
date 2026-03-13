using MediatR;
using TaskFlow.Application.Common;

namespace TaskFlow.Application.Commands.WorkItems.RestoreWorkItem;

public record RestoreWorkItemCommand(Guid Id) : IRequest<Result>;
