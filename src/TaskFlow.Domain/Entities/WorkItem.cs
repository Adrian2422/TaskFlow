using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entities;

public class WorkItem
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public WorkItemStatus Status { get; set; } = WorkItemStatus.New;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; set; }

    public static WorkItem Create(string title, string? description)
    {
        return new WorkItem
        {
            Title = title,
            Description = description,
            Status = WorkItemStatus.New,
            CreatedAt = DateTime.UtcNow
        };
    }
}