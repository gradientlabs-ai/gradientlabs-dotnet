using GradientLabs.Api.Http;

namespace GradientLabs.Api;

public sealed class TopicsClient
{
    private readonly HttpPipeline _pipeline;

    internal TopicsClient(HttpPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    public Task<IReadOnlyList<Topic>> ListAsync(string? supportPlatform = null, CancellationToken cancellationToken = default)
    {
        var qs = QueryBuilder.Build(new Dictionary<string, string?> { ["support_platform"] = supportPlatform });
        return _pipeline.SendAsync<IReadOnlyList<Topic>>(HttpMethod.Get, $"/topics{qs}", null, cancellationToken);
    }

    public Task<Topic> ReadAsync(string topicId, string? supportPlatform = null, CancellationToken cancellationToken = default)
    {
        var qs = QueryBuilder.Build(new Dictionary<string, string?> { ["support_platform"] = supportPlatform });
        return _pipeline.SendAsync<Topic>(HttpMethod.Get, $"/topics/{Uri.EscapeDataString(topicId)}{qs}", null, cancellationToken);
    }

    public Task UpsertArticleTopicAsync(string supportPlatform, UpsertArticleTopicRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Put, $"/support-platforms/{Uri.EscapeDataString(supportPlatform)}/topics/{Uri.EscapeDataString(request.Id)}", request, cancellationToken);
}
