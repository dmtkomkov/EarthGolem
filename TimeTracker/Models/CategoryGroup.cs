using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Models;

public class CategoryGroup {
    public Area? Area { get; set; }
    public List<Category> Categories { get; set; } = [];
}