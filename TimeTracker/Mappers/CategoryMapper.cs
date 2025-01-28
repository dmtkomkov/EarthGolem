using TimeTracker.Dtos.Category;
using TimeTracker.Models;

namespace TimeTracker.Mappers;

public static class CategoryMapper {
    public static CategoryDto ToDto(this Category categoryModel) {
        return new CategoryDto() {
            Id = categoryModel.Id,
            Name = categoryModel.Name,
            Color = categoryModel.Color,
            Description = categoryModel.Description,
            AreaId = categoryModel.AreaId,
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