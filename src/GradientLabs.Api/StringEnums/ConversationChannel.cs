namespace GradientLabs.Api;

public readonly record struct ConversationChannel(string Value)
{
    public static readonly ConversationChannel Web = new("web");
    public static readonly ConversationChannel Email = new("email");
    public static readonly ConversationChannel Voice = new("voice");
    [Obsolete("Supply the channel used for the conversation.")]
    public static readonly ConversationChannel Unmapped = new("unmapped");

    public override string ToString() => Value;
    public static implicit operator string(ConversationChannel v) => v.Value;
}
