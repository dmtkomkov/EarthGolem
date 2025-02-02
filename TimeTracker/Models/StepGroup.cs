namespace TimeTracker.Models;

public class StepGroup
{
    public DateOnly CompletedOn { get; set; }
    public List<Step> Steps { get; set; } = [];
}