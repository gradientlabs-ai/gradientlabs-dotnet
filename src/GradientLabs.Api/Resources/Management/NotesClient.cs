using GradientLabs.Api.Http;

namespace GradientLabs.Api;

public sealed class NotesClient
{
    private readonly HttpPipeline _pipeline;

    internal NotesClient(HttpPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    public Task<Note> CreateAsync(CreateNoteRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<Note>(HttpMethod.Post, "/notes", request, cancellationToken);

    public Task<Note> UpdateAsync(string noteId, UpdateNoteRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<Note>(HttpMethod.Post, $"/notes/{Uri.EscapeDataString(noteId)}", request, cancellationToken);

    public Task DeleteAsync(string noteId, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Delete, $"/notes/{Uri.EscapeDataString(noteId)}", null, cancellationToken);

    public Task<Note> SetStatusAsync(string noteId, SetNoteStatusRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<Note>(HttpMethod.Post, $"/notes/{Uri.EscapeDataString(noteId)}/status", request, cancellationToken);
}
