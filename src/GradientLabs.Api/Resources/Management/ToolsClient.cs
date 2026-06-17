using GradientLabs.Api.Http;

namespace GradientLabs.Api;

public sealed class ToolsClient
{
    private readonly HttpPipeline _pipeline;

    internal ToolsClient(HttpPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    public Task<IReadOnlyList<Tool>> ListAsync(CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<IReadOnlyList<Tool>>(HttpMethod.Get, "/tools", null, cancellationToken);

    public Task<Tool> CreateAsync(CreateToolRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<Tool>(HttpMethod.Post, "/tools", request, cancellationToken);

    public Task<Tool> ReadAsync(string toolId, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<Tool>(HttpMethod.Get, $"/tools/{Uri.EscapeDataString(toolId)}", null, cancellationToken);

    public Task<Tool> UpdateAsync(string toolId, UpdateToolRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<Tool>(HttpMethod.Patch, $"/tools/{Uri.EscapeDataString(toolId)}", request, cancellationToken);

    public Task DeleteAsync(string toolId, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Delete, $"/tools/{Uri.EscapeDataString(toolId)}", null, cancellationToken);

    public Task<ExecuteToolResponse> ExecuteAsync(string toolId, ExecuteToolRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<ExecuteToolResponse>(HttpMethod.Post, $"/tools/{Uri.EscapeDataString(toolId)}/execute", request, cancellationToken);
}
