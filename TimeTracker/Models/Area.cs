using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace TimeTracker.Models;

public class Area {
    public int Id { get; set; }
    [MaxLength(36)]
    public string Name { get; set; } = string.Empty;
    public Color Color { get; set; }
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    public List<Category> Categories { get; set; } = [];
}