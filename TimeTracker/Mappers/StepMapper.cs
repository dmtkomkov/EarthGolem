using TimeTracker.Dtos.Step;
using TimeTracker.Models;

namespace TimeTracker.Mappers;

public static class StepMapper {
    public static StepGroupDto ToGroupDto(this StepGroup stepGroup) {
        return new StepGroupDto() {
            CompletedOn = stepGroup.CompletedOn,
            Steps = stepGroup.Steps.Select(s => s.ToDto()).ToList(),
        };
    }
    
    public static StepDto ToDto(this Step stepModel) {
        return new StepDto() {
            Id = stepModel.Id,
            Duration = stepModel.Duration,
            CompletedOn = stepModel.CompletedOn,
            Description = stepModel.Description,
            User = stepModel.User!.ToDto(),
            Category = stepModel.Category!.ToDto(),
            Goal = stepModel.Goal?.ToDto()
        };
    }

    public static Step ToModel(this CreateStepDto stepDto) {
        return new Step() {
            Duration = stepDto.Duration,
            CompletedOn = stepDto.CompletedOn,
            Description = stepDto.Description,
            UserId = stepDto.UserId!,
            CategoryId = stepDto.CategoryId,
            GoalId = stepDto.GoalId,
        };
    }
    
    public static void UpdateModelFromDto(this Step stepModel, UpdateStepDto stepDto) {
        stepModel.Duration = stepDto.Duration;
        stepModel.CompletedOn = stepDto.CompletedOn;
        stepModel.Description = stepDto.Description;
        stepModel.UserId = stepDto.UserId;
        stepModel.CategoryId = stepDto.CategoryId;
        stepModel.GoalId = stepModel.GoalId;
    }
}