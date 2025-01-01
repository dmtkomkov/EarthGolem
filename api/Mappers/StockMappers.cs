using api.Dtos.Stock;
using api.Models;

namespace api.Mappers;

public static class StockMappers {
    public static StockDto ToStockDto(this Stock stockModel) {
        return new StockDto() {
            Id = stockModel.Id,
            Symbol = stockModel.Symbol,
            CompanyName = stockModel.CompanyName,
            Purchase = stockModel.Purchase,
            LastDiv = stockModel.LastDiv,
            Industry = stockModel.Industry,
            MarketCap = stockModel.MarketCap,
        };
    }
    
    public static void UpdateFromDto(this Stock stock, UpdateStockDto dto)
    {
        stock.Symbol = dto.Symbol;
        stock.CompanyName = dto.CompanyName;
        stock.Purchase = dto.Purchase;
        stock.LastDiv = dto.LastDiv;
        stock.Industry = dto.Industry;
        stock.MarketCap = dto.MarketCap;
    }

    public static Stock ToStock(this CreateStockDto stockDto) {
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