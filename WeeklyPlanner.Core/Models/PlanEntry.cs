namespace WeeklyPlanner.Core.Models;

public class PlanEntry
{
    public int Id { get; set; }

    public int WeeklyPlanId { get; set; }

    public WeeklyPlan WeeklyPlan { get; set; } = null!;

    public int BacklogItemId { get; set; }

    public BacklogItem BacklogItem { get; set; } = null!;

    public decimal PlannedHours { get; set; }

    public decimal ProgressPercent { get; set; } = 0;

    public decimal? ActualHours { get; set; }

    public DateTime? LastUpdated { get; set; }
}