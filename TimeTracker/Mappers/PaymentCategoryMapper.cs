using TimeTracker.Dtos.PaymentCategory;
using TimeTracker.Models;

namespace TimeTracker.Mappers;

public static class PaymentCategoryMapper {
    public static PaymentCategoryFlatDto ToFlatDto(this PaymentCategory categoryModel) {
        return new PaymentCategoryFlatDto() {
            Id = categoryModel.Id,
            Name = categoryModel.Name,
            Label = categoryModel.Label,
            Description = categoryModel.Description,
        };
    }

    public static PaymentCategoryDto ToDto(this PaymentCategory categoryModel) {
        return new PaymentCategoryDto() {
            Id = categoryModel.Id,
            Name = categoryModel.Name,
            Label = categoryModel.Label,
            Description = categoryModel.Description,
            PaymentCount = categoryModel.PaymentCount,
        };
    }

    public static PaymentCategory ToModel(this CreatePaymentCategoryDto categoryDto) {
        return new PaymentCategory() {
            Name = categoryDto.Name.Trim(),
            Label = NormalizeLabel(categoryDto.Label),
            Description = categoryDto.Description.Trim(),
        };
    }

    public static void UpdateModelFromDto(this PaymentCategory categoryModel, UpdatePaymentCategoryDto categoryDto) {
        categoryModel.Name = categoryDto.Name.Trim();
        categoryModel.Label = NormalizeLabel(categoryDto.Label);
        categoryModel.Description = categoryDto.Description.Trim();
    }

    public static string? NormalizeLabel(string? label) {
        return string.IsNullOrWhiteSpace(label) ? null : label.Trim();
    }
}
