namespace GradientLabs.Api;

public readonly record struct ParameterSource(string Value)
{
    public static readonly ParameterSource Llm = new("llm");
    public static readonly ParameterSource Literal = new("literal");
    public static readonly ParameterSource Resource = new("resource");

    public override string ToString() => Value;
    public static implicit operator string(ParameterSource v) => v.Value;
}
