using TimeTracker.Dtos.Project;

namespace TimeTracker.Dtos.Goal;

public class GoalDto : GoalFlatDto {
    public ProjectFlatDto? Project { get; set; }
    public int StepCount { get; set; }
}