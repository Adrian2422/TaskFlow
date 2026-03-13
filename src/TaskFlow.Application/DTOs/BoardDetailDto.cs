namespace TaskFlow.Application.DTOs;

public class BoardDetailDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public List<ColumnDetailDto> Columns { get; set; } = [];
    public List<WorkItemDto> Backlog { get; set; } = [];
}
