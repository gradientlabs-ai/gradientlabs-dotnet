using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class ConversationFinishedEvent : WebhookEvent
{
    public WebhookConversation Conversation { get; init; } = new();

    [JsonPropertyName("reason_code")]
    public string? Reason { get; init; }

    public string? Intent { get; init; }
}
