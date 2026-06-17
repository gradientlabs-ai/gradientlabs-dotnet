namespace GradientLabs.Api;

public readonly record struct GradientLabsWebhookEventType(string Value)
{
    public static readonly GradientLabsWebhookEventType AgentMessage = new("agent.message");
    public static readonly GradientLabsWebhookEventType ConversationHandOff = new("conversation.hand_off");
    public static readonly GradientLabsWebhookEventType ConversationFinished = new("conversation.finished");
    public static readonly GradientLabsWebhookEventType ActionExecute = new("action.execute");
    public static readonly GradientLabsWebhookEventType ResourcePull = new("resource.pull");
    public static readonly GradientLabsWebhookEventType BackOfficeTaskComplete = new("back-office-task.complete");
    public static readonly GradientLabsWebhookEventType BackOfficeTaskHandOff = new("back-office-task.hand-off");
    public static readonly GradientLabsWebhookEventType BackOfficeTaskFail = new("back-office-task.fail");

    public override string ToString() => Value;
    public static implicit operator string(GradientLabsWebhookEventType v) => v.Value;
}
