using System.Text.Json;

namespace GradientLabs.Api;

public sealed class ActionExecuteEvent : WebhookEvent
{
    public string Action { get; init; } = string.Empty;
    public JsonElement Params { get; init; }

    /// <summary>The conversation the action runs in. Null when it runs for a back-office task.</summary>
    public WebhookConversation? Conversation { get; init; }

    /// <summary>The back-office task the action runs for. Null when it runs in a conversation.</summary>
    public WebhookBackOfficeTaskReference? BackOfficeTask { get; init; }
}
