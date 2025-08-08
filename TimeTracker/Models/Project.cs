using System.ComponentModel.DataAnnotations;
using System.Drawing;
using TimeTracker.Enums;

namespace TimeTracker.Models;

public class Project {
    public int Id { get; set; }
    [MaxLength(36)] public string Name { get; set; } = string.Empty;
    public Color Color { get; set; }
    [MaxLength(500)] public string Description { get; set; } = string.Empty;
    [Required] public ProjectStatus Status { get; set; } = ProjectStatus.Open;
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public List<Goal> Goals { get; set; } = [];
}