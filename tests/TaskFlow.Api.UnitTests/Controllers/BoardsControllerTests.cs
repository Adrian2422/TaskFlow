using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using TaskFlow.Api.Controllers;
using TaskFlow.Application.Commands.Boards.CreateBoard;
using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Queries.Boards.GetAllBoards;
using TaskFlow.Application.Queries.Boards.GetBoardById;
using TaskFlow.Domain.Common;

namespace TaskFlow.Api.UnitTests.Controllers;

public class BoardsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly BoardsController _controller;

    public BoardsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new BoardsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_WhenSuccess()
    {
        // Arrange
        var boards = new List<BoardDto> { new() { Id = Guid.NewGuid(), Name = "Test Board" } };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllBoardsQuery>(), CancellationToken.None))
            .ReturnsAsync(Result.Success(boards));

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.Value.ShouldBe(boards);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenBoardExists()
    {
        // Arrange
        var boardId = Guid.NewGuid();
        var board = new BoardDetailDto { Id = boardId, Name = "Test Board" };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetBoardByIdQuery>(), CancellationToken.None))
            .ReturnsAsync(Result.Success(board));

        // Act
        var result = await _controller.GetById(boardId);

        // Assert
        var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.Value.ShouldBe(board);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenBoardDoesNotExist()
    {
        // Arrange
        var boardId = Guid.NewGuid();
        var error = new Error("Board.NotFound", "Board not found");
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetBoardByIdQuery>(), CancellationToken.None))
            .ReturnsAsync(Result.Failure<BoardDetailDto>(error));

        // Act
        var result = await _controller.GetById(boardId);

        // Assert
        var notFoundResult = result.Result.ShouldBeOfType<NotFoundObjectResult>();
        notFoundResult.Value.ShouldBe(error);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtAction_WhenSuccess()
    {
        // Arrange
        var createDto = new CreateBoardDto { Name = "New Board", Description = "Desc" };
        var boardDto = new BoardDto { Id = Guid.NewGuid(), Name = createDto.Name, Description = createDto.Description };
        
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateBoardCommand>(), CancellationToken.None))
            .ReturnsAsync(Result.Success(boardDto));

        // Act
        var result = await _controller.Create(createDto);

        // Assert
        var createdResult = result.Result.ShouldBeOfType<CreatedAtActionResult>();
        createdResult.ActionName.ShouldBe(nameof(BoardsController.GetById));
        createdResult.RouteValues!["boardId"].ShouldBe(boardDto.Id);
        createdResult.Value.ShouldBe(boardDto);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenFailure()
    {
        // Arrange
        var createDto = new CreateBoardDto { Name = "", Description = "Desc" };
        var error = new Error("Board.Invalid", "Invalid name");
        
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateBoardCommand>(), CancellationToken.None))
            .ReturnsAsync(Result.Failure<BoardDto>(error));

        // Act
        var result = await _controller.Create(createDto);

        // Assert
        var badRequestResult = result.Result.ShouldBeOfType<BadRequestObjectResult>();
        badRequestResult.Value.ShouldBe(error);
    }
}
