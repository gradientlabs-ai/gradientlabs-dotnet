using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class AgentMetadata
{
    public string? Intent { get; init; }
    public string? IntentHandoffTarget { get; init; }
    public string? HandoffReason { get; init; }
    public string? HandoffNote { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}
