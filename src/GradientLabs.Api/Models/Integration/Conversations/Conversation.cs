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
    /// <summary>Returned when reading a conversation that has a known intent.</summary>
    public string? LatestIntent { get; init; }
    /// <summary>Returned when reading a conversation with a hand-off target.</summary>
    public string? LatestHandoffTarget { get; init; }
    /// <summary>Empty when starting a conversation. Read the conversation for intent and hand-off details.</summary>
    public AgentMetadata? LatestAgentMetadata { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}
