using TimeTracker.Enums;

namespace TimeTracker.Models;

public class CurrencyTotal {
    public Currency Currency { get; set; }
    public int Amount { get; set; }

    // Always returns every currency in enum order; currencies are never converted or mixed
    public static List<CurrencyTotal> From(IEnumerable<(Currency Currency, int Amount)> amounts) {
        var sums = amounts
            .GroupBy(a => a.Currency)
            .ToDictionary(g => g.Key, g => g.Sum(a => a.Amount));

        return Enum.GetValues<Currency>()
            .Select(c => new CurrencyTotal { Currency = c, Amount = sums.GetValueOrDefault(c) })
            .ToList();
    }
}
