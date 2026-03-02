using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeeklyPlanner.Infrastructure.Data;

namespace WeeklyPlanner.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("week/{weekCycleId}")]
    public async Task<IActionResult> GetWeekDashboard(int weekCycleId)
    {
        var plans = await _context.WeeklyPlans
            .Include(p => p.PlanEntries)
                .ThenInclude(e => e.BacklogItem)
            .Where(p => p.WeekCycleId == weekCycleId)
            .ToListAsync();

        if (!plans.Any())
            return NotFound("No plans found for this week.");

        var totalPlanned = plans
            .SelectMany(p => p.PlanEntries)
            .Sum(e => (double)e.PlannedHours);

        var totalProgress = plans
            .SelectMany(p => p.PlanEntries)
            .Average(e => (double)e.ProgressPercent);

        var memberSummary = plans.Select(p => new
        {
            p.MemberId,
            TotalHours = p.PlanEntries.Sum(e => (double)e.PlannedHours),
            AverageProgress = p.PlanEntries.Average(e => (double)e.ProgressPercent)
        });

        var categorySummary = plans
            .SelectMany(p => p.PlanEntries)
            .GroupBy(e => e.BacklogItem.Category)
            .Select(g => new
            {
                Category = g.Key,
                TotalHours = g.Sum(e => (double)e.PlannedHours)
            });

        return Ok(new
        {
            WeekCycleId = weekCycleId,
            TotalTeamHours = totalPlanned,
            AverageTeamProgress = totalProgress,
            Members = memberSummary,
            Categories = categorySummary
        });
    }
}