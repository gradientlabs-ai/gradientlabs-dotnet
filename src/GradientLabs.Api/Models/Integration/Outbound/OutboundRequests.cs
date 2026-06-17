using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class StartOutboundConversationRequest
{
    public string CustomerId { get; init; } = string.Empty;
    public CustomerSource CustomerSource { get; init; }
    public string ProcedureId { get; init; } = string.Empty;
    public string? Body { get; init; }
    public ConversationChannel? Channel { get; init; }
    public string? Subject { get; init; }
    public Dictionary<string, JsonElement>? Resources { get; init; }
    public SupportPlatform? SupportPlatform { get; init; }
}

public sealed class StartOutboundConversationResponse
{
    public string ConversationId { get; init; } = string.Empty;

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}
