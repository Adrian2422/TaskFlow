using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using TaskFlow.Api.Controllers;
using TaskFlow.Application.Commands.WorkItems.CreateWorkItem;
using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Queries.WorkItems.GetWorkItemById;
using TaskFlow.Domain.Common;

namespace TaskFlow.Api.UnitTests.Controllers;

public class WorkItemsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly WorkItemsController _controller;

    public WorkItemsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new WorkItemsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenWorkItemExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new WorkItemDto { Id = id, Title = "Task 1" };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetWorkItemByIdQuery>(), CancellationToken.None))
            .ReturnsAsync(Result.Success(dto));

        // Act
        var result = await _controller.GetById(id);

        // Assert
        var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.Value.ShouldBe(dto);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtAction_WhenSuccess()
    {
        // Arrange
        var createDto = new CreateWorkItemDto { Title = "New Task", BoardId = Guid.NewGuid() };
        var workItemDto = new WorkItemDto { Id = Guid.NewGuid(), Title = createDto.Title };
        
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateWorkItemCommand>(), CancellationToken.None))
            .ReturnsAsync(Result.Success(workItemDto));

        // Act
        var result = await _controller.Create(createDto);

        // Assert
        var createdResult = result.Result.ShouldBeOfType<CreatedAtActionResult>();
        createdResult.ActionName.ShouldBe(nameof(WorkItemsController.GetById));
        createdResult.RouteValues!["id"].ShouldBe(workItemDto.Id);
        createdResult.Value.ShouldBe(workItemDto);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenFailure()
    {
        // Arrange
        var createDto = new CreateWorkItemDto { Title = "", BoardId = Guid.NewGuid() };
        var error = new Error("WorkItem.Invalid", "Invalid title");
        
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateWorkItemCommand>(), CancellationToken.None))
            .ReturnsAsync(Result.Failure<WorkItemDto>(error));

        // Act
        var result = await _controller.Create(createDto);

        // Assert
        var badRequestResult = result.Result.ShouldBeOfType<BadRequestObjectResult>();
        badRequestResult.Value.ShouldBe(error);
    }
}
