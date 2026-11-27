using System.Text.Json;

namespace GradientLabs.Api;

public sealed class WebhookConversation
{
    public string Id { get; init; } = string.Empty;
    public string CustomerId { get; init; } = string.Empty;

    /// <summary>The platform the customer came from. Set on <c>action.execute</c> events.</summary>
    public string? CustomerSource { get; init; }

    /// <summary>The conversation's metadata. Set on <c>action.execute</c> events.</summary>
    public JsonElement? Metadata { get; init; }
}

public sealed class WebhookBackOfficeTaskReference
{
    public string Id { get; init; } = string.Empty;
}
