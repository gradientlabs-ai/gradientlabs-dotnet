namespace GradientLabs.Api;

public readonly record struct AttachmentType(string Value)
{
    public static readonly AttachmentType Image = new("image");
    public static readonly AttachmentType File = new("file");

    public override string ToString() => Value;
    public static implicit operator string(AttachmentType v) => v.Value;
}
