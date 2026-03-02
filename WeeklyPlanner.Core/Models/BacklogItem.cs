namespace WeeklyPlanner.Core.Models;

public class BacklogItem
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    // 1 = Client Focused
    // 2 = Tech Debt
    // 3 = R&D
    public int Category { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<PlanEntry> PlanEntries { get; set; } = new List<PlanEntry>();
}