using System.Text.Json;

namespace GradientLabs.Api;

public sealed class CreateToolRequest
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public IReadOnlyList<ToolParameter> Parameters { get; init; } = [];
    public AsyncDefinition? Async { get; init; }
    public HttpDefinition? Http { get; init; }
    public ToolWebhookConfig? Webhook { get; init; }
    public bool? Draft { get; init; }
    public IReadOnlyList<ToolParameterSet>? ParameterSets { get; init; }

    /// <summary>The ID of a branch to create the tool on. Send this or <see cref="BranchName"/>.</summary>
    public string? BranchId { get; init; }

    /// <summary>The git branch name of the branch to create on, as an alternative to <see cref="BranchId"/>.</summary>
    public string? BranchName { get; init; }
}

public sealed class UpdateToolRequest
{
    public string? Description { get; init; }
    public IReadOnlyList<ToolParameter>? Parameters { get; init; }
    public AsyncDefinition? Async { get; init; }
    public HttpDefinition? Http { get; init; }
    public ToolWebhookConfig? Webhook { get; init; }
    public IReadOnlyList<ToolParameterSet>? ParameterSets { get; init; }
}

public sealed class ExecuteToolRequest
{
    public IReadOnlyList<ToolArgument> Arguments { get; init; } = [];

    /// <summary>An optional conversation-scoped token, needed when the tool sends one in its authorization header.</summary>
    public string? Token { get; init; }
}

public sealed class ToolArgument
{
    public string Name { get; init; } = string.Empty;
    public string? Value { get; init; }
}

public sealed class ExecuteToolResponse
{
    public string Id { get; init; } = string.Empty;
    public JsonElement Result { get; init; }
    public string? Error { get; init; }
}
