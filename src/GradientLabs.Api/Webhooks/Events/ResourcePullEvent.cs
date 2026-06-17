namespace GradientLabs.Api;

public sealed class ResourcePullEvent : WebhookEvent
{
    public string ResourceType { get; init; } = string.Empty;
    public WebhookConversation Conversation { get; init; } = new();
}
