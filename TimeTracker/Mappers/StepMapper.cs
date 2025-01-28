﻿using TimeTracker.Dtos;
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
            User = stepModel.User?.ToDto(),
        };
    }

    public static Step ToModel(this CreateStepDto stepDto, string userId)
    {
        return new Step()
        {
            Duration = stepDto.Duration,
            CompletedOn = stepDto.CompletedOn,
            Description = stepDto.Description,
            UserId = userId,
        };
    }
    
    public static void UpdateModelFromDto(this Step stepModel, UpdateStepDto stepDto)
    {
        stepModel.Duration = stepDto.Duration;
        stepModel.CompletedOn = stepDto.CompletedOn;
        stepModel.Description = stepDto.Description;
        stepModel.UserId = stepDto.UserId;
    }
}