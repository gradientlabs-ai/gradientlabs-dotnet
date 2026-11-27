using GradientLabs.Api.Http;

namespace GradientLabs.Api;

public sealed class SecretsClient
{
    private readonly HttpPipeline _pipeline;

    internal SecretsClient(HttpPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    public async Task<IReadOnlyList<Secret>> ListAsync(CancellationToken cancellationToken = default)
    {
        var response = await _pipeline.SendAsync<ListSecretsResponse>(HttpMethod.Get, "/secrets", null, cancellationToken).ConfigureAwait(false);
        return response.Secrets;
    }

    public Task<Secret> WriteAsync(string name, WriteSecretRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<Secret>(HttpMethod.Put, $"/secrets/{Uri.EscapeDataString(name)}", request, cancellationToken);

    public Task RevokeAsync(string name, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Delete, $"/secrets/{Uri.EscapeDataString(name)}", null, cancellationToken);
}
