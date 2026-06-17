namespace GradientLabs.Api;

public readonly record struct ResourceTypeRefreshStrategy(string Value)
{
    public static readonly ResourceTypeRefreshStrategy Dynamic = new("dynamic");
    public static readonly ResourceTypeRefreshStrategy Static = new("static");

    public override string ToString() => Value;
    public static implicit operator string(ResourceTypeRefreshStrategy v) => v.Value;
}
