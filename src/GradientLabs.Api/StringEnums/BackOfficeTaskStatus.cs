namespace GradientLabs.Api;

public readonly record struct BackOfficeTaskStatus(string Value)
{
    public static readonly BackOfficeTaskStatus Pending = new("pending");
    public static readonly BackOfficeTaskStatus InProgress = new("in-progress");
    public static readonly BackOfficeTaskStatus Completed = new("completed");
    public static readonly BackOfficeTaskStatus Failed = new("failed");
    public static readonly BackOfficeTaskStatus HandedOff = new("handed-off");

    public override string ToString() => Value;
    public static implicit operator string(BackOfficeTaskStatus v) => v.Value;
}
