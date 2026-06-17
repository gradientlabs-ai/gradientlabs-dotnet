using GradientLabs.Api.Http;

namespace GradientLabs.Api;

public sealed class BackOfficeTasksClient
{
    private readonly HttpPipeline _pipeline;

    internal BackOfficeTasksClient(HttpPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    public Task<BackOfficeTask> CreateAsync(CreateBackOfficeTaskRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<BackOfficeTask>(HttpMethod.Post, "/back-office-tasks", request, cancellationToken);

    public Task<BackOfficeTask> ReadAsync(string taskId, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<BackOfficeTask>(HttpMethod.Get, $"/back-office-tasks/{Uri.EscapeDataString(taskId)}", null, cancellationToken);
}
