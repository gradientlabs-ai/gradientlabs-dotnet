using GradientLabs.Api.Http;

namespace GradientLabs.Api;

public sealed class ArticlesClient
{
    private readonly HttpPipeline _pipeline;

    internal ArticlesClient(HttpPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    public Task UpsertAsync(UpsertArticleRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Post, "/articles", request, cancellationToken);

    public Task DeleteAsync(string articleId, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Delete, $"/articles/{Uri.EscapeDataString(articleId)}", null, cancellationToken);

    public Task SetUsageStatusAsync(string articleId, SetArticleUsageStatusRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Post, $"/articles/{Uri.EscapeDataString(articleId)}/usage-status", request, cancellationToken);
}
