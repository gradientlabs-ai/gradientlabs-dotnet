using GradientLabs.Api.Http;

namespace GradientLabs.Api;

public sealed class GradientLabsIntegrationClient : IDisposable
{
    private readonly HttpPipeline _pipeline;
    private readonly GradientLabsWebhookVerifier? _webhookVerifier;

    public ConversationsClient Conversations { get; }
    public BackOfficeTasksClient BackOfficeTasks { get; }
    public OutboundClient Outbound { get; }
    public VoiceClient Voice { get; }

    public GradientLabsIntegrationClient(GradientLabsClientOptions options)
    {
        if (string.IsNullOrEmpty(options.ApiKey))
            throw new GradientLabsException("ApiKey must not be null or empty.");

        _pipeline = new HttpPipeline(options);
        Conversations = new ConversationsClient(_pipeline);
        BackOfficeTasks = new BackOfficeTasksClient(_pipeline);
        Outbound = new OutboundClient(_pipeline);
        Voice = new VoiceClient(_pipeline);

        if (!string.IsNullOrEmpty(options.WebhookSigningKey))
            _webhookVerifier = new GradientLabsWebhookVerifier(options.WebhookSigningKey, options);
    }

    public WebhookEvent ParseWebhook(ReadOnlySpan<byte> rawBody, string signatureHeader, string? tokenHeader = null)
    {
        if (_webhookVerifier is null)
            throw new GradientLabsWebhookException("WebhookSigningKey must be set in GradientLabsClientOptions to parse webhooks.");
        return _webhookVerifier.Parse(rawBody, signatureHeader, tokenHeader);
    }

    public void Dispose() => _pipeline.Dispose();
}
