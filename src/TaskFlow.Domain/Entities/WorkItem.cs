namespace TaskFlow.Domain.Entities;

public class WorkItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public Guid ColumnId { get; set; }
    public BoardColumn Column { get; set; } = null!;

    public double Order { get; set; }

    public static WorkItem Create(string title, string? description, Guid columnId)
    {
        return new WorkItem
        {
            Title = title,
            Description = description,
            ColumnId = columnId
        };
    }
}