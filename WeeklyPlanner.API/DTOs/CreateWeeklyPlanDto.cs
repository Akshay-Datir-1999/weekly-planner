namespace WeeklyPlanner.API.DTOs;

public class CreateWeeklyPlanDto
{
    public int MemberId { get; set; }
    public int WeekCycleId { get; set; }

    public List<PlanEntryDto> PlanEntries { get; set; } = new();
}

public class PlanEntryDto
{
    public int BacklogItemId { get; set; }
    public decimal PlannedHours { get; set; }
}