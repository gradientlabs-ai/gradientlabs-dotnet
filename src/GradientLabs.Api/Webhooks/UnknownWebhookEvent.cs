using System.Text.Json;

namespace GradientLabs.Api;

public sealed class UnknownWebhookEvent : WebhookEvent
{
    public JsonElement Data { get; set; }
}
