using Microsoft.AspNetCore.Mvc;
using MediatR;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Queries.Boards.GetAllBoards;
using TaskFlow.Application.Queries.Boards.GetBoardById;
using TaskFlow.Application.Commands.Boards.CreateBoard;
using TaskFlow.Application.Commands.Boards.UpdateBoard;
using TaskFlow.Application.Commands.Boards.DeleteBoard;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/boards")]
public class BoardsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BoardsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<BoardDto>>> GetAll()
    {
        var boards = await _mediator.Send(new GetAllBoardsQuery());
        return Ok(boards);
    }

    [HttpGet("{boardId:guid}")]
    public async Task<ActionResult<BoardDetailDto>> GetById(Guid boardId)
    {
        var board = await _mediator.Send(new GetBoardByIdQuery(boardId));
        if (board == null)
            return NotFound();

        return Ok(board);
    }

    [HttpPost]
    public async Task<ActionResult<BoardDto>> Create([FromBody] CreateBoardDto dto)
    {
        var result = await _mediator.Send(new CreateBoardCommand(dto.Name, dto.Description));
        return CreatedAtAction(nameof(GetById), new { boardId = result.Id }, result);
    }

    [HttpPut("{boardId:guid}")]
    public async Task<ActionResult<BoardDto>> Update(Guid boardId, [FromBody] UpdateBoardDto dto)
    {
        var result = await _mediator.Send(new UpdateBoardCommand(boardId, dto.Name, dto.Description));
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{boardId:guid}")]
    public async Task<ActionResult> Delete(Guid boardId)
    {
        var deleted = await _mediator.Send(new DeleteBoardCommand(boardId));
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
