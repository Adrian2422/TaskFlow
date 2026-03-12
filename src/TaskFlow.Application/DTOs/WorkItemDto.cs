using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.DTOs;

public class WorkItemDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public WorkItemStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}