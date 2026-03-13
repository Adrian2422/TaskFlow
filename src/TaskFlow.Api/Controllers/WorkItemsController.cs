using Microsoft.AspNetCore.Mvc;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Queries.WorkItems.GetWorkItemById;
using TaskFlow.Application.Commands.WorkItems.CreateWorkItem;
using TaskFlow.Application.Commands.WorkItems.UpdateWorkItem;
using TaskFlow.Application.Commands.WorkItems.DeleteWorkItem;
using TaskFlow.Application.Commands.WorkItems.MoveWorkItem;
using TaskFlow.Application.Commands.WorkItems.ArchiveWorkItem;
using TaskFlow.Application.Commands.WorkItems.RestoreWorkItem;

namespace TaskFlow.Api.Controllers;

[Route("api/work-items")]
public class WorkItemsController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public WorkItemsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        Summary = "Get WorkItem details",
        Description = "Retrieve details of a specific WorkItem for viewing or editing",
        OperationId = "GetWorkItem",
        Tags = ["WorkItems"]
    )]
    public async Task<ActionResult<WorkItemDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetWorkItemByIdQuery(id));
        return HandleResult(result);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create WorkItem",
        Description = "Create a new WorkItem (default to board backlog if no column is specified)",
        OperationId = "CreateWorkItem",
        Tags = ["WorkItems"]
    )]
    public async Task<ActionResult<WorkItemDto>> Create([FromBody] CreateWorkItemDto dto)
    {
        var result = await _mediator.Send(new CreateWorkItemCommand(dto.Title, dto.Description, dto.BoardId, dto.ColumnId));
        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        }
        return HandleResult(result);
    }

    [HttpPut("{id:guid}")]
    [SwaggerOperation(
        Summary = "Edit WorkItem",
        Description = "Update all data of a specific WorkItem",
        OperationId = "UpdateWorkItem",
        Tags = ["WorkItems"]
    )]
    public async Task<ActionResult<WorkItemDto>> Update(Guid id, [FromBody] UpdateWorkItemDto dto)
    {
        var result = await _mediator.Send(new UpdateWorkItemCommand(id, dto.Title, dto.Description));
        return HandleResult(result);
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(
        Summary = "Delete WorkItem",
        Description = "Permanently delete an archived WorkItem",
        OperationId = "DeleteWorkItem",
        Tags = ["WorkItems"]
    )]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteWorkItemCommand(id));
        return HandleResult(result);
    }

    [HttpPost("{id:guid}/move")]
    [SwaggerOperation(
        Summary = "Move WorkItem",
        Description = "Move a WorkItem to a different column or change its order",
        OperationId = "MoveWorkItem",
        Tags = ["WorkItems"]
    )]
    public async Task<ActionResult> Move(Guid id, [FromBody] MoveWorkItemDto dto)
    {
        var result = await _mediator.Send(new MoveWorkItemCommand(id, dto.ColumnId, dto.PrevPosition, dto.NextPosition));
        return HandleResult(result);
    }

    [HttpPost("{id:guid}/archive")]
    [SwaggerOperation(
        Summary = "Archive WorkItem",
        Description = "Mark a WorkItem as archived",
        OperationId = "ArchiveWorkItem",
        Tags = ["WorkItems"]
    )]
    public async Task<ActionResult> Archive(Guid id)
    {
        var result = await _mediator.Send(new ArchiveWorkItemCommand(id));
        return HandleResult(result);
    }

    [HttpPost("{id:guid}/restore")]
    [SwaggerOperation(
        Summary = "Restore WorkItem",
        Description = "Restore an archived WorkItem",
        OperationId = "RestoreWorkItem",
        Tags = ["WorkItems"]
    )]
    public async Task<ActionResult> Restore(Guid id)
    {
        var result = await _mediator.Send(new RestoreWorkItemCommand(id));
        return HandleResult(result);
    }
}
