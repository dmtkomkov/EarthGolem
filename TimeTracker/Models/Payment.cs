using System.ComponentModel.DataAnnotations;
using TimeTracker.Enums;

namespace TimeTracker.Models;

public class Payment {
    public int Id { get; set; }
    public int Amount { get; set; }
    public DateOnly SpentOn { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public Currency Currency { get; set; }
    [Required] public int CategoryId { get; set; }
    public PaymentCategory? Category { get; set; }
}
