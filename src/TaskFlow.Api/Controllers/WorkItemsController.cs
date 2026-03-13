using Microsoft.AspNetCore.Mvc;
using MediatR;
using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Queries.WorkItems.GetAllWorkItems;
using TaskFlow.Application.Queries.WorkItems.GetPagedWorkItems;
using TaskFlow.Application.Queries.WorkItems.GetWorkItemById;
using TaskFlow.Application.Commands.WorkItems.CreateWorkItem;
using TaskFlow.Application.Commands.WorkItems.UpdateWorkItem;
using TaskFlow.Application.Commands.WorkItems.DeleteWorkItem;
using TaskFlow.Application.Commands.WorkItems.MoveWorkItem;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/work-items")]
public class WorkItemsController : ControllerBase
{
    private readonly IMediator _mediator;

    public WorkItemsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<ActionResult<PagedResult<WorkItemDto>>> GetWorkItems([FromQuery] PaginationQuery query)
    {
        var result = await _mediator.Send(new GetPagedWorkItemsQuery(query.PageNumber, query.PageSize));
        return Ok(result);
    }
    
    [HttpGet("all")]
    public async Task<ActionResult<List<WorkItemDto>>> GetAllWorkItems()
    {
        var workItems = await _mediator.Send(new GetAllWorkItemsQuery());
        return Ok(workItems);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WorkItemDto>> GetById(Guid id)
    {
        var item = await _mediator.Send(new GetWorkItemByIdQuery(id));

        if (item == null)
            return NotFound();

        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<WorkItemDto>> Create(CreateWorkItemDto dto)
    {
        var result = await _mediator.Send(new CreateWorkItemCommand(dto.Title, dto.Description, dto.ColumnId));

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<WorkItemDto>> Update(Guid id, UpdateWorkItemDto dto)
    {
        var result = await _mediator.Send(new UpdateWorkItemCommand(id, dto.Title, dto.Description));

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var deleted = await _mediator.Send(new DeleteWorkItemCommand(id));

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpPost("{id}/move")]
    public async Task<ActionResult> Move(Guid id, [FromBody] MoveWorkItemDto dto)
    {
        var item = await _mediator.Send(new GetWorkItemByIdQuery(id));
        if (item == null)
            return NotFound();

        await _mediator.Send(new MoveWorkItemCommand(id, dto.ColumnId, dto.PrevPosition, dto.NextPosition));

        return NoContent();
    }
}