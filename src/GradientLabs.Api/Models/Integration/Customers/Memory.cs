using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class Memory
{
    /// <summary>Your own identifier for the memory, unique within your company. Retrying the same memory does not create a duplicate.</summary>
    public string ExternalId { get; init; } = string.Empty;

    /// <summary>The customer-defined category for the memory.</summary>
    public MemoryCustomType CustomType { get; init; } = new();

    /// <summary>The time the memory's underlying event occurred.</summary>
    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>Any JSON value stored verbatim as the memory's payload.</summary>
    public JsonElement Data { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class MemoryCustomType
{
    /// <summary>The name of the memory category. <c>support_conversation</c> is reserved for Gradient Labs.</summary>
    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class BatchCreateMemoriesRequest
{
    public IReadOnlyList<Memory> Memories { get; init; } = [];
}

public sealed class DeprecatedMemory
{
    public string ExternalId { get; init; } = string.Empty;
    public string CustomType { get; init; } = string.Empty;
    public DateTimeOffset CreatedAt { get; init; }
    public JsonElement Data { get; init; }
}

public sealed class BatchCreateMemoriesDeprecatedRequest
{
    public IReadOnlyList<DeprecatedMemory> Memories { get; init; } = [];
}

internal sealed class ListMemoriesResponse
{
    public IReadOnlyList<Memory> Memories { get; init; } = [];
}
