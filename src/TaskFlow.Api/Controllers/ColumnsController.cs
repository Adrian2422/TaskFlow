using Microsoft.AspNetCore.Mvc;
using MediatR;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Queries.Columns.GetByBoardId;
using TaskFlow.Application.Commands.Columns.CreateColumn;
using TaskFlow.Application.Commands.Columns.UpdateColumn;
using TaskFlow.Application.Commands.Columns.DeleteColumn;
using TaskFlow.Application.Commands.Columns.MoveColumn;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/boards/{boardId:guid}/columns")]
public class ColumnsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ColumnsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<ColumnDto>>> GetAll(Guid boardId)
    {
        var columns = await _mediator.Send(new GetColumnsByBoardIdQuery(boardId));
        return Ok(columns);
    }

    [HttpPost]
    public async Task<ActionResult<ColumnDto>> Create(Guid boardId, [FromBody] CreateColumnDto dto)
    {
        var result = await _mediator.Send(new CreateColumnCommand(boardId, dto.Name));
        return Created(string.Empty, result);
    }

    [HttpPut("{columnId:guid}")]
    public async Task<ActionResult<ColumnDto>> Update(Guid boardId, Guid columnId, [FromBody] UpdateColumnDto dto)
    {
        var result = await _mediator.Send(new UpdateColumnCommand(boardId, columnId, dto.Name, dto.Order));
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{columnId:guid}")]
    public async Task<ActionResult> Delete(Guid boardId, Guid columnId)
    {
        try
        {
            var deleted = await _mediator.Send(new DeleteColumnCommand(boardId, columnId));
            if (!deleted)
                return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("{columnId:guid}/move")]
    public async Task<ActionResult> Move(Guid boardId, Guid columnId, [FromBody] MoveColumnDto dto)
    {
        await _mediator.Send(new MoveColumnCommand(boardId, columnId, dto.PrevPosition, dto.NextPosition));
        return NoContent();
    }
}
