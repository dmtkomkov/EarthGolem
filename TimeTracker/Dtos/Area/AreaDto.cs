using System.ComponentModel.DataAnnotations;
using TimeTracker.Dtos.Category;

namespace TimeTracker.Dtos.Area;

public class AreaDto : AreaFlatDto {
    [Required] public List<CategoryFlatDto> Categories { get; set; } = [];
}