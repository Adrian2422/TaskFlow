using Moq;
using FluentAssertions;
using TaskFlow.Application.Commands.WorkItems.DeleteWorkItem;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Errors;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.UnitTests.Commands.WorkItems.DeleteWorkItem;

public class DeleteWorkItemCommandHandlerTests
{
    private readonly Mock<IWorkItemRepository> _workItemRepoMock;
    private readonly DeleteWorkItemCommandHandler _handler;

    public DeleteWorkItemCommandHandlerTests()
    {
        _workItemRepoMock = new Mock<IWorkItemRepository>();
        _handler = new DeleteWorkItemCommandHandler(_workItemRepoMock.Object);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenWorkItemExistsAndIsArchived()
    {
        // Arrange
        var existingWorkItem = new WorkItem() { Title = "Task 1", Description = "Desc", IsArchived = true };
        var workItemId = existingWorkItem.Id;
        var command = new DeleteWorkItemCommand(workItemId);
        
        _workItemRepoMock.Setup(x => x.GetByIdAsync(workItemId)).ReturnsAsync(existingWorkItem);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        
        _workItemRepoMock.Verify(x => x.DeleteAsync(It.IsAny<WorkItem>()), Times.Once);
        _workItemRepoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenWorkItemExistsAndIsNotArchived()
    {
        // Arrange
        var existingWorkItem = new WorkItem() { Title = "Task 1", Description = "Desc", IsArchived = false };
        var workItemId = existingWorkItem.Id;
        var command = new DeleteWorkItemCommand(workItemId);
        
        _workItemRepoMock.Setup(x => x.GetByIdAsync(workItemId)).ReturnsAsync(existingWorkItem);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(WorkItemErrors.DeleteNotArchived);
        
        _workItemRepoMock.Verify(x => x.DeleteAsync(It.IsAny<WorkItem>()), Times.Never);
        _workItemRepoMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenWorkItemDoesNotExists()
    {
        // Arrange
        var workItemId = Guid.NewGuid();
        var command = new DeleteWorkItemCommand(workItemId);
        
        _workItemRepoMock.Setup(x => x.GetByIdAsync(workItemId)).ReturnsAsync((WorkItem?)null);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(WorkItemErrors.NotFound(workItemId));
        
        _workItemRepoMock.Verify(x => x.DeleteAsync(It.IsAny<WorkItem>()), Times.Never);
        _workItemRepoMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
}
