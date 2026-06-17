using System.Text.Json;

namespace GradientLabs.Api;

public sealed class ActionExecuteEvent : WebhookEvent
{
    public string Action { get; init; } = string.Empty;
    public JsonElement Params { get; init; }
    public WebhookConversation Conversation { get; init; } = new();
}
