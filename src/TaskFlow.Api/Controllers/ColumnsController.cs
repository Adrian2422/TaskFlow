using Microsoft.AspNetCore.Mvc;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Commands.Columns.CreateColumn;
using TaskFlow.Application.Commands.Columns.UpdateColumn;
using TaskFlow.Application.Commands.Columns.DeleteColumn;
using TaskFlow.Application.Commands.Columns.MoveColumn;

namespace TaskFlow.Api.Controllers;

[Route("api/boards/{boardId:guid}/columns")]
public class ColumnsController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public ColumnsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create column",
        Description = "Create a new column in the specified board",
        OperationId = "CreateColumn",
        Tags = ["Columns"]
    )]
    public async Task<ActionResult<ColumnDto>> Create(Guid boardId, [FromBody] CreateColumnDto dto)
    {
        var result = await _mediator.Send(new CreateColumnCommand(boardId, dto.Name));
        return HandleResult(result);
    }

    [HttpPut("{columnId:guid}")]
    [SwaggerOperation(
        Summary = "Edit column",
        Description = "Update column name and its order within the board",
        OperationId = "UpdateColumn",
        Tags = ["Columns"]
    )]
    public async Task<ActionResult<ColumnDto>> Update(Guid boardId, Guid columnId, [FromBody] UpdateColumnDto dto)
    {
        var result = await _mediator.Send(new UpdateColumnCommand(boardId, columnId, dto.Name, dto.Order));
        return HandleResult(result);
    }

    [HttpDelete("{columnId:guid}")]
    [SwaggerOperation(
        Summary = "Delete column",
        Description = "Remove a column from the board and move its tasks to the backlog",
        OperationId = "DeleteColumn",
        Tags = ["Columns"]
    )]
    public async Task<ActionResult> Delete(Guid boardId, Guid columnId)
    {
        var result = await _mediator.Send(new DeleteColumnCommand(boardId, columnId));
        return HandleResult(result);
    }

    [HttpPost("{columnId:guid}/move")]
    [SwaggerOperation(
        Summary = "Move column",
        Description = "Change column position within the board",
        OperationId = "MoveColumn",
        Tags = ["Columns"]
    )]
    public async Task<ActionResult> Move(Guid boardId, Guid columnId, [FromBody] MoveColumnDto dto)
    {
        var result = await _mediator.Send(new MoveColumnCommand(boardId, columnId, dto.PrevPosition, dto.NextPosition));
        return HandleResult(result);
    }
}
