using Microsoft.EntityFrameworkCore;
using WeeklyPlanner.API.Middleware;
using WeeklyPlanner.Core.Interfaces;
using WeeklyPlanner.Infrastructure.Data;
using WeeklyPlanner.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// ── Database ──────────────────────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Register Services (Dependency Injection) ──────────────────────────────
builder.Services.AddScoped<IBacklogService, BacklogService>();
builder.Services.AddScoped<IWeekCycleService, WeekCycleService>();
builder.Services.AddScoped<IPlanService, PlanService>();

// ── Controllers + Swagger ─────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    c.SwaggerDoc("v1", new() { Title = "Weekly Planner API", Version = "v1" }));

// ── CORS ──────────────────────────────────────────────────────────────────
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod()));

var app = builder.Build();

// ── Auto-apply migrations on startup ──────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// ── Middleware pipeline — ORDER MATTERS ───────────────────────────────────
app.UseMiddleware<ExceptionMiddleware>();  // must be first
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();
app.MapControllers();

app.Run();