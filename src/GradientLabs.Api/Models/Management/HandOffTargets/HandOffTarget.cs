using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class HandOffTarget
{
    public string Name { get; init; } = string.Empty;
    public string Id { get; init; } = string.Empty;

    /// <summary>A short reference for this hand-off target that never changes, unique within your company.</summary>
    public string Ref { get; init; } = string.Empty;

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

    /// <summary>A short reference made of letters, digits, <c>_</c> and <c>-</c>. Leave it null to have one made from <see cref="Name"/>.</summary>
    public string? Ref { get; init; }
}

public sealed class SetDefaultHandOffTargetRequest
{
    public string Id { get; init; } = string.Empty;
    public string Channel { get; init; } = string.Empty;
}

internal sealed class ListHandOffTargetsResponse
{
    public IReadOnlyList<HandOffTarget> Targets { get; init; } = [];
}
