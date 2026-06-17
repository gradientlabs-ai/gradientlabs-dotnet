using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class Attachment
{
    public AttachmentType Type { get; init; }
    public string FileName { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
    public string? Summary { get; init; }
    public string? Description { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}
