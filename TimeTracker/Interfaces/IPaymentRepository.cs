using TimeTracker.Dtos.Payment;
using TimeTracker.Models;

namespace TimeTracker.Interfaces;

public interface IPaymentRepository {
    Task<List<Payment>> GetAllAsync(DateOnly? date);
    Task<PagedList<PaymentDayGroup>> GetAllGroupedByDateAsync(int pageNumber, int pageSize);
    Task<PagedList<PaymentMonthReport>> GetMonthlyReportAsync(int pageNumber, int pageSize);
    Task<PaymentSummary> GetSummaryAsync(DateOnly date);
    Task<Payment?> GetByIdAsync(int id);
    Task<Payment?> CreateAsync(CreatePaymentDto paymentDto);
    Task<Payment?> UpdateAsync(int id, UpdatePaymentDto paymentDto);
    Task<Payment?> DeleteAsync(int id);
}
