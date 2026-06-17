using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class Conversation
{
    public string Id { get; init; } = string.Empty;
    public string CustomerId { get; init; } = string.Empty;
    public DateTimeOffset Created { get; init; }
    public DateTimeOffset Updated { get; init; }
    public string Status { get; init; } = string.Empty;
    public bool AgentIsActive { get; init; }
    public ConversationChannel Channel { get; init; }
    public string? LatestIntent { get; init; }
    public string? LatestHandoffTarget { get; init; }
    public AgentMetadata? LatestAgentMetadata { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}
