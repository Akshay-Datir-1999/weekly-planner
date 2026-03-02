using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeeklyPlanner.API.DTOs;
using WeeklyPlanner.Core.Models;
using WeeklyPlanner.Infrastructure.Data;

namespace WeeklyPlanner.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeeklyPlanController : ControllerBase
{
    private readonly AppDbContext _context;

    public WeeklyPlanController(AppDbContext context)
    {
        _context = context;
    }

    // ============================================
    // CREATE WEEKLY PLAN
    // ============================================
    [HttpPost]
    public async Task<IActionResult> CreatePlan([FromBody] CreateWeeklyPlanDto request)
    {
        var weekCycle = await _context.WeekCycles
            .FirstOrDefaultAsync(w => w.Id == request.WeekCycleId);

        if (weekCycle == null)
            return BadRequest("Invalid WeekCycleId.");

        var existingPlan = await _context.WeeklyPlans
            .FirstOrDefaultAsync(p =>
                p.MemberId == request.MemberId &&
                p.WeekCycleId == request.WeekCycleId);

        if (existingPlan != null)
            return BadRequest("Plan already exists for this member and week.");

        var totalHours = request.PlanEntries.Sum(p => p.PlannedHours);

        if (totalHours > 30)
            return BadRequest("Total planned hours cannot exceed 30.");

        if (totalHours <= 0)
            return BadRequest("Total planned hours must be greater than 0.");

        // CATEGORY LIMIT CALCULATION
        var categoryLimits = new Dictionary<int, double>
        {
            { 1, (double)(weekCycle.Category1Percent / 100m * 30m) },
            { 2, (double)(weekCycle.Category2Percent / 100m * 30m) },
            { 3, (double)(weekCycle.Category3Percent / 100m * 30m) }
        };

        var categoryTotals = request.PlanEntries
            .Join(_context.BacklogItems,
                entry => entry.BacklogItemId,
                backlog => backlog.Id,
                (entry, backlog) => new
                {
                    backlog.Category,
                    entry.PlannedHours
                })
            .GroupBy(x => x.Category)
            .ToDictionary(
                g => g.Key,
                g => g.Sum(x => (double)x.PlannedHours)
            );

        foreach (var category in categoryTotals)
        {
            if (category.Value > categoryLimits[category.Key])
            {
                return BadRequest(
                    $"Category {category.Key} exceeds allowed hours. " +
                    $"Allowed: {categoryLimits[category.Key]}, " +
                    $"Planned: {category.Value}"
                );
            }
        }

        var weeklyPlan = new WeeklyPlan
        {
            MemberId = request.MemberId,
            WeekCycleId = request.WeekCycleId,
            IsFrozen = true,
            FrozenAt = DateTime.UtcNow
        };

        foreach (var entry in request.PlanEntries)
        {
            weeklyPlan.PlanEntries.Add(new PlanEntry
            {
                BacklogItemId = entry.BacklogItemId,
                PlannedHours = entry.PlannedHours,
                ProgressPercent = 0
            });
        }

        _context.WeeklyPlans.Add(weeklyPlan);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            weeklyPlan.Id,
            weeklyPlan.MemberId,
            weeklyPlan.WeekCycleId,
            TotalPlannedHours = totalHours,
            weeklyPlan.IsFrozen,
            weeklyPlan.FrozenAt
        });
    }

    // ============================================
    // GET ALL WEEKLY PLANS
    // ============================================
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var plans = await _context.WeeklyPlans
            .Include(p => p.PlanEntries)
            .AsNoTracking()
            .ToListAsync();

        var result = plans.Select(p => new
        {
            p.Id,
            p.MemberId,
            p.WeekCycleId,
            p.IsFrozen,
            TotalPlannedHours = p.PlanEntries.Sum(e => (double)e.PlannedHours),
            Entries = p.PlanEntries.Select(e => new
            {
                e.Id,
                e.BacklogItemId,
                e.PlannedHours,
                e.ProgressPercent
            })
        });

        return Ok(result);
    }

    // ============================================
    // UPDATE PROGRESS
    // ============================================
    [HttpPut("entry/{planEntryId}/progress")]
    public async Task<IActionResult> UpdateProgress(
        int planEntryId,
        [FromBody] UpdateProgressDto request)
    {
        if (request.ProgressPercent < 0 || request.ProgressPercent > 100)
            return BadRequest("Progress must be between 0 and 100.");

        var planEntry = await _context.PlanEntries
            .Include(p => p.WeeklyPlan)
            .FirstOrDefaultAsync(p => p.Id == planEntryId);

        if (planEntry == null)
            return NotFound("Plan entry not found.");

        if (!planEntry.WeeklyPlan.IsFrozen)
            return BadRequest("Cannot update progress before plan is frozen.");

        planEntry.ProgressPercent = request.ProgressPercent;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            planEntry.Id,
            planEntry.ProgressPercent
        });
    }

    // ============================================
    // DASHBOARD ANALYTICS
    // ============================================
    [HttpGet("{planId}/dashboard")]
    public async Task<IActionResult> GetDashboard(int planId)
    {
        var plan = await _context.WeeklyPlans
            .Include(p => p.PlanEntries)
            .ThenInclude(e => e.BacklogItem)
            .FirstOrDefaultAsync(p => p.Id == planId);

        if (plan == null)
            return NotFound("Plan not found.");

        var totalPlanned = plan.PlanEntries.Sum(e => (double)e.PlannedHours);

        var totalCompleted = plan.PlanEntries
            .Sum(e => (double)(e.PlannedHours * e.ProgressPercent / 100));

        var overallProgress = totalPlanned == 0
            ? 0
            : Math.Round((totalCompleted / totalPlanned) * 100, 2);

        var categoryBreakdown = plan.PlanEntries
            .GroupBy(e => e.BacklogItem.Category)
            .Select(g => new
            {
                Category = g.Key,
                Planned = g.Sum(x => (double)x.PlannedHours),
                Completed = g.Sum(x =>
                    (double)(x.PlannedHours * x.ProgressPercent / 100))
            });

        return Ok(new
        {
            PlanId = plan.Id,
            plan.MemberId,
            TotalPlannedHours = totalPlanned,
            TotalCompletedHours = totalCompleted,
            OverallProgressPercent = overallProgress,
            CategoryBreakdown = categoryBreakdown
        });
    }
}