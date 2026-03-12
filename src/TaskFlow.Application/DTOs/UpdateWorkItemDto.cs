using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.DTOs;

public class UpdateWorkItemDto
{
    public string? Title { get; init; }
    public string? Description { get; init; }
    public WorkItemStatus? Status { get; init; }
}