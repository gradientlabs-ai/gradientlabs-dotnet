using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class Procedure
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public ProcedureStatus Status { get; init; }
    public UserDetails? Author { get; init; }
    public DateTimeOffset Created { get; init; }
    public DateTimeOffset Updated { get; init; }
    public bool HasDailyLimit { get; init; }
    public long? MaxDailyConversations { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class UserDetails
{
    public string? Email { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class ProcedureVersion
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public long Version { get; init; }
    public UserDetails? Author { get; init; }
    public DateTimeOffset Created { get; init; }
    public bool Gated { get; init; }
    public GatedConfig? GatedConfig { get; init; }
    public bool Live { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class GatedConfig
{
    public long MaxDailyConversations { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}
