using MediatR;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Queries.Columns.GetByBoardId;

public class GetColumnsByBoardIdQueryHandler : IRequestHandler<GetColumnsByBoardIdQuery, List<ColumnDto>>
{
    private readonly IBoardColumnRepository _columnRepository;

    public GetColumnsByBoardIdQueryHandler(IBoardColumnRepository columnRepository)
    {
        _columnRepository = columnRepository;
    }

    public async Task<List<ColumnDto>> Handle(GetColumnsByBoardIdQuery request, CancellationToken cancellationToken)
    {
        var columns = await _columnRepository.GetByBoardIdAsync(request.BoardId);
        return columns.OrderBy(c => c.Order).Select(c => new ColumnDto
        {
            Id = c.Id,
            Name = c.Name,
            Order = c.Order,
            IsProtected = c.IsProtected
        }).ToList();
    }
}
