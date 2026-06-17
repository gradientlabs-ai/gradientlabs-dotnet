namespace GradientLabs.Api;

public readonly record struct ResourceSourceBodyEncoding(string Value)
{
    public static readonly ResourceSourceBodyEncoding FormUrlEncoded = new("application/x-www-form-urlencoded");
    public static readonly ResourceSourceBodyEncoding Json = new("application/json");

    public override string ToString() => Value;
    public static implicit operator string(ResourceSourceBodyEncoding v) => v.Value;
}
