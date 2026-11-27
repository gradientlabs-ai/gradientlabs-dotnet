namespace GradientLabs.Api;

public readonly record struct ConversationEventType(string Value)
{
    [Obsolete("Use internal-note, delivered, read or typing when adding a conversation event.")]
    public static readonly ConversationEventType Assigned = new("assigned");
    [Obsolete("Use internal-note, delivered, read or typing when adding a conversation event.")]
    public static readonly ConversationEventType Cancelled = new("cancelled");
    [Obsolete("Use internal-note, delivered, read or typing when adding a conversation event.")]
    public static readonly ConversationEventType Finished = new("finished");
    [Obsolete("Use internal-note, delivered, read or typing when adding a conversation event.")]
    public static readonly ConversationEventType Resumed = new("resumed");
    public static readonly ConversationEventType InternalNote = new("internal-note");
    [Obsolete("Use internal-note, delivered, read or typing when adding a conversation event.")]
    public static readonly ConversationEventType Message = new("message");
    public static readonly ConversationEventType Delivered = new("delivered");
    public static readonly ConversationEventType Read = new("read");
    [Obsolete("Use internal-note, delivered, read or typing when adding a conversation event.")]
    public static readonly ConversationEventType Rated = new("rated");
    [Obsolete("Use internal-note, delivered, read or typing when adding a conversation event.")]
    public static readonly ConversationEventType Started = new("started");
    public static readonly ConversationEventType Typing = new("typing");
    [Obsolete("Use internal-note, delivered, read or typing when adding a conversation event.")]
    public static readonly ConversationEventType AsyncToolResult = new("async-tool-result");

    public override string ToString() => Value;
    public static implicit operator string(ConversationEventType v) => v.Value;
}
