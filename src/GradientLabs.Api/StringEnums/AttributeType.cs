namespace GradientLabs.Api;

public readonly record struct AttributeType(string Value)
{
    public static readonly AttributeType String = new("string");
    public static readonly AttributeType Date = new("date");
    public static readonly AttributeType Timestamp = new("timestamp");
    public static readonly AttributeType Boolean = new("boolean");
    public static readonly AttributeType Number = new("number");
    public static readonly AttributeType Array = new("array");
    public static readonly AttributeType Complex = new("complex");

    public override string ToString() => Value;
    public static implicit operator string(AttributeType v) => v.Value;
}
