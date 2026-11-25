namespace GradientLabs.Api;

public sealed class ResourcePullEvent : WebhookEvent
{
    public string ResourceType { get; init; } = string.Empty;

    /// <summary>The conversation the resource is pulled for. Null when it is pulled for a back-office task.</summary>
    public WebhookConversation? Conversation { get; init; }

    /// <summary>The back-office task the resource is pulled for. Null when it is pulled for a conversation.</summary>
    public WebhookBackOfficeTaskReference? BackOfficeTask { get; init; }
}
