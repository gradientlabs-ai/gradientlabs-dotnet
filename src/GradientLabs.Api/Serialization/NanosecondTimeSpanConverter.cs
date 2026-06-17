using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api.Serialization;

internal sealed class NanosecondTimeSpanConverter : JsonConverter<TimeSpan>
{
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var ns = reader.GetInt64();
        return TimeSpan.FromTicks(ns / 100);
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        checked
        {
            writer.WriteNumberValue(value.Ticks * 100L);
        }
    }
}
