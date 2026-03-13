namespace TaskFlow.Application.Interfaces;

public interface IOrderNormalizationService
{
    Task NormalizeWorkItemsIfNeeded(double minGap);
    Task NormalizeColumnsIfNeeded(double minGap);
}