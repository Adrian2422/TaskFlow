namespace TaskFlow.Application.DTOs;

public class BoardDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}
