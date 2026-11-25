using GradientLabs.Api.Http;

namespace GradientLabs.Api;

public sealed class ResourceSourcesClient
{
    private readonly HttpPipeline _pipeline;

    internal ResourceSourcesClient(HttpPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    public async Task<IReadOnlyList<ResourceSource>> ListAsync(CancellationToken cancellationToken = default)
    {
        var response = await _pipeline.SendAsync<ListResourceSourcesResponse>(HttpMethod.Get, "/resource-sources", null, cancellationToken).ConfigureAwait(false);
        return response.ResourceSources;
    }

    public Task<ResourceSource> CreateAsync(CreateResourceSourceRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<ResourceSource>(HttpMethod.Post, "/resource-sources", request, cancellationToken);

    public Task<ResourceSource> ReadAsync(string sourceId, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<ResourceSource>(HttpMethod.Get, $"/resource-sources/{Uri.EscapeDataString(sourceId)}", null, cancellationToken);

    public Task<ResourceSource> UpdateAsync(string sourceId, UpdateResourceSourceRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<ResourceSource>(HttpMethod.Put, $"/resource-sources/{Uri.EscapeDataString(sourceId)}", request, cancellationToken);

    public Task DeleteAsync(string sourceId, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Delete, $"/resource-sources/{Uri.EscapeDataString(sourceId)}", null, cancellationToken);

    public Task<ResourceSource> UpdateSchemaByExamplesAsync(string sourceId, UpdateSchemaByExamplesRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<ResourceSource>(HttpMethod.Post, $"/resource-sources/{Uri.EscapeDataString(sourceId)}/schema-by-examples", request, cancellationToken);
}
