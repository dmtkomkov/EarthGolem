using TimeTracker.Dtos.PaymentCategory;
using TimeTracker.Models;

namespace TimeTracker.Interfaces;

public interface IPaymentCategoryRepository {
    Task<List<PaymentCategory>> GetAllAsync();
    Task<PaymentCategory?> GetByIdAsync(int id);
    Task<PaymentCategory> CreateAsync(CreatePaymentCategoryDto categoryDto);
    Task<PaymentCategory?> UpdateAsync(int id, UpdatePaymentCategoryDto categoryDto);
    Task<PaymentCategory?> DeleteAsync(int id);
    Task<bool> PaymentCategoryExists(int id);
    Task<bool> NameExistsAsync(string name, int? excludeId = null);
    Task<bool> LabelExistsAsync(string? label, int? excludeId = null);
    Task<bool> HasPaymentsAsync(int id);
}
