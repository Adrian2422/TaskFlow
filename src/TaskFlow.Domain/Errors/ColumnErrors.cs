using TaskFlow.Application.Common;

namespace TaskFlow.Domain.Errors;

public static class ColumnErrors
{
    public static Error NotFound(Guid columnId, Guid boardId) => new("Column.NotFound", $"Column with id {columnId} not found on board {boardId}.");
    public static readonly Error Protected = new("Column.Protected", "Cannot delete a protected column.");
}