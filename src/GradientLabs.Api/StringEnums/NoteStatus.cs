namespace GradientLabs.Api;

public readonly record struct NoteStatus(string Value)
{
    public static readonly NoteStatus Draft = new("draft");
    public static readonly NoteStatus Live = new("live");
    public static readonly NoteStatus Deleted = new("deleted");

    public override string ToString() => Value;
    public static implicit operator string(NoteStatus v) => v.Value;
}
