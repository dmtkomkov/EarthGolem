using System.ComponentModel.DataAnnotations;
using System.Drawing;
using TimeTracker.Enums;

namespace TimeTracker.Dtos.Project;

public class UpdateProjectDto {
    [MaxLength(36)]
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public Color Color { get; set; }
    [MaxLength(500, ErrorMessage = "Content can not be over 500 characters")]
    public string Description { get; set; } = string.Empty;
    [Required]
    public ProjectStatus Status { get; set; }
}