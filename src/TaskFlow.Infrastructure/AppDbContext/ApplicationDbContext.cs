using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.AppDbContext;

public class ApplicationDbContext : DbContext
{
    public DbSet<Board> Boards => Set<Board>();
    public DbSet<BoardColumn> Columns => Set<BoardColumn>();
    public DbSet<WorkItem> WorkItems => Set<WorkItem>();


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Board>()
            .HasMany(b => b.BacklogItems)
            .WithOne(wi => wi.Board)
            .HasForeignKey(wi => wi.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BoardColumn>()
            .HasMany(c => c.WorkItems)
            .WithOne(wi => wi.Column!)
            .HasForeignKey(wi => wi.ColumnId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }
}