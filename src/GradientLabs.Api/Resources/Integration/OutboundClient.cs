using GradientLabs.Api.Http;

namespace GradientLabs.Api;

public sealed class OutboundClient
{
    private readonly HttpPipeline _pipeline;

    internal OutboundClient(HttpPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    /// <summary>
    /// Starts an outbound live chat conversation in which the AI agent proactively contacts a customer,
    /// following the specified outbound procedure. The customer is created, or matched to an existing
    /// record, from the customer ID and any support platform identifiers supplied.
    /// </summary>
    public Task<StartOutboundConversationResponse> StartChatConversationAsync(StartOutboundChatConversationRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<StartOutboundConversationResponse>(HttpMethod.Post, "/outbound/conversations/chat", request, cancellationToken);

    /// <summary>
    /// Starts an outbound email conversation in which the AI agent proactively contacts a customer,
    /// following the specified outbound procedure. The platform the email is sent from needs an
    /// identifier for the customer, so sending from Zendesk needs a Zendesk identifier, and so on.
    /// </summary>
    public Task<StartOutboundConversationResponse> StartEmailConversationAsync(StartOutboundEmailConversationRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<StartOutboundConversationResponse>(HttpMethod.Post, "/outbound/conversations/email", request, cancellationToken);

    /// <summary>
    /// Places an outbound phone call in which the AI agent proactively contacts a customer, following
    /// the specified outbound procedure. The number called from must already be provisioned for your company.
    /// </summary>
    public Task<StartOutboundConversationResponse> StartPhoneConversationAsync(StartOutboundPhoneConversationRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<StartOutboundConversationResponse>(HttpMethod.Post, "/outbound/conversations/phone", request, cancellationToken);
}
