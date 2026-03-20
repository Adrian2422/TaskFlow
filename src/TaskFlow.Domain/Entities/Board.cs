namespace TaskFlow.Domain.Entities;

public class Board : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public ICollection<BoardColumn> Columns { get; set; } = new List<BoardColumn>();

    public ICollection<WorkItem> BacklogItems { get; set; } = new List<WorkItem>();

    public Guid CreatedById { get; set; }
    public User CreatedBy { get; set; } = null!;

    public ICollection<BoardMember> Members { get; set; } = new List<BoardMember>();

    public static Board Create(string name, string? description, Guid createdById)
    {
        return new Board
        {
            Name = name,
            Description = description,
            CreatedById = createdById
        };
    }

    public void Archive()
    {
        IsArchived = true;
        foreach (var item in BacklogItems)
        {
            item.Archive();
        }

        foreach (var column in Columns)
        {
            foreach (var item in column.WorkItems)
            {
                item.Archive();
            }
        }
    }

    public void Restore()
    {
        IsArchived = false;
    }
}