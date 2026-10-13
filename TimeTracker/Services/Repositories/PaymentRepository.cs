using Microsoft.EntityFrameworkCore;
using TimeTracker.Data;
using TimeTracker.Dtos.Payment;
using TimeTracker.Interfaces;
using TimeTracker.Mappers;
using TimeTracker.Models;

namespace TimeTracker.Services.Repositories;

public class PaymentRepository(ApplicationDbContext context) : IPaymentRepository {
    public async Task<List<Payment>> GetAllAsync(DateOnly? dateFilter) {
        IQueryable<Payment> query = context.Payments
            .AsNoTracking()
            .Include(p => p.Category);

        if (dateFilter.HasValue) {
            query = query.Where(p => p.SpentOn == dateFilter.Value);
        }

        return await query
            .OrderBy(p => p.Id)
            .ToListAsync();
    }

    public async Task<PagedList<PaymentDayGroup>> GetAllGroupedByDateAsync(int pageNumber, int pageSize) {
        // Paging applies to days, not payments
        var days = context.Payments
            .Select(p => p.SpentOn)
            .Distinct();

        var totalCount = await days.CountAsync();

        var pageDays = await days
            .OrderByDescending(d => d)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        if (pageDays.Count == 0) {
            return new PagedList<PaymentDayGroup>([], pageNumber, pageSize, totalCount);
        }

        // Page days are consecutive among days with payments, so the range contains only page days
        var newestDay = pageDays.First();
        var oldestDay = pageDays.Last();

        var payments = await context.Payments
            .AsNoTracking()
            .Include(p => p.Category)
            .Where(p => p.SpentOn >= oldestDay && p.SpentOn <= newestDay)
            .OrderBy(p => p.Id)
            .ToListAsync();

        var groups = payments
            .GroupBy(p => p.SpentOn)
            .OrderByDescending(g => g.Key)
            .Select(g => new PaymentDayGroup() {
                SpentOn = g.Key,
                Totals = CurrencyTotal.From(g.Select(p => (p.Currency, p.Amount))),
                Payments = g.ToList(),
            })
            .ToList();

        return new PagedList<PaymentDayGroup>(groups, pageNumber, pageSize, totalCount);
    }

    public async Task<PagedList<PaymentMonthReport>> GetMonthlyReportAsync(int pageNumber, int pageSize) {
        // Paging applies to months that have payments
        var months = context.Payments
            .Select(p => new { p.SpentOn.Year, p.SpentOn.Month })
            .Distinct();

        var totalCount = await months.CountAsync();

        var pageMonths = await months
            .OrderByDescending(m => m.Year)
            .ThenByDescending(m => m.Month)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        if (pageMonths.Count == 0) {
            return new PagedList<PaymentMonthReport>([], pageNumber, pageSize, totalCount);
        }

        // Page months are consecutive among months with payments, so the range contains only page months
        var newestMonth = pageMonths.First();
        var oldestMonth = pageMonths.Last();
        var rangeStart = new DateOnly(oldestMonth.Year, oldestMonth.Month, 1);
        var rangeEnd = new DateOnly(newestMonth.Year, newestMonth.Month, 1).AddMonths(1).AddDays(-1);

        var sums = await context.Payments
            .Where(p => p.SpentOn >= rangeStart && p.SpentOn <= rangeEnd)
            .GroupBy(p => new { p.SpentOn.Year, p.SpentOn.Month, p.CategoryId, p.Currency })
            .Select(g => new {
                g.Key.Year,
                g.Key.Month,
                g.Key.CategoryId,
                g.Key.Currency,
                Amount = g.Sum(p => p.Amount),
            })
            .ToListAsync();

        // Every category is reported, in the same order as the category list
        var categoryIds = await context.PaymentCategories
            .OrderBy(c => c.Id)
            .Select(c => c.Id)
            .ToListAsync();

        var reports = pageMonths
            .Select(m => {
                var monthSums = sums.Where(s => s.Year == m.Year && s.Month == m.Month).ToList();

                return new PaymentMonthReport() {
                    Year = m.Year,
                    Month = m.Month,
                    Totals = CurrencyTotal.From(monthSums.Select(s => (s.Currency, s.Amount))),
                    Categories = categoryIds
                        .Select(id => new PaymentCategoryTotal() {
                            CategoryId = id,
                            Totals = CurrencyTotal.From(monthSums
                                .Where(s => s.CategoryId == id)
                                .Select(s => (s.Currency, s.Amount))),
                        })
                        .ToList(),
                };
            })
            .ToList();

        return new PagedList<PaymentMonthReport>(reports, pageNumber, pageSize, totalCount);
    }

    public async Task<PaymentSummary> GetSummaryAsync(DateOnly date) {
        var monthStart = new DateOnly(date.Year, date.Month, 1);
        var monthEnd = monthStart.AddMonths(1).AddDays(-1);

        var sums = await context.Payments
            .Where(p => p.SpentOn >= monthStart && p.SpentOn <= monthEnd)
            .GroupBy(p => new { p.SpentOn, p.Currency })
            .Select(g => new { g.Key.SpentOn, g.Key.Currency, Amount = g.Sum(p => p.Amount) })
            .ToListAsync();

        return new PaymentSummary() {
            Date = date,
            Day = CurrencyTotal.From(sums.Where(s => s.SpentOn == date).Select(s => (s.Currency, s.Amount))),
            Month = CurrencyTotal.From(sums.Select(s => (s.Currency, s.Amount))),
        };
    }

    public async Task<Payment?> GetByIdAsync(int id) {
        return await context.Payments
            .AsNoTracking()
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Payment?> CreateAsync(CreatePaymentDto paymentDto) {
        var paymentModel = paymentDto.ToModel();
        await context.Payments.AddAsync(paymentModel);
        await context.SaveChangesAsync();

        return await GetByIdAsync(paymentModel.Id);
    }

    public async Task<Payment?> UpdateAsync(int id, UpdatePaymentDto paymentDto) {
        var paymentModel = await context.Payments.FindAsync(id);

        if (paymentModel == null) {
            return null;
        }

        paymentModel.UpdateModelFromDto(paymentDto);
        await context.SaveChangesAsync();

        return await GetByIdAsync(paymentModel.Id);
    }

    public async Task<Payment?> DeleteAsync(int id) {
        var paymentModel = await context.Payments.FindAsync(id);

        if (paymentModel == null) {
            return null;
        }

        context.Payments.Remove(paymentModel);
        await context.SaveChangesAsync();
        return paymentModel;
    }
}
