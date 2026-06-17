using GradientLabs.Api.Http;

namespace GradientLabs.Api;

public sealed class HandOffTargetsClient
{
    private readonly HttpPipeline _pipeline;

    internal HandOffTargetsClient(HttpPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    public Task<IReadOnlyList<HandOffTarget>> ListAsync(CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<IReadOnlyList<HandOffTarget>>(HttpMethod.Get, "/hand-off-targets", null, cancellationToken);

    public Task UpsertAsync(UpsertHandOffTargetRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Put, "/hand-off-targets", request, cancellationToken);

    public Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var qs = QueryBuilder.Build(new Dictionary<string, string?> { ["id"] = id });
        return _pipeline.SendAsync(HttpMethod.Delete, $"/hand-off-targets{qs}", null, cancellationToken);
    }

    public Task<GetDefaultHandOffTargetResponse> GetDefaultAsync(string channel, CancellationToken cancellationToken = default)
    {
        var qs = QueryBuilder.Build(new Dictionary<string, string?> { ["channel"] = channel });
        return _pipeline.SendAsync<GetDefaultHandOffTargetResponse>(HttpMethod.Get, $"/hand-off-targets/default{qs}", null, cancellationToken);
    }

    public Task SetDefaultAsync(SetDefaultHandOffTargetRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Put, "/hand-off-targets/default", request, cancellationToken);
}
