using MediatR;

namespace TaskFlow.Application.Commands.WorkItems.DeleteWorkItem;

public record DeleteWorkItemCommand(Guid Id) : IRequest<bool>;
