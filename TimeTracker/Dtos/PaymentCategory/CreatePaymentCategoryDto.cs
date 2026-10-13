using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Dtos.PaymentCategory;

public class CreatePaymentCategoryDto {
    [MaxLength(36)] [Required] public string Name { get; set; } = string.Empty;
    [MaxLength(4)] public string? Label { get; set; }

    [MaxLength(500, ErrorMessage = "Content can not be over 500 characters")]
    public string Description { get; set; } = string.Empty;
}
