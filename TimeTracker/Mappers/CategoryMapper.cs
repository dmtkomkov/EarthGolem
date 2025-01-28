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
}