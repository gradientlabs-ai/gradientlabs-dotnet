namespace GradientLabs.Api;

public readonly record struct ArticleStatus(string Value)
{
    public static readonly ArticleStatus Draft = new("draft");
    public static readonly ArticleStatus Published = new("published");
    public static readonly ArticleStatus Deleted = new("deleted");
    public static readonly ArticleStatus Excluded = new("excluded");
    public static readonly ArticleStatus Unknown = new("unknown");

    public override string ToString() => Value;
    public static implicit operator string(ArticleStatus v) => v.Value;
}
