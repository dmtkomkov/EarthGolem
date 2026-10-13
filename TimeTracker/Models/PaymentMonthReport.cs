namespace TimeTracker.Models;

public class PaymentMonthReport {
    public int Year { get; set; }
    public int Month { get; set; }
    public List<CurrencyTotal> Totals { get; set; } = [];
    public List<PaymentCategoryTotal> Categories { get; set; } = [];
}
