using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.Errors;

public static class BoardErrors
{
    public static Error NotFound(Guid id) => new("Board.NotFound", $"Board with id {id} not found.");
    public static Error AlreadyArchived(Guid id) => new ("Board.AlreadyArchived", $"Board with id {id} is already archived.");
    public static readonly Error DeleteNotArchived = new ("Board.Delete.NotArchived", "Only archived boards can be deleted.");
    public static readonly Error RestoreNotArchived = new ("Board.Restore.NotArchived", "Only archived boards can be restored.");
}