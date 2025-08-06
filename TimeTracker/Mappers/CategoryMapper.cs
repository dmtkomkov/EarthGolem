using TimeTracker.Dtos.Category;
using TimeTracker.Models;

namespace TimeTracker.Mappers;

public static class CategoryMapper {
    public static CategoryGroupDto ToGroupDto(this CategoryGroup categoryGroup) {
        return new CategoryGroupDto() {
            Area = categoryGroup.Area!.ToFlatDto(),
            Categories = categoryGroup.Categories.Select(s => s.ToFlatDto()).ToList(),
        };
    }

    public static CategoryFlatDto ToFlatDto(this Category categoryModel) {
        return new CategoryFlatDto() {
            Id = categoryModel.Id,
            Name = categoryModel.Name,
            Color = categoryModel.Color,
            Description = categoryModel.Description,
            AreaId = categoryModel.AreaId,
        };
    }

    public static CategoryDto ToDto(this Category categoryModel) {
        return new CategoryDto() {
            Id = categoryModel.Id,
            Name = categoryModel.Name,
            Color = categoryModel.Color,
            Description = categoryModel.Description,
            AreaId = categoryModel.AreaId,
            Area = categoryModel.Area!.ToFlatDto(),
            StepCount = categoryModel.StepCount,
        };
    }

    public static Category ToModel(this CreateCategoryDto categoryDto) {
        return new Category() {
            Name = categoryDto.Name,
            Color = categoryDto.Color,
            Description = categoryDto.Description,
            AreaId = categoryDto.AreaId,
        };
    }

    public static void UpdateModelFromDto(this Category categoryModel, UpdateCategoryDto categoryDto) {
        categoryModel.Name = categoryDto.Name;
        categoryModel.Color = categoryDto.Color;
        categoryModel.Description = categoryDto.Description;
        categoryModel.AreaId = categoryDto.AreaId;
    }
}