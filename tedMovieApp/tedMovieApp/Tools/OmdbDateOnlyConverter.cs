
using System.Text.Json;
using System.Text.Json.Serialization;

namespace tedMovieApp.Tools;

public sealed class OmdbDateOnlyConverter : JsonConverter<DateOnly>
{
    private const string Format = "dd MMM yyyy";

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();

        if (str == "N/A")
            return DateOnly.MinValue;
            
        if (DateOnly.TryParseExact(str, Format, out var date))
            return date;
        
        throw new JsonException($"Invalid OMDB date format: {str}");
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(
            value == DateOnly.MinValue ? "N/A" : value.ToString(Format)
        );
    }
}