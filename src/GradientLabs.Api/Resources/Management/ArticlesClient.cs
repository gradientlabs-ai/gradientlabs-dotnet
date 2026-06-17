using GradientLabs.Api.Http;

namespace GradientLabs.Api;

public sealed class ArticlesClient
{
    private readonly HttpPipeline _pipeline;

    internal ArticlesClient(HttpPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    public Task UpsertAsync(string supportPlatform, UpsertArticleRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Put, $"/support-platforms/{Uri.EscapeDataString(supportPlatform)}/articles/{Uri.EscapeDataString(request.Id)}", request, cancellationToken);

    public Task DeleteAsync(string supportPlatform, string articleId, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Delete, $"/support-platforms/{Uri.EscapeDataString(supportPlatform)}/articles/{Uri.EscapeDataString(articleId)}", null, cancellationToken);

    public Task SetUsageStatusAsync(string supportPlatform, string articleId, SetArticleUsageStatusRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Put, $"/support-platforms/{Uri.EscapeDataString(supportPlatform)}/articles/{Uri.EscapeDataString(articleId)}/usage-status", request, cancellationToken);
}
