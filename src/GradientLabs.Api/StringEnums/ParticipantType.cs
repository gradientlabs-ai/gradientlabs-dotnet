namespace GradientLabs.Api;

public readonly record struct ParticipantType(string Value)
{
    public static readonly ParticipantType Customer = new("Customer");
    public static readonly ParticipantType Agent = new("Agent");
    public static readonly ParticipantType AiAgent = new("AI Agent");
    public static readonly ParticipantType Bot = new("Bot");

    public override string ToString() => Value;
    public static implicit operator string(ParticipantType v) => v.Value;
}
