using api.Dtos.Stock;
using api.Models;

namespace api.Interfaces;

public interface IStockRepository {
    Task<List<Stock>> GetAllAsync();
    Task<Stock?> GetByIdAsync(int id); // First or Default
    Task<Stock> CreateAsync(Stock stockModel);
    Task<Stock?> UpdateAsync(int id, UpdateStockDto stockDto);
    Task<Stock?> DeleteAsync(int id);
}