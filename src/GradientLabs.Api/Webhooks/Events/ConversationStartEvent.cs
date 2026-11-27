namespace GradientLabs.Api;

/// <summary>
/// Asks you to open an outbound conversation on your own platform. Reply with a
/// <see cref="ConversationStartWebhookResponse"/> naming the conversation you opened.
/// </summary>
public sealed class ConversationStartEvent : WebhookEvent
{
    /// <summary>Your identifier for the customer to contact.</summary>
    public string CustomerId { get; init; } = string.Empty;

    public ConversationChannel Channel { get; init; }

    /// <summary>The first message to send to the customer.</summary>
    public string Body { get; init; } = string.Empty;

    /// <summary>The subject line, for email conversations. Empty on other channels.</summary>
    public string Subject { get; init; } = string.Empty;

    /// <summary>
    /// Who started the outbound conversation: <c>public-api</c> when you started it through the API, otherwise the
    /// Gradient Labs ID of the conversation or back-office task the AI agent started it from.
    /// </summary>
    public string InitiatorId { get; init; } = string.Empty;
}

/// <summary>The body to reply with to a <see cref="ConversationStartEvent"/>.</summary>
public sealed class ConversationStartWebhookResponse
{
    /// <summary>Your identifier for the conversation you opened. Use it in later API calls about this conversation.</summary>
    public string ConversationId { get; init; } = string.Empty;

    public string ToJson() => System.Text.Json.JsonSerializer.Serialize(this, Serialization.GradientLabsJsonOptions.Default);
}
