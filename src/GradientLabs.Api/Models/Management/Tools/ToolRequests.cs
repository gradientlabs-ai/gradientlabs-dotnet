using System.Text.Json;

namespace GradientLabs.Api;

public sealed class CreateToolRequest
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public IReadOnlyList<ToolParameter> Parameters { get; init; } = [];
    public AsyncDefinition? Async { get; init; }
    public HttpDefinition? Http { get; init; }
    public ToolWebhookConfig? Webhook { get; init; }
    public McpConfiguration? Mcp { get; init; }
    public bool? Draft { get; init; }
    public IReadOnlyList<ToolParameterSet>? ParameterSets { get; init; }
}

public sealed class UpdateToolRequest
{
    public string? Description { get; init; }
    public IReadOnlyList<ToolParameter>? Parameters { get; init; }
    public AsyncDefinition? Async { get; init; }
    public HttpDefinition? Http { get; init; }
    public ToolWebhookConfig? Webhook { get; init; }
}

public sealed class ExecuteToolRequest
{
    public IReadOnlyList<ToolArgument> Arguments { get; init; } = [];
    public string Token { get; init; } = string.Empty;
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
