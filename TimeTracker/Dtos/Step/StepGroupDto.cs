namespace TimeTracker.Dtos.Step;

public class StepGroupDto {
    public DateOnly CompletedOn { get; set; }
    public List<StepDto> Steps { get; set; } = [];
}