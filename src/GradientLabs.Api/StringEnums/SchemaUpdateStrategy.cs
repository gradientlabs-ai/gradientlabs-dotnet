namespace GradientLabs.Api;

public readonly record struct SchemaUpdateStrategy(string Value)
{
    public static readonly SchemaUpdateStrategy Replace = new("replace");
    public static readonly SchemaUpdateStrategy Merge = new("merge");

    public override string ToString() => Value;
    public static implicit operator string(SchemaUpdateStrategy v) => v.Value;
}
