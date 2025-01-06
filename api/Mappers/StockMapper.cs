using api.Dtos.Stock;
using api.Models;

namespace api.Mappers;

public static class StockMapper {
    public static StockDto ToDto(this Stock stockModel) {
        return new StockDto() {
            Id = stockModel.Id,
            Symbol = stockModel.Symbol,
            CompanyName = stockModel.CompanyName,
            Purchase = stockModel.Purchase,
            LastDiv = stockModel.LastDiv,
            Industry = stockModel.Industry,
            MarketCap = stockModel.MarketCap,
            Comments = stockModel.Comments.Select(c => c.ToDto()).ToList(),
        };
    }
    
    public static void UpdateModelFromDto(this Stock model, UpdateStockDto dto)
    {
        model.Symbol = dto.Symbol;
        model.CompanyName = dto.CompanyName;
        model.Purchase = dto.Purchase;
        model.LastDiv = dto.LastDiv;
        model.Industry = dto.Industry;
        model.MarketCap = dto.MarketCap;
    }

    public static Stock ToModel(this CreateStockDto stockDto) {
        return new Stock() {
            Symbol = stockDto.Symbol,
            CompanyName = stockDto.CompanyName,
            Purchase = stockDto.Purchase,
            LastDiv = stockDto.LastDiv,
            Industry = stockDto.Industry,
            MarketCap = stockDto.MarketCap,
        };
    }
}