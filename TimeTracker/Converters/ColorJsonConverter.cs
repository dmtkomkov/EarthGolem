using System.Drawing;
using Newtonsoft.Json;
using TimeTracker.Exceptions;

public class ColorJsonConverter() : JsonConverter<Color>
{
    public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
    {
        writer.WriteValue(ColorTranslator.ToHtml(value));
    }

    public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var colorString = (string)reader.Value!;
        try {
            return ColorTranslator.FromHtml(colorString);
        }
        catch {
            throw new InvalidColorFormatException($"Invalid color format: {colorString}. Expected format is '#RRGGBB'.");
        }
    }
}