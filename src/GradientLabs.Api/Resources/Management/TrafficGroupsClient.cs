using GradientLabs.Api.Http;

namespace GradientLabs.Api;

public sealed class TrafficGroupsClient
{
    private readonly HttpPipeline _pipeline;

    internal TrafficGroupsClient(HttpPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    public Task<IReadOnlyList<TrafficGroup>> ListAsync(CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<IReadOnlyList<TrafficGroup>>(HttpMethod.Get, "/traffic-groups", null, cancellationToken);

    public Task<TrafficGroup> CreateAsync(CreateTrafficGroupRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<TrafficGroup>(HttpMethod.Post, "/traffic-groups", request, cancellationToken);

    public Task<TrafficGroup> UpdateAsync(string groupId, UpdateTrafficGroupRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<TrafficGroup>(HttpMethod.Patch, $"/traffic-groups/{Uri.EscapeDataString(groupId)}", request, cancellationToken);

    public Task DeleteAsync(string groupId, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Delete, $"/traffic-groups/{Uri.EscapeDataString(groupId)}", null, cancellationToken);

    public Task<TrafficGroup> CreateTargetAsync(string groupId, CreateTrafficGroupTargetRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<TrafficGroup>(HttpMethod.Post, $"/traffic-groups/{Uri.EscapeDataString(groupId)}/targets", request, cancellationToken);

    public Task DeleteTargetAsync(string groupId, string targetId, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Delete, $"/traffic-groups/{Uri.EscapeDataString(groupId)}/targets/{Uri.EscapeDataString(targetId)}", null, cancellationToken);

    public Task<TrafficGroup> CreateExclusionAsync(string groupId, CreateTrafficGroupExclusionRequest request, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync<TrafficGroup>(HttpMethod.Post, $"/traffic-groups/{Uri.EscapeDataString(groupId)}/exclusions", request, cancellationToken);

    public Task DeleteExclusionAsync(string groupId, string targetId, CancellationToken cancellationToken = default)
        => _pipeline.SendAsync(HttpMethod.Delete, $"/traffic-groups/{Uri.EscapeDataString(groupId)}/exclusions/{Uri.EscapeDataString(targetId)}", null, cancellationToken);
}
