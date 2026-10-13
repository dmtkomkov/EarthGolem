namespace TimeTracker.Dtos.Payment;

public class PaymentSummaryDto {
    public DateOnly Date { get; set; }
    public List<CurrencyTotalDto> Day { get; set; } = [];
    public List<CurrencyTotalDto> Month { get; set; } = [];
}
