namespace TaskFlow.Domain.Entities;

public class Board : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public ICollection<BoardColumn> Columns { get; set; } = new List<BoardColumn>();

    public ICollection<WorkItem> BacklogItems { get; set; } = new List<WorkItem>();

    public static Board Create(string name, string? description)
    {
        return new Board
        {
            Name = name,
            Description = description
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