using System.Text.Json;

namespace GradientLabs.Api;

public sealed class BackOfficeTaskCompleteEvent : WebhookEvent
{
    public WebhookConversation Conversation { get; init; } = new();
    public JsonElement Data { get; init; }
}
