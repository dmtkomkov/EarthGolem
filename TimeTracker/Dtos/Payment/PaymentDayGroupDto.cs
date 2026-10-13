namespace TimeTracker.Dtos.Payment;

public class PaymentDayGroupDto {
    public DateOnly SpentOn { get; set; }
    public List<CurrencyTotalDto> Totals { get; set; } = [];
    public List<PaymentDto> Payments { get; set; } = [];
}
