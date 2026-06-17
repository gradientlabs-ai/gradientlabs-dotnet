namespace GradientLabs.Api;

public readonly record struct ArticleVisibility(string Value)
{
    public static readonly ArticleVisibility Public = new("public");
    public static readonly ArticleVisibility Users = new("users");
    public static readonly ArticleVisibility Internal = new("internal");
    public static readonly ArticleVisibility Unknown = new("unknown");

    public override string ToString() => Value;
    public static implicit operator string(ArticleVisibility v) => v.Value;
}
