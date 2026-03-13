namespace TaskFlow.Domain.Entities;

public class Board : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public ICollection<BoardColumn> Columns { get; set; } = new List<BoardColumn>();

    public static Board Create(string name, string? description)
    {
        return new Board
        {
            Name = name,
            Description = description
        };
    }
}