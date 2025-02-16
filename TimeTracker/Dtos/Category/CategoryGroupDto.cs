using TimeTracker.Dtos.Area;

namespace TimeTracker.Dtos.Category;

public class CategoryGroupDto {
    public AreaFlatDto? Area { get; set; }
    public List<CategoryFlatDto> Categories { get; set; } = [];
}