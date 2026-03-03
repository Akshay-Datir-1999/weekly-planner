using Microsoft.EntityFrameworkCore;
using WeeklyPlanner.Core.Models;
namespace WeeklyPlanner.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Member> Members => Set<Member>();
    public DbSet<BacklogItem> BacklogItems => Set<BacklogItem>();
    public DbSet<WeekCycle> WeekCycles => Set<WeekCycle>();
    public DbSet<WeeklyPlan> WeeklyPlans => Set<WeeklyPlan>();
    public DbSet<PlanEntry> PlanEntries => Set<PlanEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed default members
        modelBuilder.Entity<Member>().HasData(
            new Member { Id = 1, Name = "Team Lead", IsLead = true },
            new Member { Id = 2, Name = "Alice", IsLead = false },
            new Member { Id = 3, Name = "Bob", IsLead = false }
        );
        // Seed sample backlog items for all 3 categories
        modelBuilder.Entity<BacklogItem>().HasData(
            new BacklogItem { Id=1, Title="Fix login bug", Category=1, Description="Client reported login issue", IsActive=true },
            new BacklogItem { Id=2, Title="Customer dashboard", Category=1, Description="New client feature", IsActive=true },
            new BacklogItem { Id=3, Title="Refactor auth module", Category=2, Description="Tech debt cleanup", IsActive=true },
            new BacklogItem { Id=4, Title="Database indexing", Category=2, Description="Performance improvement", IsActive=true },
            new BacklogItem { Id=5, Title="AI recommendation spike", Category=3, Description="Research ML options", IsActive=true },
            new BacklogItem { Id=6, Title="Cloud cost analysis", Category=3, Description="R&D for cost reduction", IsActive=true }
        );
    }
}
