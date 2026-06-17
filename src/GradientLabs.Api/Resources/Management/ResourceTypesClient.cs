using GradientLabs.Api.Http;

namespace GradientLabs.Api;

public sealed class ResourceTypesClient
{
    private readonly HttpPipeline _pipeline;

    internal ResourceTypesClient(HttpPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    public Task<IReadOnlyList<ResourceType>> ListAsync(CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<IReadOnlyList<ResourceType>>(HttpMethod.Get, "/resource-types", null, cancellationToken);

    public Task<ResourceType> CreateAsync(CreateResourceTypeRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<ResourceType>(HttpMethod.Post, "/resource-types", request, cancellationToken);

    public Task<ResourceType> ReadAsync(string resourceTypeId, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<ResourceType>(HttpMethod.Get, $"/resource-types/{Uri.EscapeDataString(resourceTypeId)}", null, cancellationToken);

    public Task<ResourceType> UpdateAsync(string resourceTypeId, UpdateResourceTypeRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<ResourceType>(HttpMethod.Patch, $"/resource-types/{Uri.EscapeDataString(resourceTypeId)}", request, cancellationToken);

    public Task DeleteAsync(string resourceTypeId, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Delete, $"/resource-types/{Uri.EscapeDataString(resourceTypeId)}", null, cancellationToken);
}
