using GradientLabs.Api.Http;

namespace GradientLabs.Api;

public sealed class SecretsClient
{
    private readonly HttpPipeline _pipeline;

    internal SecretsClient(HttpPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    public Task<IReadOnlyList<Secret>> ListAsync(CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<IReadOnlyList<Secret>>(HttpMethod.Get, "/secrets", null, cancellationToken);

    public Task WriteAsync(string name, WriteSecretRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Put, $"/secrets/{Uri.EscapeDataString(name)}", request, cancellationToken);

    public Task RevokeAsync(string name, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Delete, $"/secrets/{Uri.EscapeDataString(name)}", null, cancellationToken);
}
