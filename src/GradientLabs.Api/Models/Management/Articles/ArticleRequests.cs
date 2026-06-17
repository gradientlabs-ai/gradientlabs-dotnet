using System.Text.Json;

namespace GradientLabs.Api;

public sealed class UpsertArticleRequest
{
    public string AuthorId { get; init; } = string.Empty;
    public string Id { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string Body { get; init; } = string.Empty;
    public ArticleVisibility Visibility { get; init; }
    public string? TopicId { get; init; }
    public ArticleStatus Status { get; init; }
    public JsonElement? Data { get; init; }
    public DateTimeOffset Created { get; init; }
    public DateTimeOffset LastEdited { get; init; }
    public string? PublicUrl { get; init; }
}

public sealed class SetArticleUsageStatusRequest
{
    public ArticleUsageStatus UsageStatus { get; init; }
}
