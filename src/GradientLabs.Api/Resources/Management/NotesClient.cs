using GradientLabs.Api.Http;

namespace GradientLabs.Api;

public sealed class NotesClient
{
    private readonly HttpPipeline _pipeline;

    internal NotesClient(HttpPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    public Task<Note> CreateAsync(string supportPlatform, CreateNoteRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<Note>(HttpMethod.Post, $"/support-platforms/{Uri.EscapeDataString(supportPlatform)}/notes", request, cancellationToken);

    public Task<Note> UpdateAsync(string supportPlatform, string noteId, UpdateNoteRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<Note>(HttpMethod.Patch, $"/support-platforms/{Uri.EscapeDataString(supportPlatform)}/notes/{Uri.EscapeDataString(noteId)}", request, cancellationToken);

    public Task DeleteAsync(string supportPlatform, string noteId, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Delete, $"/support-platforms/{Uri.EscapeDataString(supportPlatform)}/notes/{Uri.EscapeDataString(noteId)}", null, cancellationToken);

    public Task<Note> SetStatusAsync(string supportPlatform, string noteId, SetNoteStatusRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<Note>(HttpMethod.Patch, $"/support-platforms/{Uri.EscapeDataString(supportPlatform)}/notes/{Uri.EscapeDataString(noteId)}/status", request, cancellationToken);
}
