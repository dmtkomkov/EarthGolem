namespace TimeTracker.Dtos;

public class StepDto
{
    public int Id { get; set; }
    public int Duration { get; set; }
    public DateOnly CompletedOn { get; set; }
    public string? Description { get; set; }
}