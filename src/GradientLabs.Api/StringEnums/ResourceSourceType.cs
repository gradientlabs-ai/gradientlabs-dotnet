namespace GradientLabs.Api;

public readonly record struct ResourceSourceType(string Value)
{
    public static readonly ResourceSourceType Http = new("http");
    public static readonly ResourceSourceType Internal = new("internal");
    public static readonly ResourceSourceType Webhook = new("webhook");

    public override string ToString() => Value;
    public static implicit operator string(ResourceSourceType v) => v.Value;
}
