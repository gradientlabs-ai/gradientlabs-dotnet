namespace GradientLabs.Api;

public readonly record struct ArticleUsageStatus(string Value)
{
    public static readonly ArticleUsageStatus On = new("on");
    public static readonly ArticleUsageStatus Off = new("off");

    public override string ToString() => Value;
    public static implicit operator string(ArticleUsageStatus v) => v.Value;
}
