namespace TimeTracker.Dtos.Payment;

public class PaymentCategoryTotalDto {
    public int CategoryId { get; set; }
    public List<CurrencyTotalDto> Totals { get; set; } = [];
}
