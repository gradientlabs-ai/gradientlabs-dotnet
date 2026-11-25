using GradientLabs.Api.Http;

namespace GradientLabs.Api;

public sealed class NotesClient
{
    private readonly HttpPipeline _pipeline;

    internal NotesClient(HttpPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    public async Task<Page<Note>> ListAsync(string? cursor = null, CancellationToken cancellationToken = default)
    {
        var qs = QueryBuilder.Build(new Dictionary<string, string?> { ["cursor"] = cursor });
        var response = await _pipeline.SendAsync<ListNotesResponse>(HttpMethod.Get, $"/notes{qs}", null, cancellationToken).ConfigureAwait(false);
        return (response.Pagination ?? new PaginationCursors()).ToPage(response.Notes);
    }

    public Task<Note> ReadAsync(string noteId, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<Note>(HttpMethod.Get, $"/notes/{Uri.EscapeDataString(noteId)}", null, cancellationToken);

    public Task<Note> CreateAsync(CreateNoteRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<Note>(HttpMethod.Post, "/notes", request, cancellationToken);

    public Task<Note> UpdateAsync(string noteId, UpdateNoteRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<Note>(HttpMethod.Post, $"/notes/{Uri.EscapeDataString(noteId)}", request, cancellationToken);

    public Task DeleteAsync(string noteId, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Delete, $"/notes/{Uri.EscapeDataString(noteId)}", null, cancellationToken);

    public Task SetStatusAsync(string noteId, SetNoteStatusRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Post, $"/notes/{Uri.EscapeDataString(noteId)}/status", request, cancellationToken);
}
