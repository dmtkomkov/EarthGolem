using TimeTracker.Dtos.Payment;
using TimeTracker.Models;

namespace TimeTracker.Mappers;

public static class PaymentMapper {
    public static CurrencyTotalDto ToDto(this CurrencyTotal currencyTotal) {
        return new CurrencyTotalDto() {
            Currency = currencyTotal.Currency,
            Amount = currencyTotal.Amount,
        };
    }

    public static PaymentDto ToDto(this Payment paymentModel) {
        return new PaymentDto() {
            Id = paymentModel.Id,
            Amount = paymentModel.Amount,
            SpentOn = paymentModel.SpentOn,
            Currency = paymentModel.Currency,
            Category = paymentModel.Category!.ToFlatDto(),
        };
    }

    public static PaymentDayGroupDto ToDto(this PaymentDayGroup paymentDayGroup) {
        return new PaymentDayGroupDto() {
            SpentOn = paymentDayGroup.SpentOn,
            Totals = paymentDayGroup.Totals.Select(t => t.ToDto()).ToList(),
            Payments = paymentDayGroup.Payments.Select(p => p.ToDto()).ToList(),
        };
    }

    public static PagedPaymentDayGroupDto ToDto(this PagedList<PaymentDayGroup> pagedList) {
        return new PagedPaymentDayGroupDto() {
            Items = pagedList.Items.Select(g => g.ToDto()).ToList(),
            PageNumber = pagedList.PageNumber,
            PageSize = pagedList.PageSize,
            TotalCount = pagedList.TotalCount,
            TotalPages = pagedList.TotalPages,
            HasNextPage = pagedList.HasNextPage,
            HasPreviousPage = pagedList.HasPreviousPage,
        };
    }

    public static PaymentMonthReportDto ToDto(this PaymentMonthReport monthReport) {
        return new PaymentMonthReportDto() {
            Year = monthReport.Year,
            Month = monthReport.Month,
            Totals = monthReport.Totals.Select(t => t.ToDto()).ToList(),
            Categories = monthReport.Categories.Select(c => new PaymentCategoryTotalDto() {
                CategoryId = c.CategoryId,
                Totals = c.Totals.Select(t => t.ToDto()).ToList(),
            }).ToList(),
        };
    }

    public static PagedPaymentMonthReportDto ToDto(this PagedList<PaymentMonthReport> pagedList) {
        return new PagedPaymentMonthReportDto() {
            Items = pagedList.Items.Select(r => r.ToDto()).ToList(),
            PageNumber = pagedList.PageNumber,
            PageSize = pagedList.PageSize,
            TotalCount = pagedList.TotalCount,
            TotalPages = pagedList.TotalPages,
            HasNextPage = pagedList.HasNextPage,
            HasPreviousPage = pagedList.HasPreviousPage,
        };
    }

    public static PaymentSummaryDto ToDto(this PaymentSummary paymentSummary) {
        return new PaymentSummaryDto() {
            Date = paymentSummary.Date,
            Day = paymentSummary.Day.Select(t => t.ToDto()).ToList(),
            Month = paymentSummary.Month.Select(t => t.ToDto()).ToList(),
        };
    }

    public static Payment ToModel(this CreatePaymentDto paymentDto) {
        return new Payment() {
            Amount = paymentDto.Amount,
            SpentOn = paymentDto.SpentOn!.Value,
            Currency = paymentDto.Currency!.Value,
            CategoryId = paymentDto.CategoryId,
        };
    }

    public static void UpdateModelFromDto(this Payment paymentModel, UpdatePaymentDto paymentDto) {
        paymentModel.Amount = paymentDto.Amount;
        paymentModel.SpentOn = paymentDto.SpentOn!.Value;
        paymentModel.Currency = paymentDto.Currency!.Value;
        paymentModel.CategoryId = paymentDto.CategoryId;
    }
}
