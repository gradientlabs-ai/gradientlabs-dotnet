using System.Text.Json;

namespace GradientLabs.Api;

public sealed class BatchCreateMemoriesRequest
{
    public IReadOnlyList<Memory> Memories { get; init; } = Array.Empty<Memory>();
}

public sealed class Memory
{
    public string ExternalId { get; init; } = string.Empty;
    public string? CustomType { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public JsonElement Data { get; init; }
}
