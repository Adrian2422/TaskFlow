using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using TaskFlow.Api.Controllers;
using TaskFlow.Application.Commands.Columns.CreateColumn;
using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Common;

namespace TaskFlow.Api.UnitTests.Controllers;

public class ColumnsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ColumnsController _controller;

    public ColumnsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ColumnsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task Create_ShouldReturnOk_WhenSuccess()
    {
        // Arrange
        var boardId = Guid.NewGuid();
        var dto = new CreateColumnDto { Name = "New Column" };
        var columnDto = new ColumnDto { Id = Guid.NewGuid(), Name = dto.Name };
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateColumnCommand>(), CancellationToken.None))
            .ReturnsAsync(Result.Success(columnDto));

        // Act
        var result = await _controller.Create(boardId, dto);

        // Assert
        var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.Value.ShouldBe(columnDto);
    }

    [Fact]
    public async Task Create_ShouldReturnNotFound_WhenBoardDoesNotExist()
    {
        // Arrange
        var boardId = Guid.NewGuid();
        var dto = new CreateColumnDto { Name = "New Column" };
        var error = new Error("Board.NotFound", "Board not found");
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateColumnCommand>(), CancellationToken.None))
            .ReturnsAsync(Result.Failure<ColumnDto>(error));

        // Act
        var result = await _controller.Create(boardId, dto);

        // Assert
        var notFoundResult = result.Result.ShouldBeOfType<NotFoundObjectResult>();
        notFoundResult.Value.ShouldBe(error);
    }
}
