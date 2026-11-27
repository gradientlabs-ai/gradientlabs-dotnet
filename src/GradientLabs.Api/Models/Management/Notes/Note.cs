using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class Note
{
    public string GradientLabsId { get; init; } = string.Empty;
    public string Id { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string? Body { get; init; }
    public string? Url { get; init; }
    public DateTimeOffset? ValidFrom { get; init; }
    public DateTimeOffset? ValidTo { get; init; }
    public string? LastModifiedBy { get; init; }
    public DateTimeOffset Created { get; init; }
    public DateTimeOffset Updated { get; init; }
    public NoteStatus Status { get; init; }

    /// <summary>Which customers the note applies to, as a map from attribute key to the attribute values it applies to.</summary>
    public IReadOnlyDictionary<string, IReadOnlyList<string>>? Attributes { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class CreateNoteRequest
{
    public string Id { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;

    /// <summary>The main contents of the note. Send exactly one of <see cref="Body"/> and <see cref="WebpageUrl"/>.</summary>
    public string? Body { get; init; }

    public string? WebpageUrl { get; init; }
    public DateTimeOffset? StartTime { get; init; }
    public DateTimeOffset? EndTime { get; init; }

    /// <summary>Which customers the note applies to. Leave it null for a note that applies to every conversation.</summary>
    public IReadOnlyDictionary<string, IReadOnlyList<string>>? Attributes { get; init; }

    /// <summary>The knowledge bases the note belongs to. Leave it null to use the default <c>public-api</c> knowledge base.</summary>
    public IReadOnlyList<string>? KnowledgeBaseIds { get; init; }
}

public sealed class UpdateNoteRequest
{
    public string? Title { get; init; }
    public string? Body { get; init; }
    public string? WebpageUrl { get; init; }
    public DateTimeOffset? StartTime { get; init; }
    public DateTimeOffset? EndTime { get; init; }
    public IReadOnlyDictionary<string, IReadOnlyList<string>>? Attributes { get; init; }
    public IReadOnlyList<string>? KnowledgeBaseIds { get; init; }
}

public sealed class SetNoteStatusRequest
{
    public NoteStatus Status { get; init; }
}

internal sealed class ListNotesResponse
{
    public IReadOnlyList<Note> Notes { get; init; } = [];
    public PaginationCursors? Pagination { get; init; }
}
