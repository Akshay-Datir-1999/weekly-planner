namespace WeeklyPlanner.Core.Models;

public class WeeklyPlan
{
    public int Id { get; set; }

    public int MemberId { get; set; }

    public Member Member { get; set; } = null!;

    public int WeekCycleId { get; set; }

    public WeekCycle WeekCycle { get; set; } = null!;

    public bool IsFrozen { get; set; } = false;

    public DateTime? FrozenAt { get; set; }

    public ICollection<PlanEntry> PlanEntries { get; set; } = new List<PlanEntry>();
}