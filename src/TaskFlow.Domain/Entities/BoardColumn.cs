namespace TaskFlow.Domain.Entities;

public class BoardColumn : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public double Order { get; set; }

    public bool IsProtected { get; set; }

    public Guid BoardId { get; set; }
    public Board Board { get; set; } = null!;

    public ICollection<WorkItem> WorkItems { get; set; } = new List<WorkItem>();

    public static BoardColumn Create(string name, double order, Guid boardId, bool isProtected = false)
    {
        return new BoardColumn
        {
            Name = name,
            Order = order,
            BoardId = boardId,
            IsProtected = isProtected
        };
    }
}
