using System.Text.Json;

namespace GradientLabs.Api;

public sealed class CreateResourceSourceRequest
{
    public string DisplayName { get; init; } = string.Empty;
    public ResourceSourceType SourceType { get; init; }
    public string? Description { get; init; }
    public IReadOnlyDictionary<string, string>? AttributeDescriptions { get; init; }
    public ResourceSourceHttpConfig? HttpConfig { get; init; }
    public ResourceSourceWebhookConfig? WebhookConfig { get; init; }
}

public sealed class UpdateResourceSourceRequest
{
    public string? DisplayName { get; init; }
    public string? Description { get; init; }
    public IReadOnlyDictionary<string, string>? AttributeDescriptions { get; init; }
    public ResourceSourceHttpConfig? HttpConfig { get; init; }
    public ResourceSourceWebhookConfig? WebhookConfig { get; init; }
    public ResourceSourceType? SourceType { get; init; }
    public ResourceSchema? Schema { get; init; }
}

public sealed class UpdateSchemaByExamplesRequest
{
    public IReadOnlyList<JsonElement> Examples { get; init; } = [];
    public SchemaUpdateStrategy? SchemaUpdateStrategy { get; init; }
}
