using GradientLabs.Api.Http;

namespace GradientLabs.Api;

public sealed class FilesClient
{
    private readonly HttpPipeline _pipeline;

    internal FilesClient(HttpPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    /// <summary>
    /// Starts a file upload. Send the file to <see cref="FileUpload.UploadUrl"/> as an HTTP <c>PUT</c> with every header
    /// in <see cref="FileUpload.Headers"/>, then call <see cref="CreateAsync"/> with the upload ID.
    /// </summary>
    public Task<FileUpload> BeginUploadAsync(BeginFileUploadRequest? request = null, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<FileUpload>(HttpMethod.Post, "/file-uploads", request, cancellationToken);

    public Task<UploadedFile> CreateAsync(CreateFileRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<UploadedFile>(HttpMethod.Post, "/files", request, cancellationToken);

    public async Task<Page<UploadedFile>> ListAsync(string? cursor = null, CancellationToken cancellationToken = default)
    {
        var qs = QueryBuilder.Build(new Dictionary<string, string?> { ["cursor"] = cursor });
        var response = await _pipeline.SendAsync<ListFilesResponse>(HttpMethod.Get, $"/files{qs}", null, cancellationToken).ConfigureAwait(false);
        return (response.Pagination ?? new PaginationCursors()).ToPage(response.Files);
    }
}
