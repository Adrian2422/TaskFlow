using MediatR;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Commands.Columns.CreateColumn;

public class CreateColumnCommandHandler : IRequestHandler<CreateColumnCommand, ColumnDto>
{
    private readonly IBoardColumnRepository _columnRepository;

    public CreateColumnCommandHandler(IBoardColumnRepository columnRepository)
    {
        _columnRepository = columnRepository;
    }

    public async Task<ColumnDto> Handle(CreateColumnCommand request, CancellationToken cancellationToken)
    {
        var maxOrder = await _columnRepository.GetMaxOrderInBoardAsync(request.BoardId);
        var column = BoardColumn.Create(request.Name, maxOrder + 1000, request.BoardId);
        
        await _columnRepository.CreateAsync(column);
        await _columnRepository.SaveChangesAsync();
        
        return new ColumnDto
        {
            Id = column.Id,
            Name = column.Name,
            Order = column.Order,
            IsProtected = column.IsProtected
        };
    }
}
