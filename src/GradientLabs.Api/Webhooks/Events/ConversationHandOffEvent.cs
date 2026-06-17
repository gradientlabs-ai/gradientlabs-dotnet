using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class ConversationHandOffEvent : WebhookEvent
{
    public WebhookConversation Conversation { get; init; } = new();
    public string? Target { get; init; }

    [JsonPropertyName("reason_code")]
    public string Reason { get; init; } = string.Empty;

    [JsonPropertyName("reason")]
    public string Description { get; init; } = string.Empty;

    public string? Note { get; init; }
    public string? Intent { get; init; }
}
