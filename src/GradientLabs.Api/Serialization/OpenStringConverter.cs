using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api.Serialization;

internal sealed class OpenStringConverter<T> : JsonConverter<T>
{
    private readonly Func<string, T> _factory;

    public OpenStringConverter(Func<string, T> factory)
    {
        _factory = factory;
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString() ?? string.Empty;
        return _factory(value);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.ToString());
    }
}
