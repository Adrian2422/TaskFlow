namespace TaskFlow.Application.DTOs;

public class WorkItemDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public double Order { get; set; }
}