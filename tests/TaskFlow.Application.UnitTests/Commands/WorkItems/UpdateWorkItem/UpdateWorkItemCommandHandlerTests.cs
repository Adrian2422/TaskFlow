using Moq;
using Shouldly;
using TaskFlow.Application.Commands.WorkItems.UpdateWorkItem;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Errors;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.UnitTests.Commands.WorkItems.UpdateWorkItem;

public class UpdateWorkItemCommandHandlerTests
{
    private readonly Mock<IWorkItemRepository> _workItemRepoMock;
    private readonly UpdateWorkItemCommandHandler _handler;

    public UpdateWorkItemCommandHandlerTests()
    {
        _workItemRepoMock = new Mock<IWorkItemRepository>();
        _handler = new UpdateWorkItemCommandHandler(_workItemRepoMock.Object);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenDataIsValid()
    {
        // Arrange
        var existingWorkItem = new WorkItem() { Title = "Task 1", Description = "Desc" };
        var workItemId = existingWorkItem.Id;
        
        _workItemRepoMock.Setup(x => x.GetByIdAsync(workItemId)).ReturnsAsync(existingWorkItem);
        
        var command = new UpdateWorkItemCommand(workItemId, "Task 1", "Desc new");
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Title.ShouldBe(command.Title);
        result.Value.Description.ShouldBe(command.Description);

        existingWorkItem.Title.ShouldBe(command.Title);
        existingWorkItem.Description.ShouldBe(command.Description);
        
        _workItemRepoMock.Verify(x => x.UpdateAsync(It.IsAny<WorkItem>()), Times.Once);
        _workItemRepoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenBoardDoesNotExist()
    {
        // Arrange
        var workItemId = Guid.NewGuid();
        var command = new UpdateWorkItemCommand(workItemId, "Task 1", "Desc new");
        
        _workItemRepoMock.Setup(x => x.GetByIdAsync(workItemId)).ReturnsAsync((WorkItem?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(WorkItemErrors.NotFound(workItemId));
        
        _workItemRepoMock.Verify(x => x.UpdateAsync(It.IsAny<WorkItem>()), Times.Never);
        _workItemRepoMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenSomeFieldsAreNull()
    {
        // Arrange
        var existingWorkItem = new WorkItem() { Title = "Task 1", Description = "Desc" };
        var workItemId = existingWorkItem.Id;
        _workItemRepoMock.Setup(x => x.GetByIdAsync(workItemId)).ReturnsAsync(existingWorkItem);
        
        var command = new UpdateWorkItemCommand(workItemId, null, "Desc new");
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        existingWorkItem.Title.ShouldBe("Task 1");
        existingWorkItem.Description.ShouldBe(command.Description);
        
        _workItemRepoMock.Verify(x => x.UpdateAsync(It.IsAny<WorkItem>()), Times.Once);
        _workItemRepoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
}
