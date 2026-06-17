namespace GradientLabs.Api;

public sealed class GradientLabsClientOptions
{
    private static readonly Uri DefaultBaseUri = new("https://api.gradient-labs.ai");

    public string ApiKey { get; init; } = string.Empty;
    public Uri BaseUri { get; init; } = DefaultBaseUri;
    public HttpClient? HttpClient { get; init; }
    public HttpMessageHandler? HttpMessageHandler { get; init; }
    public TimeSpan? Timeout { get; init; }
    public string? WebhookSigningKey { get; init; }
    public TimeSpan WebhookLeeway { get; init; } = TimeSpan.FromMinutes(5);
    public TimeProvider TimeProvider { get; init; } = TimeProvider.System;
    public string? UserAgentVersion { get; init; }
}
