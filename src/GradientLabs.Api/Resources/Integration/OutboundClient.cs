using GradientLabs.Api.Http;

namespace GradientLabs.Api;

public sealed class OutboundClient
{
    private readonly HttpPipeline _pipeline;

    internal OutboundClient(HttpPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    public Task<StartOutboundConversationResponse> StartConversationAsync(StartOutboundConversationRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<StartOutboundConversationResponse>(HttpMethod.Post, "/outbound/conversations", request, cancellationToken);
}
