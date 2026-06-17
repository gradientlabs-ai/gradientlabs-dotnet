namespace GradientLabs.Api;

public readonly record struct ParameterType(string Value)
{
    public static readonly ParameterType String = new("string");
    public static readonly ParameterType Integer = new("integer");
    public static readonly ParameterType Float = new("float");
    public static readonly ParameterType Boolean = new("boolean");
    public static readonly ParameterType Date = new("date");
    public static readonly ParameterType Timestamp = new("timestamp");

    public override string ToString() => Value;
    public static implicit operator string(ParameterType v) => v.Value;
}
