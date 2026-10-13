namespace TimeTracker.Models;

public class PaymentCategoryTotal {
    public int CategoryId { get; set; }
    public List<CurrencyTotal> Totals { get; set; } = [];
}
