using GradientLabs.Api.Http;

namespace GradientLabs.Api;

public sealed class GradientLabsManagementClient : IDisposable
{
    private readonly HttpPipeline _pipeline;
    private readonly GradientLabsWebhookVerifier? _webhookVerifier;

    public ConversationsClient Conversations { get; }
    public BackOfficeTasksClient BackOfficeTasks { get; }
    public ArticlesClient Articles { get; }
    public ToolsClient Tools { get; }
    public ProceduresClient Procedures { get; }
    public ResourceTypesClient ResourceTypes { get; }
    public ResourceSourcesClient ResourceSources { get; }
    public HandOffTargetsClient HandOffTargets { get; }
    public NotesClient Notes { get; }
    public TrafficGroupsClient TrafficGroups { get; }
    public SecretsClient Secrets { get; }
    public TopicsClient Topics { get; }
    public TerminologySubstitutionsClient TerminologySubstitutions { get; }
    public IpAddressesClient IpAddresses { get; }

    public GradientLabsManagementClient(GradientLabsClientOptions options)
    {
        if (string.IsNullOrEmpty(options.ApiKey))
            throw new GradientLabsException("ApiKey must not be null or empty.");

        _pipeline = new HttpPipeline(options);
        Conversations = new ConversationsClient(_pipeline);
        BackOfficeTasks = new BackOfficeTasksClient(_pipeline);
        Articles = new ArticlesClient(_pipeline);
        Tools = new ToolsClient(_pipeline);
        Procedures = new ProceduresClient(_pipeline);
        ResourceTypes = new ResourceTypesClient(_pipeline);
        ResourceSources = new ResourceSourcesClient(_pipeline);
        HandOffTargets = new HandOffTargetsClient(_pipeline);
        Notes = new NotesClient(_pipeline);
        TrafficGroups = new TrafficGroupsClient(_pipeline);
        Secrets = new SecretsClient(_pipeline);
        Topics = new TopicsClient(_pipeline);
        TerminologySubstitutions = new TerminologySubstitutionsClient(_pipeline);
        IpAddresses = new IpAddressesClient(_pipeline);

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
