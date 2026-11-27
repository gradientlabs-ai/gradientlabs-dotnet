using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class VoiceCallContext
{
    public DateTimeOffset StartedAt { get; init; }
    public string? Summary { get; init; }
    public string? Transcript { get; init; }
    public string? HandoffReason { get; init; }
    public string? LastExecutedProcedure { get; init; }
    public string? LastExecutedProcedureUrl { get; init; }
    public string? GradientLabsUrl { get; init; }

    /// <summary>A link a human agent opens to take over the call. Only set when the call is being handed off live.</summary>
    public string? LiveHandoffUrl { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}
