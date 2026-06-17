using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class Note
{
    public string GradientLabsId { get; init; } = string.Empty;
    public string Id { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Body { get; init; } = string.Empty;
    public string? Url { get; init; }
    public DateTimeOffset? ValidFrom { get; init; }
    public DateTimeOffset? ValidTo { get; init; }
    public string? LastModifiedBy { get; init; }
    public DateTimeOffset Created { get; init; }
    public DateTimeOffset Updated { get; init; }
    public NoteStatus Status { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class CreateNoteRequest
{
    public string Id { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Body { get; init; } = string.Empty;
    public string? WebpageUrl { get; init; }
    public DateTimeOffset? StartTime { get; init; }
    public DateTimeOffset? EndTime { get; init; }
}

public sealed class UpdateNoteRequest
{
    public string? Title { get; init; }
    public string? Body { get; init; }
    public string? WebpageUrl { get; init; }
    public DateTimeOffset? StartTime { get; init; }
    public DateTimeOffset? EndTime { get; init; }
}

public sealed class SetNoteStatusRequest
{
    public NoteStatus Status { get; init; }
}
