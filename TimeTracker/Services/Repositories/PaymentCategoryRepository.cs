using Microsoft.EntityFrameworkCore;
using TimeTracker.Data;
using TimeTracker.Dtos.PaymentCategory;
using TimeTracker.Interfaces;
using TimeTracker.Mappers;
using TimeTracker.Models;

namespace TimeTracker.Services.Repositories;

public class PaymentCategoryRepository(ApplicationDbContext context) : IPaymentCategoryRepository {
    public async Task<List<PaymentCategory>> GetAllAsync() {
        // Ascending id order is the column order of the monthly report
        return await QueryWithPaymentCount()
            .OrderBy(c => c.Id)
            .ToListAsync();
    }

    public async Task<PaymentCategory?> GetByIdAsync(int id) {
        return await QueryWithPaymentCount()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<PaymentCategory> CreateAsync(CreatePaymentCategoryDto categoryDto) {
        var categoryModel = categoryDto.ToModel();
        await context.PaymentCategories.AddAsync(categoryModel);
        await context.SaveChangesAsync();
        return categoryModel;
    }

    public async Task<PaymentCategory?> UpdateAsync(int id, UpdatePaymentCategoryDto categoryDto) {
        var categoryModel = await context.PaymentCategories.FindAsync(id);

        if (categoryModel == null) {
            return null;
        }

        categoryModel.UpdateModelFromDto(categoryDto);
        await context.SaveChangesAsync();
        return categoryModel;
    }

    public async Task<PaymentCategory?> DeleteAsync(int id) {
        var categoryModel = await context.PaymentCategories.FindAsync(id);

        if (categoryModel == null) {
            return null;
        }

        context.PaymentCategories.Remove(categoryModel);
        await context.SaveChangesAsync();
        return categoryModel;
    }

    public async Task<bool> PaymentCategoryExists(int id) {
        return await context.PaymentCategories.AnyAsync(c => c.Id == id);
    }

    public async Task<bool> NameExistsAsync(string name, int? excludeId = null) {
        var normalizedName = name.Trim();

        // Comparison is case-insensitive through the NOCASE column collation
        return await context.PaymentCategories
            .AnyAsync(c => c.Name == normalizedName && (excludeId == null || c.Id != excludeId));
    }

    public async Task<bool> LabelExistsAsync(string? label, int? excludeId = null) {
        var normalizedLabel = PaymentCategoryMapper.NormalizeLabel(label);

        if (normalizedLabel == null) {
            return false;
        }

        return await context.PaymentCategories
            .AnyAsync(c => c.Label == normalizedLabel && (excludeId == null || c.Id != excludeId));
    }

    public async Task<bool> HasPaymentsAsync(int id) {
        return await context.Payments.AnyAsync(p => p.CategoryId == id);
    }

    private IQueryable<PaymentCategory> QueryWithPaymentCount() {
        return context.PaymentCategories
            .AsNoTracking()
            .Select(c => new PaymentCategory() {
                Id = c.Id,
                Name = c.Name,
                Label = c.Label,
                Description = c.Description,
                PaymentCount = c.Payments.Count,
            });
    }
}
