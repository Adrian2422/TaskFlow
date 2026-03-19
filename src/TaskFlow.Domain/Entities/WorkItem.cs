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

    public Guid CreatedById { get; set; }
    public User CreatedBy { get; set; } = null!;

    public Guid? AssignedToId { get; set; }
    public User? AssignedTo { get; set; }

    public static WorkItem Create(string title, string? description, Guid boardId, Guid? columnId, double order, Guid createdById, Guid? assignedToId = null)
    {
        return new WorkItem
        {
            Title = title,
            Description = description,
            BoardId = boardId,
            ColumnId = columnId,
            Order = order,
            CreatedById = createdById,
            AssignedToId = assignedToId
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