using GradientLabs.Api.Http;

namespace GradientLabs.Api;

public sealed class ToolsClient
{
    private readonly HttpPipeline _pipeline;

    internal ToolsClient(HttpPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    /// <summary>Lists company tools and enabled MCP tools. Interactive tools are omitted.</summary>
    public async Task<IReadOnlyList<Tool>> ListAsync(CancellationToken cancellationToken = default)
    {
        var response = await _pipeline.SendAsync<ListToolsResponse>(HttpMethod.Get, "/tools", null, cancellationToken).ConfigureAwait(false);
        return response.Tools;
    }

    public Task<Tool> CreateAsync(CreateToolRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<Tool>(HttpMethod.Post, "/tools", request, cancellationToken);

    /// <summary>Enabled MCP tools can be read by ID. Interactive tools return 404.</summary>
    public Task<Tool> ReadAsync(string toolId, CancellationToken cancellationToken = default)
        => ReadAsync(toolId, null, cancellationToken);

    /// <summary>Enabled MCP tools can be read by ID. Interactive tools return 404.</summary>
    /// <param name="toolId">The ID of the tool.</param>
    /// <param name="version">The version of the tool to read. Leave it null to read the tool's current version.</param>
    /// <param name="cancellationToken">Cancels the request.</param>
    public Task<Tool> ReadAsync(string toolId, long? version, CancellationToken cancellationToken = default)
    {
        var qs = QueryBuilder.Build(new Dictionary<string, string?> { ["version"] = version?.ToString(System.Globalization.CultureInfo.InvariantCulture) });
        return _pipeline.SendAsync<Tool>(HttpMethod.Get, $"/tools/{Uri.EscapeDataString(toolId)}{qs}", null, cancellationToken);
    }

    /// <summary>Updates a webhook, HTTP, async or draft tool. Only draft tools can change kind. Edit repository-managed tools in their files.</summary>
    public Task<Tool> UpdateAsync(string toolId, UpdateToolRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<Tool>(HttpMethod.Put, $"/tools/{Uri.EscapeDataString(toolId)}", request, cancellationToken);

    public Task DeleteAsync(string toolId, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Delete, $"/tools/{Uri.EscapeDataString(toolId)}", null, cancellationToken);

    public Task<ExecuteToolResponse> ExecuteAsync(string toolId, ExecuteToolRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<ExecuteToolResponse>(HttpMethod.Post, $"/tools/{Uri.EscapeDataString(toolId)}/execute", request, cancellationToken);
}
