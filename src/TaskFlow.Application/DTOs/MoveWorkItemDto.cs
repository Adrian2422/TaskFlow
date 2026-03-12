namespace TaskFlow.Application.DTOs;

public class MoveWorkItemDto
{
    public Guid ColumnId { get; set; }

    public double? PrevPosition { get; set; }

    public double? NextPosition { get; set; }
}