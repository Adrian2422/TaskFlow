namespace TaskFlow.Domain.Entities;

public class BoardColumn : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public int Order { get; set; }

    public Guid BoardId { get; set; }
    public Board Board { get; set; } = null!;

    public ICollection<WorkItem> WorkItems { get; set; } = new List<WorkItem>();

    public static BoardColumn Create(string name, int order, Guid boardId)
    {
        return new BoardColumn
        {
            Name = name,
            Order = order,
            BoardId = boardId
        };
    }
}