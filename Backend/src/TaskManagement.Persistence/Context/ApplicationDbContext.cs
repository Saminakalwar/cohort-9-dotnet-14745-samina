using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Persistence.Context;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    public DbSet<Category> Categories => Set<Category>();

   protected override void OnModelCreating(ModelBuilder builder)
    {
        // Configure Identity tables first
        base.OnModelCreating(builder);

        builder.Entity<TaskItem>(entity =>
        {
            entity.Property(task => task.AssignedUserId)
                .HasMaxLength(450);

            entity.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(task => task.AssignedUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}