using Moq;
using FluentAssertions;
using TaskFlow.Application.Commands.WorkItems.CreateWorkItem;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Errors;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.UnitTests.Commands.WorkItems.CreateWorkItem;

public class CreateWorkItemCommandHandlerTests
{
    private readonly Mock<IWorkItemRepository> _workItemRepoMock;
    private readonly Mock<IBoardRepository> _boardRepoMock;
    private readonly Mock<IBoardColumnRepository> _boardColumnRepoMock;
    private readonly CreateWorkItemCommandHandler _handler;

    public CreateWorkItemCommandHandlerTests()
    {
        _workItemRepoMock = new Mock<IWorkItemRepository>();
        _boardRepoMock = new Mock<IBoardRepository>();
        _boardColumnRepoMock = new Mock<IBoardColumnRepository>();
        _handler = new CreateWorkItemCommandHandler(_workItemRepoMock.Object, _boardRepoMock.Object);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenDataIsValidAndBoardExists()
    {
        var existingBoard = new Board(){ Name = "TaskFlow", Description = "Desc" };
        var boardId = existingBoard.Id;
        
        _boardRepoMock.Setup(x => x.GetByIdAsync(boardId)).ReturnsAsync(existingBoard);
        
        // Arrange
        var command = new CreateWorkItemCommand("Task 1", "Desc", boardId, null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Title.Should().Be("Task 1");
        result.Value.Order.Should().Be(0);
        _workItemRepoMock.Verify(x => x.CreateAsync(It.IsAny<WorkItem>()), Times.Once);
        _workItemRepoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenDataIsValidButBoardDoesNotExist()
    {
        var boardId = Guid.NewGuid();

        _boardRepoMock.Setup(x => x.GetByIdAsync(boardId)).ReturnsAsync((Board?)null);
        
        // Arrange
        var command = new CreateWorkItemCommand("Task 1", "Desc", boardId, null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BoardErrors.NotFound(boardId));
        
        _workItemRepoMock.Verify(x => x.CreateAsync(It.IsAny<WorkItem>()), Times.Never);
        _workItemRepoMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenDataIsValidAndBoardExistsAndColumnIsSet()
    {
        var existingBoard = new Board(){ Name = "TaskFlow", Description = "Desc" };
        var boardId = existingBoard.Id;
        
        var existingColumn = new BoardColumn(){ Name = "Task 1" };
        var columnId = existingColumn.Id;
        
        _boardRepoMock.Setup(x => x.GetByIdAsync(boardId)).ReturnsAsync(existingBoard);
        _boardColumnRepoMock.Setup(x => x.GetByIdAsync(columnId)).ReturnsAsync(existingColumn);
        
        // Arrange
        var command = new CreateWorkItemCommand("Task 1", "Desc", boardId, columnId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _workItemRepoMock.Verify(x => x.CreateAsync(It.Is<WorkItem>(wi => wi.ColumnId == columnId)), Times.Once);
        _workItemRepoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
}
