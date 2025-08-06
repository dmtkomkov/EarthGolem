using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace TimeTracker.Models;

public class Category {
    public int Id { get; set; }
    [MaxLength(36)] public string Name { get; set; } = string.Empty;
    public Color Color { get; set; }
    [MaxLength(500)] public string Description { get; set; } = string.Empty;
    [Required] public int AreaId { get; set; }
    public Area? Area { get; set; }
    public List<Step> Steps { get; set; } = [];

    [NotMapped] public int StepCount => Steps?.Count ?? 0;
}