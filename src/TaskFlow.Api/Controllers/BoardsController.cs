using Microsoft.AspNetCore.Mvc;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Queries.Boards.GetAllBoards;
using TaskFlow.Application.Queries.Boards.GetBoardById;
using TaskFlow.Application.Commands.Boards.CreateBoard;
using TaskFlow.Application.Commands.Boards.UpdateBoard;
using TaskFlow.Application.Commands.Boards.DeleteBoard;
using TaskFlow.Application.Commands.Boards.ArchiveBoard;
using TaskFlow.Application.Commands.Boards.RestoreBoard;

namespace TaskFlow.Api.Controllers;

[Route("api/boards")]
public class BoardsController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public BoardsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all boards",
        Description = "Retrieve a list of all boards with metadata (no columns or work items)",
        OperationId = "GetAllBoards",
        Tags = ["Boards"]
    )]
    public async Task<ActionResult<List<BoardDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllBoardsQuery());
        return HandleResult(result);
    }

    [HttpGet("{boardId:guid}")]
    [SwaggerOperation(
        Summary = "Get board details",
        Description = "Retrieve full board data including columns and work items (board view)",
        OperationId = "GetBoardDetails",
        Tags = ["Boards"]
    )]
    public async Task<ActionResult<BoardDetailDto>> GetById(Guid boardId)
    {
        var result = await _mediator.Send(new GetBoardByIdQuery(boardId));
        return HandleResult(result);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create board",
        Description = "Create a new board",
        OperationId = "CreateBoard",
        Tags = ["Boards"]
    )]
    public async Task<ActionResult<BoardDto>> Create([FromBody] CreateBoardDto dto)
    {
        var result = await _mediator.Send(new CreateBoardCommand(dto.Name, dto.Description));
        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetById), new { boardId = result.Value.Id }, result.Value);
        }
        return HandleResult(result);
    }

    [HttpPut("{boardId:guid}")]
    [SwaggerOperation(
        Summary = "Edit board",
        Description = "Update board name and description",
        OperationId = "EditBoard",
        Tags = ["Boards"]
    )]
    public async Task<ActionResult<BoardDto>> Update(Guid boardId, [FromBody] UpdateBoardDto dto)
    {
        var result = await _mediator.Send(new UpdateBoardCommand(boardId, dto.Name, dto.Description));
        return HandleResult(result);
    }

    [HttpDelete("{boardId:guid}")]
    [SwaggerOperation(
        Summary = "Delete board",
        Description = "Permanently delete an archived board",
        OperationId = "DeleteBoard",
        Tags = ["Boards"]
    )]
    public async Task<ActionResult> Delete(Guid boardId)
    {
        var result = await _mediator.Send(new DeleteBoardCommand(boardId));
        return HandleResult(result);
    }

    [HttpPost("{boardId:guid}/archive")]
    [SwaggerOperation(
        Summary = "Archive board",
        Description = "Archive a board and its backlog items",
        OperationId = "ArchiveBoard",
        Tags = ["Boards"]
    )]
    public async Task<ActionResult> Archive(Guid boardId)
    {
        var result = await _mediator.Send(new ArchiveBoardCommand(boardId));
        return HandleResult(result);
    }

    [HttpPost("{boardId:guid}/restore")]
    [SwaggerOperation(
        Summary = "Restore board",
        Description = "Restore an archived board",
        OperationId = "RestoreBoard",
        Tags = ["Boards"]
    )]
    public async Task<ActionResult> Restore(Guid boardId)
    {
        var result = await _mediator.Send(new RestoreBoardCommand(boardId));
        return HandleResult(result);
    }
}
