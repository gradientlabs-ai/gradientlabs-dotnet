using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class Procedure
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public ProcedureStatus Status { get; init; }
    public DateTimeOffset Created { get; init; }
    public DateTimeOffset Updated { get; init; }
    public bool HasDailyLimit { get; init; }
    public long? MaxDailyConversations { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class ProcedureVersion
{
    [JsonPropertyName("Name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("Description")]
    public string? Description { get; init; }

    [JsonPropertyName("Version")]
    public long Version { get; init; }

    [JsonPropertyName("Author")]
    public string? Author { get; init; }

    [JsonPropertyName("Created")]
    public DateTimeOffset Created { get; init; }

    [JsonPropertyName("Gated")]
    public bool Gated { get; init; }

    [JsonPropertyName("GatedConfig")]
    public GatedConfig? GatedConfig { get; init; }

    [JsonPropertyName("Live")]
    public bool Live { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class GatedConfig
{
    [JsonPropertyName("MaxDailyConversations")]
    public long MaxDailyConversations { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

internal sealed class ListProceduresResponse
{
    public IReadOnlyList<Procedure> Procedures { get; init; } = [];
    public PaginationCursors? Pagination { get; init; }
}

internal sealed class ListProcedureVersionsResponse
{
    [JsonPropertyName("Versions")]
    public IReadOnlyList<ProcedureVersion> Versions { get; init; } = [];
}

internal sealed class SetProcedureLimitResponse
{
    public Procedure Procedure { get; init; } = new();
}
