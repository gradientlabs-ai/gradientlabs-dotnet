using GradientLabs.Api.Http;

namespace GradientLabs.Api;

public sealed class ConversationsClient
{
    private readonly HttpPipeline _pipeline;

    internal ConversationsClient(HttpPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    public Task<Conversation> StartAsync(StartConversationRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<Conversation>(HttpMethod.Post, "/conversations", request, cancellationToken);

    public Task CancelAsync(string conversationId, CancelConversationRequest? request = null, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Put, $"/conversations/{Uri.EscapeDataString(conversationId)}/cancel", request, cancellationToken);

    public Task FinishAsync(string conversationId, FinishConversationRequest? request = null, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Put, $"/conversations/{Uri.EscapeDataString(conversationId)}/finish", request, cancellationToken);

    public Task AddMessageAsync(string conversationId, AddMessageRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Post, $"/conversations/{Uri.EscapeDataString(conversationId)}/messages", request, cancellationToken);

    public Task ResumeAsync(string conversationId, ResumeConversationRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Put, $"/conversations/{Uri.EscapeDataString(conversationId)}/resume", request, cancellationToken);

    public Task ReturnAsyncToolResultAsync(string conversationId, ReturnAsyncToolResultRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Put, $"/conversations/{Uri.EscapeDataString(conversationId)}/return-async-tool-result", request, cancellationToken);

    public Task AssignAsync(string conversationId, AssignConversationRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Put, $"/conversations/{Uri.EscapeDataString(conversationId)}/assignee", request, cancellationToken);

    public Task AddEventAsync(string conversationId, AddEventRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Post, $"/conversations/{Uri.EscapeDataString(conversationId)}/events", request, cancellationToken);

    public Task RateAsync(string conversationId, RateConversationRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Put, $"/conversations/{Uri.EscapeDataString(conversationId)}/rate", request, cancellationToken);

    public Task<Conversation> ReadAsync(string conversationId, CancellationToken cancellationToken = default)
        => ReadAsync(conversationId, null, cancellationToken);

    /// <param name="conversationId">Your own identifier for the conversation: the ID you gave it when you started it.</param>
    /// <param name="supportPlatform">Which platform's conversation to read: <c>public-api</c> or <c>intercom</c>. Defaults to <c>public-api</c> when null.</param>
    /// <param name="cancellationToken">Cancels the request.</param>
    public Task<Conversation> ReadAsync(string conversationId, string? supportPlatform, CancellationToken cancellationToken = default)
    {
        var qs = QueryBuilder.Build(new Dictionary<string, string?> { ["support_platform"] = supportPlatform });
        return _pipeline.SendAsync<Conversation>(HttpMethod.Get, $"/conversations/{Uri.EscapeDataString(conversationId)}/read{qs}", null, cancellationToken);
    }

    public Task DeleteAsync(string conversationId, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Delete, $"/conversations/{Uri.EscapeDataString(conversationId)}", null, cancellationToken);

    [Obsolete("Use ReadAsync instead.")]
    public Task<Conversation> ReadDeprecatedAsync(string conversationId, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<Conversation>(HttpMethod.Get, $"/conversations/{Uri.EscapeDataString(conversationId)}", null, cancellationToken);
}
