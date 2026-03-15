using Moq;
using FluentAssertions;
using TaskFlow.Application.Commands.WorkItems.ArchiveWorkItem;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Errors;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.UnitTests.Commands.WorkItems.ArchiveWorkItem;

public class ArchiveWorkItemCommandHandlerTests
{
    private readonly Mock<IWorkItemRepository> _workItemRepoMock;
    private readonly ArchiveWorkItemCommandHandler _handler;

    public ArchiveWorkItemCommandHandlerTests()
    {
        _workItemRepoMock = new Mock<IWorkItemRepository>();
        _handler = new ArchiveWorkItemCommandHandler(_workItemRepoMock.Object);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenWorkItemExistAndIsNotArchived()
    {
        // Arrange
        var existingWorkItem = new WorkItem() { Title = "TaskFlow", Description = "Desc", IsArchived = false};
        var workItemId = existingWorkItem.Id;
        _workItemRepoMock.Setup(x => x.GetByIdAsync(workItemId)).ReturnsAsync(existingWorkItem);
        
        var command = new ArchiveWorkItemCommand
            (workItemId);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        existingWorkItem.IsArchived.Should().BeTrue();
        
        _workItemRepoMock.Verify(x => x.UpdateAsync(It.IsAny<WorkItem>()), Times.Once);
        _workItemRepoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenWorkItemExistsAndIsArchived()
    {
        // Arrange
        var existingWorkItem = new WorkItem() { Title = "TaskFlow", Description = "Desc", IsArchived = true };
        var workItemId = existingWorkItem.Id;
        var command = new ArchiveWorkItemCommand
            (workItemId);
        
        _workItemRepoMock.Setup(x => x.GetByIdAsync(workItemId)).ReturnsAsync(existingWorkItem);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(WorkItemErrors.AlreadyArchived(workItemId));
        
        _workItemRepoMock.Verify(x => x.DeleteAsync(It.IsAny<WorkItem>()), Times.Never);
        _workItemRepoMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenWorkItemDoesNotExist()
    {
        // Arrange
        var workItemId = Guid.NewGuid();
        var command = new ArchiveWorkItemCommand
            (workItemId);
        
        _workItemRepoMock.Setup(x => x.GetByIdAsync(workItemId)).ReturnsAsync((WorkItem?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(WorkItemErrors.NotFound(workItemId));
        
        _workItemRepoMock.Verify(x => x.UpdateAsync(It.IsAny<WorkItem>()), Times.Never);
        _workItemRepoMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
}
