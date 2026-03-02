namespace WeeklyPlanner.Core.Models;

public class Member
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsLead { get; set; } = false;

    // Navigation property
    public ICollection<WeeklyPlan> WeeklyPlans { get; set; } = new List<WeeklyPlan>();
}