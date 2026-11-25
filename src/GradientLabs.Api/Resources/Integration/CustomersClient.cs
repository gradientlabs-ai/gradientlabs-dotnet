using GradientLabs.Api.Http;

namespace GradientLabs.Api;

public sealed class CustomersClient
{
    private readonly HttpPipeline _pipeline;

    internal CustomersClient(HttpPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    public async Task<IReadOnlyList<Memory>> ListMemoriesAsync(string customerId, CancellationToken cancellationToken = default)
    {
        var response = await _pipeline.SendAsync<ListMemoriesResponse>(HttpMethod.Get, $"/customers/{Uri.EscapeDataString(customerId)}/memories", null, cancellationToken).ConfigureAwait(false);
        return response.Memories;
    }

    /// <summary>
    /// Creates memories for a customer that the AI agent can search on demand. The call is asynchronous and
    /// returns no body; it fails with <c>409 Conflict</c> while another batch is being created for the same customer.
    /// </summary>
    public Task BatchCreateMemoriesAsync(string customerId, BatchCreateMemoriesRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Post, $"/customers/{Uri.EscapeDataString(customerId)}/memories/v2", request, cancellationToken);

    [Obsolete("Use BatchCreateMemoriesAsync instead.")]
    public Task BatchCreateMemoriesDeprecatedAsync(string customerId, BatchCreateMemoriesDeprecatedRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Post, $"/customers/{Uri.EscapeDataString(customerId)}/memories", request, cancellationToken);
}
