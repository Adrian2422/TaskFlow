namespace TaskFlow.Domain.Entities;

public class WorkItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public Guid? ColumnId { get; set; }
    public BoardColumn? Column { get; set; }

    public Guid BoardId { get; set; }
    public Board Board { get; set; } = null!;

    public double Order { get; set; }

    public static WorkItem Create(string title, string? description, Guid boardId, Guid? columnId, double order)
    {
        return new WorkItem
        {
            Title = title,
            Description = description,
            BoardId = boardId,
            ColumnId = columnId,
            Order = order
        };
    }

    public void Archive()
    {
        IsArchived = true;
    }

    public void Restore()
    {
        IsArchived = false;
    }
}