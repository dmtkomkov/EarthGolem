namespace TimeTracker.Models;

public class PaymentSummary {
    public DateOnly Date { get; set; }
    public List<CurrencyTotal> Day { get; set; } = [];
    public List<CurrencyTotal> Month { get; set; } = [];
}
