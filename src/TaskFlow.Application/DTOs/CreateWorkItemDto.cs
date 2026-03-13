namespace TaskFlow.Application.DTOs;

public class CreateWorkItemDto
{
    public required string Title { get; init; }
    public string? Description { get; init; }
    public required Guid BoardId { get; init; }
    public Guid? ColumnId { get; init; }
}