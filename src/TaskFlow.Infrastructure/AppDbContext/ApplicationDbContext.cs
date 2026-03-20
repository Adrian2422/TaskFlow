using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.AppDbContext;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DbSet<Board> Boards => Set<Board>();
    public DbSet<BoardColumn> Columns => Set<BoardColumn>();
    public DbSet<WorkItem> WorkItems => Set<WorkItem>();
    public DbSet<BoardMember> BoardMembers => Set<BoardMember>();


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Board>()
            .HasMany(b => b.BacklogItems)
            .WithOne(wi => wi.Board)
            .HasForeignKey(wi => wi.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Board>()
            .HasOne(b => b.CreatedBy)
            .WithMany()
            .HasForeignKey(b => b.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BoardColumn>()
            .HasMany(c => c.WorkItems)
            .WithOne(wi => wi.Column!)
            .HasForeignKey(wi => wi.ColumnId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<WorkItem>()
            .HasOne(wi => wi.CreatedBy)
            .WithMany()
            .HasForeignKey(wi => wi.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<WorkItem>()
            .HasOne(wi => wi.AssignedTo)
            .WithMany()
            .HasForeignKey(wi => wi.AssignedToId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<BoardMember>()
            .HasOne(bm => bm.Board)
            .WithMany(b => b.Members)
            .HasForeignKey(bm => bm.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BoardMember>()
            .HasOne(bm => bm.User)
            .WithMany(u => u.BoardMemberships)
            .HasForeignKey(bm => bm.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}