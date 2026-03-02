namespace WeeklyPlanner.Core.Models;

public class WeekCycle
{
    public int Id { get; set; }

    public DateTime PlanningDate { get; set; }   // Tuesday

    public DateTime WeekStartDate { get; set; }  // Wednesday

    public DateTime WeekEndDate { get; set; }    // Monday

    public decimal Category1Percent { get; set; }

    public decimal Category2Percent { get; set; }

    public decimal Category3Percent { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<WeeklyPlan> WeeklyPlans { get; set; } = new List<WeeklyPlan>();
}