using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WeeklyPlanner.Infrastructure.Data;

/// <summary>
/// Design-time factory used by EF Core tools to create AppDbContext
/// when running migrations.
/// This avoids dependency injection issues at design time.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // SQLite database file will be created in the root folder
        optionsBuilder.UseSqlite("Data Source=WeeklyPlanner.db");

        return new AppDbContext(optionsBuilder.Options);
    }
}