using TaskFlow.Application.Common;

namespace TaskFlow.Domain.Errors;

public static class WorkItemErrors
{
    public static Error NotFound(Guid id) => new("WorkItem.NotFound", $"WorkItem with id {id} not found.");
    public static Error AlreadyArchived(Guid id) => new("WorkItem.AlreadyArchived", $"WorkItem with id {id} is already archived.");
    public static readonly Error DeleteNotArchived = new ("WorkItem.Delete.NotArchived", "Only archived work items can be deleted.");
    public static readonly Error RestoreNotArchived = new ("WorkItem.Restore.NotArchived", "Only archived work items can be restored.");
}