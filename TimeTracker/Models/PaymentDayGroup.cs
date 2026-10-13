namespace TimeTracker.Models;

public class PaymentDayGroup {
    public DateOnly SpentOn { get; set; }
    public List<CurrencyTotal> Totals { get; set; } = [];
    public List<Payment> Payments { get; set; } = [];
}
