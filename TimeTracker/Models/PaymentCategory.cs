using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTracker.Models;

public class PaymentCategory {
    public int Id { get; set; }
    [MaxLength(36)] public string Name { get; set; } = string.Empty;
    [MaxLength(4)] public string? Label { get; set; }
    [MaxLength(500)] public string Description { get; set; } = string.Empty;
    public List<Payment> Payments { get; set; } = [];

    // Filled by repository projections; payments are not loaded to count them
    [NotMapped] public int PaymentCount { get; set; }
}
