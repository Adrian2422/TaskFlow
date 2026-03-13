using TaskFlow.Application.Common;

namespace TaskFlow.Domain.Errors;

public static class BoardErrors
{
    public static Error NotFound(Guid id) => new("Board.NotFound", $"Board with id {id} not found.");
    public static readonly Error NotArchived = new Error("Board.NotArchived", "Only archived boards can be deleted.");
}