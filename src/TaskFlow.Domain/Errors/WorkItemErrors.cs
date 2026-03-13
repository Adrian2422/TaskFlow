using TaskFlow.Application.Common;

namespace TaskFlow.Domain.Errors;

public static class WorkItemErrors
{
    public static Error NotFound(Guid id) => new("WorkItem.NotFound", $"WorkItem with id {id} not found.");
    public static readonly Error NotArchived = new Error("WorkItem.NotArchived", "Only archived work items can be deleted.");
}