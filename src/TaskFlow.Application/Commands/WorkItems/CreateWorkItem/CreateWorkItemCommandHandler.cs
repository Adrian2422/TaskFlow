using MediatR;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Common;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Errors;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Commands.WorkItems.CreateWorkItem;

public class CreateWorkItemCommandHandler : IRequestHandler<CreateWorkItemCommand, Result<WorkItemDto>>
{
    private readonly IWorkItemRepository _workItemRepository;
    private readonly IBoardRepository _boardRepository;

    public CreateWorkItemCommandHandler(IWorkItemRepository workItemRepository, IBoardRepository boardRepository)
    {
        _workItemRepository = workItemRepository;
        _boardRepository = boardRepository;
    }

    public async Task<Result<WorkItemDto>> Handle(CreateWorkItemCommand request, CancellationToken ct)
    {
        var board = await _boardRepository.GetByIdAsync(request.BoardId);
        if (board == null)
        {
            return Result.Failure<WorkItemDto>(BoardErrors.NotFound(request.BoardId));
        }

        double order;
        if (request.ColumnId.HasValue)
        {
            var maxOrder = await _workItemRepository.GetMaxOrderInColumnAsync(request.ColumnId.Value);
            order = maxOrder.HasValue ? maxOrder.Value + 1000 : 1000;
        }
        else
        {
            var maxOrder = await _workItemRepository.GetMaxOrderInBacklogAsync(request.BoardId);
            order = maxOrder.HasValue ? maxOrder.Value + 1000 : 1000;
        }

        var entity = WorkItem.Create(request.Title, request.Description, request.BoardId, request.ColumnId, order);

        await _workItemRepository.CreateAsync(entity);
        await _workItemRepository.SaveChangesAsync();

        return new WorkItemDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            Order = entity.Order
        };
    }
}
