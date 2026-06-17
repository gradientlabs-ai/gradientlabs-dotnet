namespace GradientLabs.Api;

public readonly record struct BodyEncoding(string Value)
{
    public static readonly BodyEncoding FormUrlEncoded = new("application/x-www-form-urlencoded");
    public static readonly BodyEncoding Json = new("application/json");

    public override string ToString() => Value;
    public static implicit operator string(BodyEncoding v) => v.Value;
}
