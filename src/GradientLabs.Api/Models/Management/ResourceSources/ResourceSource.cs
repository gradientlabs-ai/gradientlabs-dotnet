using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class ResourceSource
{
    public string Id { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public string? Description { get; init; }
    public ResourceSourceType SourceType { get; init; }
    public IReadOnlyList<string>? AvailableRefreshStrategies { get; init; }
    public IReadOnlyList<string>? AvailableScopes { get; init; }
    public DateTimeOffset Created { get; init; }
    public DateTimeOffset Updated { get; init; }
    public IReadOnlyDictionary<string, string>? AttributeDescriptions { get; init; }
    public ResourceSourceHttpConfig? HttpConfig { get; init; }
    public ResourceSourceWebhookConfig? WebhookConfig { get; init; }
    public ResourceSchema? Schema { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class ResourceSourceHttpConfig
{
    public string Method { get; init; } = string.Empty;
    public string UrlTemplate { get; init; } = string.Empty;
    public ResourceSourceHttpBodyDefinition? Body { get; init; }
    public IReadOnlyDictionary<string, string>? HeaderTemplates { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class ResourceSourceHttpBodyDefinition
{
    public ResourceSourceBodyEncoding Encoding { get; init; }
    public string? JsonTemplate { get; init; }
    public IReadOnlyDictionary<string, string>? FormFieldTemplates { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class ResourceSourceWebhookConfig
{
    public string Name { get; init; } = string.Empty;

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}
