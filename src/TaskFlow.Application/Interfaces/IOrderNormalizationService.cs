namespace TaskFlow.Application.Interfaces;

public interface IOrderNormalizationService
{
    Task NormalizeColumnsIfNeeded(double minGap);
}