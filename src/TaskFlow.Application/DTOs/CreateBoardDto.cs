namespace TaskFlow.Application.DTOs;

public class CreateBoardDto
{
    public required string Name { get; init; }
    public string? Description { get; init; }
}
