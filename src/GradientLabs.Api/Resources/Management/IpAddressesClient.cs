using GradientLabs.Api.Http;

namespace GradientLabs.Api;

public sealed class IpAddressesClient
{
    private readonly HttpPipeline _pipeline;

    internal IpAddressesClient(HttpPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    public Task<IpAddressesResponse> ListAsync(CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<IpAddressesResponse>(HttpMethod.Get, "/ip-addresses", null, cancellationToken);
}
