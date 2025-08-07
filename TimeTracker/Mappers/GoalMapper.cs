using TimeTracker.Dtos.Goal;
using TimeTracker.Models;

namespace TimeTracker.Mappers;

public static class GoalMapper {
    public static GoalGroupDto ToGroupDto(this GoalGroup goalGroup) {
        return new GoalGroupDto() {
            Project = goalGroup.Project?.ToFlatDto(),
            Goals = goalGroup.Goals.Select(g => g.ToFlatDto()).ToList(),
        };
    }

    public static GoalFlatDto ToFlatDto(this Goal goalModel) {
        return new GoalFlatDto() {
            Id = goalModel.Id,
            Name = goalModel.Name,
            Description = goalModel.Description,
            StartDate = goalModel.StartDate,
            CloseDate = goalModel.StartDate,
            ProjectId = goalModel.ProjectId,
        };
    }

    public static GoalDto ToDto(this Goal goalModel) {
        return new GoalDto() {
            Id = goalModel.Id,
            Name = goalModel.Name,
            Description = goalModel.Description,
            StartDate = goalModel.StartDate,
            CloseDate = goalModel.StartDate,
            ProjectId = goalModel.ProjectId,
            Project = goalModel.Project?.ToFlatDto(),
            StepCount = goalModel.StepCount,
        };
    }

    public static Goal ToModel(this CreateGoalDto goalDto) {
        return new Goal() {
            Name = goalDto.Name,
            Description = goalDto.Description,
            ProjectId = goalDto.ProjectId,
        };
    }

    public static void UpdateModelFromDto(this Goal goalModel, UpdateGoalDto goalDto) {
        goalModel.Name = goalDto.Name;
        goalModel.Description = goalDto.Description;
        goalModel.ProjectId = goalDto.ProjectId;
    }
}