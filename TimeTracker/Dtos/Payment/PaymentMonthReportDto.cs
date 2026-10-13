namespace TimeTracker.Dtos.Payment;

public class PaymentMonthReportDto {
    public int Year { get; set; }
    public int Month { get; set; }
    public List<CurrencyTotalDto> Totals { get; set; } = [];
    public List<PaymentCategoryTotalDto> Categories { get; set; } = [];
}
