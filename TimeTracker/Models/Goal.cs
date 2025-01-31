using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace TimeTracker.Models;

public class Goal {
    public int Id { get; set; }
    [MaxLength(36)]
    public string Name { get; set; } = string.Empty;
    public Color Color { get; set; }
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    public DateOnly? StartDate { get; set; }
    public DateOnly? CloseDate { get; set; }
    public int? ProjectId { get; set; }
    public Project? Project { get; set; }
}