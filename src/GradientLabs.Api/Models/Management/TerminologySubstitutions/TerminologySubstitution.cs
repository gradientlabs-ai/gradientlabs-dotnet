using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class TerminologySubstitution
{
    public string Id { get; init; } = string.Empty;
    public string Blocked { get; init; } = string.Empty;
    public string? BlockedDescription { get; init; }
    public string? Replacement { get; init; }
    public DateTimeOffset Created { get; init; }
    public DateTimeOffset Updated { get; init; }
    public string? ResourceTypeId { get; init; }
    public string? ResourceAttributeJsonPath { get; init; }
    public string? ResourceValueToMatch { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class CreateTerminologySubstitutionRequest
{
    public string Blocked { get; init; } = string.Empty;
    public string? BlockedDescription { get; init; }
    public string? Replacement { get; init; }
    public string? ResourceTypeId { get; init; }
    public string? ResourceAttributeJsonPath { get; init; }
    public string? ResourceValueToMatch { get; init; }
}

public sealed class UpdateTerminologySubstitutionRequest
{
    public string? Blocked { get; init; }
    public string? BlockedDescription { get; init; }
    public string? Replacement { get; init; }
    public string? ResourceTypeId { get; init; }
    public string? ResourceAttributeJsonPath { get; init; }
    public string? ResourceValueToMatch { get; init; }
}
