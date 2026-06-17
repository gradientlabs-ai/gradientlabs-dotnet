using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class Tool
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

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class ToolParameter
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public ParameterType Type { get; init; }
    public IReadOnlyList<ParameterSource> AllowedSources { get; init; } = [];
    public bool? Required { get; init; }
    public IReadOnlyList<ParameterOption>? Options { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class ParameterOption
{
    public string Value { get; init; } = string.Empty;
    public string? Text { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class AsyncDefinition
{
    public ChildTool StartExecutionTool { get; init; } = new();
    public ChildTool? CancelExecutionTool { get; init; }
    public TimeSpan Timeout { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class HttpDefinition
{
    public string Method { get; init; } = string.Empty;
    public string UrlTemplate { get; init; } = string.Empty;
    public HttpBodyDefinition? Body { get; init; }
    public IReadOnlyDictionary<string, string>? HeaderTemplates { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class HttpBodyDefinition
{
    public BodyEncoding Encoding { get; init; }
    public string? JsonTemplate { get; init; }
    public IReadOnlyDictionary<string, string>? FormFieldTemplates { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class ToolWebhookConfig
{
    public string Name { get; init; } = string.Empty;

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class McpConfiguration
{
    public string ServerId { get; init; } = string.Empty;
    public string ExternalToolName { get; init; } = string.Empty;

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class ChildTool
{
    public HttpDefinition? Http { get; init; }
    public ToolWebhookConfig? Webhook { get; init; }
    public WorkflowConfiguration? Workflow { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class WorkflowConfiguration
{
    public string WorkflowType { get; init; } = string.Empty;
    public string TaskQueue { get; init; } = string.Empty;

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class ToolParameterSet
{
    public string DiscriminatorParameterName { get; init; } = string.Empty;
    public string DiscriminatorValue { get; init; } = string.Empty;
    public IReadOnlyList<ToolParameter> Parameters { get; init; } = [];

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}
