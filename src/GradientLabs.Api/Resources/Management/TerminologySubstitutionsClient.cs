using GradientLabs.Api.Http;

namespace GradientLabs.Api;

public sealed class TerminologySubstitutionsClient
{
    private readonly HttpPipeline _pipeline;

    internal TerminologySubstitutionsClient(HttpPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    public Task<IReadOnlyList<TerminologySubstitution>> ListAsync(CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<IReadOnlyList<TerminologySubstitution>>(HttpMethod.Get, "/terminology-substitutions", null, cancellationToken);

    public Task<TerminologySubstitution> CreateAsync(CreateTerminologySubstitutionRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<TerminologySubstitution>(HttpMethod.Post, "/terminology-substitutions", request, cancellationToken);

    public Task<TerminologySubstitution> ReadAsync(string id, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<TerminologySubstitution>(HttpMethod.Get, $"/terminology-substitutions/{Uri.EscapeDataString(id)}", null, cancellationToken);

    public Task<TerminologySubstitution> UpdateAsync(string id, UpdateTerminologySubstitutionRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<TerminologySubstitution>(HttpMethod.Patch, $"/terminology-substitutions/{Uri.EscapeDataString(id)}", request, cancellationToken);

    public Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Delete, $"/terminology-substitutions/{Uri.EscapeDataString(id)}", null, cancellationToken);
}
