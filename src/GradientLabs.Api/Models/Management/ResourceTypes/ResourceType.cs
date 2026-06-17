using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class ResourceType
{
    public string Id { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public string? Description { get; init; }
    public ResourceTypeScope Scope { get; init; }
    public ResourceTypeRefreshStrategy RefreshStrategy { get; init; }
    public bool IsEnabled { get; init; }
    public DateTimeOffset Created { get; init; }
    public DateTimeOffset Updated { get; init; }
    public ResourceSchema? Schema { get; init; }
    public ResourceTypeSourceConfig? SourceConfig { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class ResourceSchema
{
    public JsonElement Raw { get; init; }
    public IReadOnlyList<ResourceAttribute>? Attributes { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class ResourceAttribute
{
    public string Path { get; init; } = string.Empty;
    public AttributeType Type { get; init; }
    public AttributeCardinality Cardinality { get; init; }
    public string? Description { get; init; }
    public bool IsRoot { get; init; }
    public string? Name { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class ResourceTypeSourceConfig
{
    public string SourceId { get; init; } = string.Empty;
    public IReadOnlyList<string> Attributes { get; init; } = [];
    public bool Cache { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}
