using Microsoft.EntityFrameworkCore;
using WeeklyPlanner.API.Middleware;
using WeeklyPlanner.Core.Interfaces;
using WeeklyPlanner.Infrastructure.Data;
using WeeklyPlanner.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// ── Database Configuration (SQLite) ───────────────────────────────────────
// Get database path from environment variable or use default
var dbPath = Environment.GetEnvironmentVariable("DB_PATH") ?? 
             Path.Combine(Directory.GetCurrentDirectory(), "app_data");

// Create app_data directory if it doesn't exist
Directory.CreateDirectory(dbPath);

// Build SQLite connection string
var dbFilePath = Path.Combine(dbPath, "weekly-planner.db");
var connectionString = $"Data Source={dbFilePath};";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// ── Register Services (Dependency Injection) ──────────────────────────────
builder.Services.AddScoped<IBacklogService, BacklogService>();
builder.Services.AddScoped<IWeekCycleService, WeekCycleService>();
builder.Services.AddScoped<IPlanService, PlanService>();

// ── Controllers + JSON Settings ───────────────────────────────────────────
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// ── Swagger Configuration ─────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Weekly Planner API", Version = "v1" });
});

// ── CORS Configuration ────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "https://localhost:4200",
                "https://weekly-planner-ui-akshay.azurewebsites.net",
                "https://ambitious-flower-08c08c400.4.azurestaticapps.net",
                "https://mango-river-0a9a03300.6.azurestaticapps.net"  // ← NEW
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// ── Apply Migrations Automatically on Startup ─────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// ── Middleware Pipeline ───────────────────────────────────────────────────
app.UseMiddleware<ExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.MapControllers();

app.Run();
```

---

**Only 1 line was added** — the new frontend URL in CORS:
```
"https://mango-river-0a9a03300.6.azurestaticapps.net"
