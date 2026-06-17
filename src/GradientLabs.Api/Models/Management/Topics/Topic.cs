using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class Topic
{
    public string Source { get; init; } = string.Empty;
    public string ExternalId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public ArticleVisibility Visibility { get; init; }
    public string? ParentExternalId { get; init; }
    public DateTimeOffset Created { get; init; }
    public DateTimeOffset LastEdited { get; init; }
    public DateTimeOffset? LastSeen { get; init; }
    public byte[]? Data { get; init; }
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
