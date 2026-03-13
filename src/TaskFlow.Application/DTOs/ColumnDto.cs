namespace TaskFlow.Application.DTOs;

public class ColumnDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public double Order { get; set; }
    public bool IsProtected { get; set; }
}

public class ColumnDetailDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public double Order { get; set; }
    public bool IsProtected { get; set; }
    public List<WorkItemDto> WorkItems { get; set; } = [];
}
