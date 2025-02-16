using TimeTracker.Dtos.Project;

namespace TimeTracker.Dtos.Goal;

public class GoalGroupDto {
    public ProjectFlatDto? Project { get; set; }
    public List<GoalFlatDto> Goals { get; set; } = [];
}