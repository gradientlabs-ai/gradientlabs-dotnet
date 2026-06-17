namespace GradientLabs.Api;

public readonly record struct ResourceTypeScope(string Value)
{
    public static readonly ResourceTypeScope Global = new("global");
    public static readonly ResourceTypeScope Local = new("local");

    public override string ToString() => Value;
    public static implicit operator string(ResourceTypeScope v) => v.Value;
}
