using TimeTracker.Dtos.Area;

namespace TimeTracker.Dtos.Category;

public class CategoryDto : CategoryFlatDto {
    public AreaFlatDto Area { get; set; }
}