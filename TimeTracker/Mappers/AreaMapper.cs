using TimeTracker.Dtos.Area;
using TimeTracker.Models;

namespace TimeTracker.Mappers;

public static class AreaMapper {
    public static AreaFlatDto ToFlatDto(this Area areaModel) {
        return new AreaFlatDto() {
            Id = areaModel.Id,
            Name = areaModel.Name,
            Color = areaModel.Color,
            Description = areaModel.Description,
        };
    }

    public static AreaDto ToDto(this Area areaModel) {
        return new AreaDto() {
            Id = areaModel.Id,
            Name = areaModel.Name,
            Color = areaModel.Color,
            Description = areaModel.Description,
            Categories = areaModel.Categories.Select(c => c.ToFlatDto()).ToList(),
        };
    }

    public static Area ToModel(this CreateAreaDto areaDto) {
        return new Area() {
            Name = areaDto.Name,
            Color = areaDto.Color,
            Description = areaDto.Description,
        };
    }

    public static void UpdateModelFromDto(this Area areaModel, UpdateAreaDto areaDto) {
        areaModel.Name = areaDto.Name;
        areaModel.Color = areaDto.Color;
        areaModel.Description = areaDto.Description;
    }
}