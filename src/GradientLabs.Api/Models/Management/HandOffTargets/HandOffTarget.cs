using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class HandOffTarget
{
    public string Name { get; init; } = string.Empty;
    public string Id { get; init; } = string.Empty;

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class GetDefaultHandOffTargetResponse
{
    public string Id { get; init; } = string.Empty;

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class UpsertHandOffTargetRequest
{
    public string Name { get; init; } = string.Empty;
    public string Id { get; init; } = string.Empty;
}

public sealed class SetDefaultHandOffTargetRequest
{
    public string Id { get; init; } = string.Empty;
    public string Channel { get; init; } = string.Empty;
}
