namespace GradientLabs.Api;

public readonly record struct ParameterTransferMode(string Value)
{
    public static readonly ParameterTransferMode Base64 = new("base64");
    public static readonly ParameterTransferMode TemporaryUrl = new("temporary_url");

    public override string ToString() => Value;
    public static implicit operator string(ParameterTransferMode v) => v.Value;
}
