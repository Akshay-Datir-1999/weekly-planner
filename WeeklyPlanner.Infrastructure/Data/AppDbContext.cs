using Microsoft.EntityFrameworkCore;
using WeeklyPlanner.Core.Models;

namespace WeeklyPlanner.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Member> Members => Set<Member>();
    public DbSet<BacklogItem> BacklogItems => Set<BacklogItem>();
    public DbSet<WeekCycle> WeekCycles => Set<WeekCycle>();
    public DbSet<WeeklyPlan> WeeklyPlans => Set<WeeklyPlan>();
    public DbSet<PlanEntry> PlanEntries => Set<PlanEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed Members
        modelBuilder.Entity<Member>().HasData(
            new Member { Id = 1, Name = "Team Lead", IsLead = true },
            new Member { Id = 2, Name = "Alice", IsLead = false },
            new Member { Id = 3, Name = "Bob", IsLead = false }
        );

        // Seed Backlog Items (All 3 Categories)
        modelBuilder.Entity<BacklogItem>().HasData(
            new BacklogItem { Id = 1, Title = "Fix login bug", Description = "Client issue", Category = 1, IsActive = true },
            new BacklogItem { Id = 2, Title = "New customer dashboard", Description = "Client feature", Category = 1, IsActive = true },
            new BacklogItem { Id = 3, Title = "Refactor auth module", Description = "Tech debt cleanup", Category = 2, IsActive = true },
            new BacklogItem { Id = 4, Title = "Database indexing", Description = "Performance improvement", Category = 2, IsActive = true },
            new BacklogItem { Id = 5, Title = "AI research spike", Description = "R&D exploration", Category = 3, IsActive = true },
            new BacklogItem { Id = 6, Title = "Cloud cost analysis", Description = "R&D optimization", Category = 3, IsActive = true }
        );
    }
}