namespace GradientLabs.Api;

public sealed class AgentMessageEvent : WebhookEvent
{
    public string Body { get; init; } = string.Empty;
    public int Total { get; init; }
    public int Sequence { get; init; }
    public string? Intent { get; init; }
    public bool IsHolding { get; init; }
    public WebhookConversation Conversation { get; init; } = new();

    /// <summary>Your identifier for the last customer message received before the agent wrote this batch of replies. Null if unknown.</summary>
    public string? LastCustomerMessageId { get; init; }
}
