using GradientLabs.Api.Http;

namespace GradientLabs.Api;

public sealed class ProceduresClient
{
    private readonly HttpPipeline _pipeline;

    internal ProceduresClient(HttpPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    public async Task<Page<Procedure>> ListAsync(string? cursor = null, ProcedureStatus? status = null, CancellationToken cancellationToken = default)
    {
        var qs = QueryBuilder.Build(new Dictionary<string, string?>
        {
            ["cursor"] = cursor,
            ["status"] = status?.Value,
        });
        var response = await _pipeline.SendAsync<ListProceduresResponse>(HttpMethod.Get, $"/procedures{qs}", null, cancellationToken).ConfigureAwait(false);
        return (response.Pagination ?? new PaginationCursors()).ToPage(response.Procedures);
    }

    public Task<Procedure> ReadAsync(string procedureId, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<Procedure>(HttpMethod.Get, $"/procedure/{Uri.EscapeDataString(procedureId)}", null, cancellationToken);

    public async Task<Procedure> SetLimitAsync(string procedureId, SetProcedureLimitRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _pipeline.SendAsync<SetProcedureLimitResponse>(HttpMethod.Post, $"/procedure/{Uri.EscapeDataString(procedureId)}/limit", request, cancellationToken).ConfigureAwait(false);
        return response.Procedure;
    }

    public async Task<IReadOnlyList<ProcedureVersion>> ListVersionsAsync(string procedureId, CancellationToken cancellationToken = default)
    {
        var response = await _pipeline.SendAsync<ListProcedureVersionsResponse>(HttpMethod.Get, $"/procedures/{Uri.EscapeDataString(procedureId)}/versions", null, cancellationToken).ConfigureAwait(false);
        return response.Versions;
    }

    public Task SetLiveAsync(string procedureId, long version, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Post, $"/procedures/{Uri.EscapeDataString(procedureId)}/versions/{version}/set-live", null, cancellationToken);

    public Task UnsetLiveAsync(string procedureId, long version, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Post, $"/procedures/{Uri.EscapeDataString(procedureId)}/versions/{version}/unset-live", null, cancellationToken);

    public Task SetGatedAsync(string procedureId, long version, SetGatedVersionRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Post, $"/procedures/{Uri.EscapeDataString(procedureId)}/versions/{version}/set-gated", request, cancellationToken);

    public Task UnsetGatedAsync(string procedureId, long version, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Post, $"/procedures/{Uri.EscapeDataString(procedureId)}/versions/{version}/unset-gated", null, cancellationToken);
}
