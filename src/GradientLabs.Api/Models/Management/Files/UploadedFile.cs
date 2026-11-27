using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class UploadedFile
{
    public string Id { get; init; } = string.Empty;

    /// <summary>The name a back-office test case uses to list this file as an attachment, as <c>file_ref</c>.</summary>
    public string Ref { get; init; } = string.Empty;

    public string FileName { get; init; } = string.Empty;
    public string ContentType { get; init; } = string.Empty;
    public long SizeBytes { get; init; }
    public DateTimeOffset Created { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class BeginFileUploadRequest
{
    /// <summary>The <c>Content-Type</c> header the upload sends. Leave it null to send <c>application/octet-stream</c>.</summary>
    public string? ContentType { get; init; }
}

public sealed class FileUpload
{
    /// <summary>Pass this to <see cref="FilesClient.CreateAsync"/> once the file has been uploaded.</summary>
    public string UploadId { get; init; } = string.Empty;

    /// <summary>The URL to send the file to as an HTTP <c>PUT</c>, with every header in <see cref="Headers"/>.</summary>
    public string UploadUrl { get; init; } = string.Empty;

    public IReadOnlyDictionary<string, string> Headers { get; init; } = new Dictionary<string, string>();
    public long MaxBytes { get; init; }
    public string ContentType { get; init; } = string.Empty;
    public DateTimeOffset ExpiresAt { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class CreateFileRequest
{
    /// <summary>The <c>upload_id</c> that <see cref="FilesClient.BeginUploadAsync"/> returned.</summary>
    public string UploadId { get; init; } = string.Empty;

    public string FileName { get; init; } = string.Empty;
    public string? Ref { get; init; }
}

internal sealed class ListFilesResponse
{
    public IReadOnlyList<UploadedFile> Files { get; init; } = [];
    public PaginationCursors? Pagination { get; init; }
}
