using TimeTracker.Enums;

namespace TimeTracker.Dtos.Payment;

public class CurrencyTotalDto {
    public Currency Currency { get; set; }
    public int Amount { get; set; }
}
