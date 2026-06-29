namespace GradientLabs.Api;

public sealed class CreateBackOfficeTaskRequest
{
    public string Id { get; init; } = string.Empty;

    /// <summary>Identifies the agent (<c>agent_…</c>) that owns the procedure to run the task against.</summary>
    public string? AgentId { get; init; }

    /// <summary>Identifies the procedure (<c>proc_…</c>) within the agent to start the task from.</summary>
    public string? ProcedureId { get; init; }

    public object Input { get; init; } = new();
    public IReadOnlyList<BackOfficeTaskAttachment>? Attachments { get; init; }
    public DateTimeOffset? Created { get; init; }
    public IReadOnlyDictionary<string, string>? Metadata { get; init; }
}

public sealed class BackOfficeTaskAttachment
{
    public string FileName { get; init; } = string.Empty;
    public string? Url { get; init; }
    public string? Base64Contents { get; init; }
}
