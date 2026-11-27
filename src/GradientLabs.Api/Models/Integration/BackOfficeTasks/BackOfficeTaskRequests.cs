using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class CreateBackOfficeTaskRequest
{
    public string Id { get; init; } = string.Empty;

    /// <summary>Identifies the agent (<c>agent_…</c>) that owns the procedure to run the task against.</summary>
    public required string AgentId { get; init; }

    /// <summary>Identifies the procedure (<c>proc_…</c>) within the agent to start the task from.</summary>
    public required string ProcedureId { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public required object? Input { get; init; }
    public IReadOnlyList<BackOfficeTaskAttachment>? Attachments { get; init; }
    public DateTimeOffset? Created { get; init; }
    public IReadOnlyDictionary<string, string>? Metadata { get; init; }
}

public sealed class BackOfficeTaskAttachment
{
    public string FileName { get; init; } = string.Empty;
    public string? Url { get; init; }
    [JsonPropertyName("base_64_contents")]
    public string? Base64Contents { get; init; }
}
