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
    
    public static Goal ToModel(this CreateGoalDto goalDto) {
        return new Goal() {
            Name = goalDto.Name,
            Color = goalDto.Color,
            Description = goalDto.Description,
            ProjectId = goalDto.ProjectId,
        };
    }

    public static void UpdateModelFromDto(this Goal goalModel, UpdateGoalDto goalDto) {
        goalModel.Name = goalDto.Name;
        goalModel.Color = goalDto.Color;
        goalModel.Description = goalDto.Description;
        goalModel.ProjectId = goalDto.ProjectId;
    }
}