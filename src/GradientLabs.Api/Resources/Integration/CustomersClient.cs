using GradientLabs.Api.Http;

namespace GradientLabs.Api;

public sealed class CustomersClient
{
    private readonly HttpPipeline _pipeline;

    internal CustomersClient(HttpPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    public Task BatchCreateMemoriesAsync(string customerId, BatchCreateMemoriesRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Post, $"/customers/{Uri.EscapeDataString(customerId)}/memories", request, cancellationToken);
}
