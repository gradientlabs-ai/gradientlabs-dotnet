namespace GradientLabs.Api;

public sealed class CreateResourceTypeRequest
{
    public string DisplayName { get; init; } = string.Empty;
    public ResourceTypeScope Scope { get; init; }
    public ResourceTypeRefreshStrategy RefreshStrategy { get; init; }
    public string? Description { get; init; }
    public bool? IsEnabled { get; init; }
    public ResourceTypeSourceConfig? SourceConfig { get; init; }

    /// <summary>The ID of a branch to create the resource type on. Send this or <see cref="BranchName"/>.</summary>
    public string? BranchId { get; init; }

    /// <summary>The git branch name of the branch to create on, as an alternative to <see cref="BranchId"/>.</summary>
    public string? BranchName { get; init; }
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
