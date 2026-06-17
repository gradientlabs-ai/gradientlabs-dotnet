namespace GradientLabs.Api;

public sealed class CreateResourceTypeRequest
{
    public string DisplayName { get; init; } = string.Empty;
    public ResourceTypeScope Scope { get; init; }
    public ResourceTypeRefreshStrategy RefreshStrategy { get; init; }
    public string? Description { get; init; }
    public bool? IsEnabled { get; init; }
    public ResourceTypeSourceConfig? SourceConfig { get; init; }
}

public sealed class UpdateResourceTypeRequest
{
    public string? DisplayName { get; init; }
    public string? Description { get; init; }
    public bool? IsEnabled { get; init; }
    public ResourceTypeRefreshStrategy? RefreshStrategy { get; init; }
    public ResourceTypeScope? Scope { get; init; }
    public ResourceTypeSourceConfig? SourceConfig { get; init; }
}
