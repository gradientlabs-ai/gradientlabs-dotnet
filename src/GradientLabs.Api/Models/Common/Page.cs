using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class Page<T>
{
    public required IReadOnlyList<T> Items { get; init; }
    public string? Next { get; init; }
    public string? Prev { get; init; }

    public bool HasNextPage => Next is not null;
    public bool HasPrevPage => Prev is not null;

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}
