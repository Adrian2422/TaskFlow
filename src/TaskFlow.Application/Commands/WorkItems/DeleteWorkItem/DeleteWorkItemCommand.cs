using MediatR;
using TaskFlow.Application.Common;

namespace TaskFlow.Application.Commands.WorkItems.DeleteWorkItem;

public record DeleteWorkItemCommand(Guid Id) : IRequest<Result>;
