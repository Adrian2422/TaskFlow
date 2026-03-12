using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.DTOs;

public class CreateWorkItemDto
{
    public required string Title { get; init; }
    public string? Description { get; init; }
}