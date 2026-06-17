using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public abstract class WebhookEvent
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public GradientLabsWebhookEventType Type { get; set; }

    [JsonPropertyName("sequence_number")]
    public long SequenceNumber { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTimeOffset Timestamp { get; set; }

    public string? Token { get; set; }
}
