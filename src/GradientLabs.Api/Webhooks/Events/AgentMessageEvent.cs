namespace GradientLabs.Api;

public sealed class AgentMessageEvent : WebhookEvent
{
    public string Body { get; init; } = string.Empty;
    public int Total { get; init; }
    public int Sequence { get; init; }
    public string? Intent { get; init; }
    public bool IsHolding { get; init; }
    public WebhookConversation Conversation { get; init; } = new();
}
