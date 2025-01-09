using TimeTracker.Dtos;
using TimeTracker.Models;

namespace TimeTracker.Mappers;

public static class StepMapper
{
    public static StepDto ToDto(this Step stepModel)
    {
        return new StepDto()
        {
            Id = stepModel.Id,
            Duration = stepModel.Duration,
            CompletedOn = stepModel.CompletedOn,
            Description = stepModel.Description,
        };
    }

    public static void UpdateModelFromDto(this Step stepModel, UpdateStepDto stepDto)
    {
        stepModel.Duration = stepDto.Duration;
        stepModel.CompletedOn = stepDto.CompletedOn;
        stepModel.Description = stepDto.Description;
    }

    public static Step ToModel(this CreateStepDto stepDto)
    {
        return new Step()
        {
            Duration = stepDto.Duration,
            CompletedOn = stepDto.CompletedOn,
            Description = stepDto.Description,
        };
    }
}