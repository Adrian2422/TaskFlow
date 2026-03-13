namespace TaskFlow.Application.Common;

public class PaginationQuery
{
    private const int MaxPageSize = 100;
    private readonly int _pageSize = 20;

    public int PageNumber { get; init; } = 1;

    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
}