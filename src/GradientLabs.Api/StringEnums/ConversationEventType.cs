namespace GradientLabs.Api;

public readonly record struct ConversationEventType(string Value)
{
    public static readonly ConversationEventType Assigned = new("assigned");
    public static readonly ConversationEventType Cancelled = new("cancelled");
    public static readonly ConversationEventType Finished = new("finished");
    public static readonly ConversationEventType Resumed = new("resumed");
    public static readonly ConversationEventType InternalNote = new("internal-note");
    public static readonly ConversationEventType Message = new("message");
    public static readonly ConversationEventType Delivered = new("delivered");
    public static readonly ConversationEventType Read = new("read");
    public static readonly ConversationEventType Rated = new("rated");
    public static readonly ConversationEventType Started = new("started");
    public static readonly ConversationEventType Typing = new("typing");
    public static readonly ConversationEventType AsyncToolResult = new("async-tool-result");

    public override string ToString() => Value;
    public static implicit operator string(ConversationEventType v) => v.Value;
}
