using System.Text.Json;

namespace GradientLabs.Api;

public sealed class BackOfficeTaskFailEvent : WebhookEvent
{
    public BackOfficeTask BackOfficeTask { get; init; } = new();

    [Obsolete("Never set: back-office task events carry no conversation. Use BackOfficeTask instead.")]
    public WebhookConversation Conversation { get; init; } = new();

    [Obsolete("Never set. Use BackOfficeTask instead.")]
    public JsonElement Data { get; init; }
}
