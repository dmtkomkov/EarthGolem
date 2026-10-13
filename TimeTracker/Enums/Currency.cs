using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TimeTracker.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum Currency {
    PLN,
    EUR,
}
