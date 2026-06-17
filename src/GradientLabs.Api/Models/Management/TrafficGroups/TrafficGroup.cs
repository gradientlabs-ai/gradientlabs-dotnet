using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class TrafficGroup
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public IReadOnlyList<TrafficGroupTarget> Targets { get; init; } = [];
    public IReadOnlyList<TrafficGroupTarget> ExcludedTargets { get; init; } = [];

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class TrafficGroupTarget
{
    public string TargetType { get; init; } = string.Empty;
    public string TargetId { get; init; } = string.Empty;

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class CreateTrafficGroupRequest
{
    public string Name { get; init; } = string.Empty;
}

public sealed class UpdateTrafficGroupRequest
{
    public string? Name { get; init; }
}

public sealed class CreateTrafficGroupTargetRequest
{
    public string TargetType { get; init; } = string.Empty;
    public string TargetId { get; init; } = string.Empty;
}

public sealed class CreateTrafficGroupExclusionRequest
{
    public string TargetType { get; init; } = string.Empty;
    public string TargetId { get; init; } = string.Empty;
}
