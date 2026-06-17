using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class BackOfficeTask
{
    public string Id { get; init; } = string.Empty;
    public string AgentId { get; init; } = string.Empty;
    public JsonElement Input { get; init; }
    public IReadOnlyList<PublicApiAttachment>? Attachments { get; init; }
    public IReadOnlyDictionary<string, string>? Metadata { get; init; }
    public BackOfficeTaskResult? Result { get; init; }
    public BackOfficeTaskStatus? Status { get; init; }
    public DateTimeOffset Created { get; init; }
    public DateTimeOffset? Completed { get; init; }
    public DateTimeOffset? Failed { get; init; }
    public DateTimeOffset? HandedOff { get; init; }
    public IReadOnlyList<string>? FailureReasons { get; init; }
    public string? HandOffReason { get; init; }
    public DateTimeOffset? Updated { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class PublicApiAttachment
{
    public string IdempotencyKey { get; init; } = string.Empty;
    public string FileName { get; init; } = string.Empty;
    public string? ExternalUrl { get; init; }
    public byte[]? RawContents { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class BackOfficeTaskResult
{
    public string ResultType { get; init; } = string.Empty;
    public JsonElement? Custom { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}
