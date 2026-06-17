namespace GradientLabs.Api;

public readonly record struct AttributeCardinality(string Value)
{
    public static readonly AttributeCardinality One = new("one");
    public static readonly AttributeCardinality Many = new("many");

    public override string ToString() => Value;
    public static implicit operator string(AttributeCardinality v) => v.Value;
}
