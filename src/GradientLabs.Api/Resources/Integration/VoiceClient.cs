using GradientLabs.Api.Http;

namespace GradientLabs.Api;

public sealed class VoiceClient
{
    private readonly HttpPipeline _pipeline;

    internal VoiceClient(HttpPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    public Task<VoiceCallContext> ReadLatestCallContextAsync(
        string phoneNumber,
        int? lookbackSeconds = null,
        bool? includeLargeFields = null,
        CancellationToken cancellationToken = default)
    {
        var qs = QueryBuilder.Build(new Dictionary<string, string?>
        {
            ["lookback_seconds"] = lookbackSeconds?.ToString(),
            ["include_large_fields"] = includeLargeFields?.ToString()?.ToLowerInvariant(),
        });
        return _pipeline.SendAsync<VoiceCallContext>(HttpMethod.Get, $"/voice/calls/{Uri.EscapeDataString(phoneNumber)}/context{qs}", null, cancellationToken);
    }
}
