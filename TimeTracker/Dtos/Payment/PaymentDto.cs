using TimeTracker.Dtos.PaymentCategory;
using TimeTracker.Enums;

namespace TimeTracker.Dtos.Payment;

public class PaymentDto {
    public int Id { get; set; }
    public int Amount { get; set; }
    public DateOnly SpentOn { get; set; }
    public Currency Currency { get; set; }
    public required PaymentCategoryFlatDto Category { get; set; }
}
