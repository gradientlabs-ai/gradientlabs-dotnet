namespace GradientLabs.Api;

public readonly record struct ProcedureStatus(string Value)
{
    public static readonly ProcedureStatus Unsaved = new("unsaved");
    public static readonly ProcedureStatus Draft = new("draft");
    public static readonly ProcedureStatus Live = new("live");
    public static readonly ProcedureStatus Archived = new("archived");

    public override string ToString() => Value;
    public static implicit operator string(ProcedureStatus v) => v.Value;
}
