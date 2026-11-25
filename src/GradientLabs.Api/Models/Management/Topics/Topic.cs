using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class Topic
{
    [JsonPropertyName("Source")]
    public string Source { get; init; } = string.Empty;

    [JsonPropertyName("ExternalID")]
    public string ExternalId { get; init; } = string.Empty;

    [JsonPropertyName("Name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("Description")]
    public string? Description { get; init; }

    [JsonPropertyName("Visibility")]
    public ArticleVisibility Visibility { get; init; }

    [JsonPropertyName("ParentExternalID")]
    public string? ParentExternalId { get; init; }

    [JsonPropertyName("Created")]
    public DateTimeOffset Created { get; init; }

    [JsonPropertyName("LastEdited")]
    public DateTimeOffset LastEdited { get; init; }

    [JsonPropertyName("LastSeen")]
    public DateTimeOffset? LastSeen { get; init; }

    [JsonPropertyName("Data")]
    public byte[]? Data { get; init; }

    [JsonPropertyName("PublicURL")]
    public string? PublicUrl { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class UpsertArticleTopicRequest
{
    public string Id { get; init; } = string.Empty;
    public string? ParentId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public ArticleVisibility Visibility { get; init; }
    public ArticleStatus Status { get; init; }
    public JsonElement? Data { get; init; }
    public DateTimeOffset Created { get; init; }
    public DateTimeOffset LastEdited { get; init; }
    public string? PublicUrl { get; init; }
}

internal sealed class ListTopicsResponse
{
    [JsonPropertyName("Topics")]
    public IReadOnlyList<Topic> Topics { get; init; } = [];
}
