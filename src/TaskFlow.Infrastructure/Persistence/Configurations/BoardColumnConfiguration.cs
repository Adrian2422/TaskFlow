using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Persistence.Configurations;

public class BoardColumnConfiguration
{
    public void Configure(EntityTypeBuilder<BoardColumn> builder)
    {
        builder.HasMany(x => x.WorkItems)
            .WithOne(x => x.Column)
            .HasForeignKey(x => x.ColumnId);
    }
}