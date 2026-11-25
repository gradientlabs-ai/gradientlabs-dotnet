using GradientLabs.Api.Http;

namespace GradientLabs.Api;

public sealed class KnowledgeBasesClient
{
    private readonly HttpPipeline _pipeline;

    internal KnowledgeBasesClient(HttpPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    public async Task<IReadOnlyList<KnowledgeBase>> ListAsync(CancellationToken cancellationToken = default)
    {
        var response = await _pipeline.SendAsync<ListKnowledgeBasesResponse>(HttpMethod.Get, "/knowledge-bases", null, cancellationToken).ConfigureAwait(false);
        return response.KnowledgeBases;
    }

    public Task<KnowledgeBase> CreateAsync(CreateKnowledgeBaseRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<KnowledgeBase>(HttpMethod.Post, "/knowledge-bases", request, cancellationToken);

    public Task<KnowledgeBase> ReadAsync(string knowledgeBaseId, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<KnowledgeBase>(HttpMethod.Get, $"/knowledge-bases/{Uri.EscapeDataString(knowledgeBaseId)}", null, cancellationToken);

    public Task<KnowledgeBase> UpdateAsync(string knowledgeBaseId, UpdateKnowledgeBaseRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<KnowledgeBase>(HttpMethod.Post, $"/knowledge-bases/{Uri.EscapeDataString(knowledgeBaseId)}", request, cancellationToken);
}
