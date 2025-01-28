using System.ComponentModel.DataAnnotations;
using System.Drawing;
using TimeTracker.Dtos.Category;

namespace TimeTracker.Dtos.Area;

public class AreaDto {
    public int Id { get; set; }
    [Required]
    [MaxLength(36)]
    public string Name { get; set; } = string.Empty;
    [Required]
    public Color Color { get; set; }
    [MaxLength(500, ErrorMessage = "Content can not be over 500 characters")]
    public string Description { get; set; } = string.Empty;
    [Required]
    public List<CategoryDto> Categories { get; set; } = [];
}