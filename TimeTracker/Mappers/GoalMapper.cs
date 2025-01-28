using TimeTracker.Dtos.Goal;
using TimeTracker.Models;

namespace TimeTracker.Mappers;

public static class GoalMapper {
    public static GoalDto ToDto(this Goal goalModel) {
        return new GoalDto() {
            Id = goalModel.Id,
            Name = goalModel.Name,
            Color = goalModel.Color,
            Description = goalModel.Description,
            StartDate = goalModel.StartDate,
            CloseDate = goalModel.StartDate,
            ProjectId = goalModel.ProjectId,
        };
    }
}