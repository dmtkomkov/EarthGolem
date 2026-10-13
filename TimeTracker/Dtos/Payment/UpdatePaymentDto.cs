using System.ComponentModel.DataAnnotations;
using TimeTracker.Enums;

namespace TimeTracker.Dtos.Payment;

public class UpdatePaymentDto {
    [Range(1, int.MaxValue)] public int Amount { get; set; }

    // Nullable so that a missing value fails validation instead of becoming a default
    [Required] public DateOnly? SpentOn { get; set; }
    [Required] [EnumDataType(typeof(Currency))] public Currency? Currency { get; set; }

    [Range(1, int.MaxValue)] public int CategoryId { get; set; }
}
