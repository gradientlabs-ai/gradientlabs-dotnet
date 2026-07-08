using System.Text.Json;

namespace GradientLabs.Api;

public sealed class StartConversationRequest
{
    public string Id { get; init; } = string.Empty;
    public string CustomerId { get; init; } = string.Empty;
    public ConversationChannel Channel { get; init; }
    public Dictionary<string, JsonElement>? Resources { get; init; }
    public string? TrafficGroupId { get; init; }
    public string? AssigneeType { get; init; }
    public string? AssigneeId { get; init; }
    public string? ConversationToken { get; init; }
    public DateTimeOffset? Created { get; init; }

    /// <summary>Links the customer to their record(s) in third-party support platforms, alongside <see cref="CustomerId"/>.</summary>
    public IReadOnlyList<CustomerSupportPlatformIdentifier>? CustomerSupportPlatformIdentifiers { get; init; }
}

public sealed class CustomerSupportPlatformIdentifier
{
    public SupportPlatform SupportPlatform { get; init; }

    /// <summary>Only required (and validated) for <c>intercom</c>, <c>zendesk</c>, and <c>salesforce</c>; omit for <c>freshchat</c>/<c>freshdesk</c>.</summary>
    public CustomerSupportPlatformIdentifierType? Type { get; init; }

    public string Value { get; init; } = string.Empty;
}

public sealed class CancelConversationRequest
{
    public string? Reason { get; init; }
    public DateTimeOffset? Timestamp { get; init; }
}

public sealed class FinishConversationRequest
{
    public string? Reason { get; init; }
    public DateTimeOffset? Timestamp { get; init; }
}

public sealed class AddMessageRequest
{
    public string Id { get; init; } = string.Empty;
    public string Body { get; init; } = string.Empty;
    public string ParticipantId { get; init; } = string.Empty;
    public ParticipantType ParticipantType { get; init; }
    public IReadOnlyList<Attachment>? Attachments { get; init; }
    public string? ConversationToken { get; init; }
    public DateTimeOffset? Created { get; init; }
    public string? Subject { get; init; }
}

public sealed class ResumeConversationRequest
{
    public string AssigneeType { get; init; } = string.Empty;
    public Dictionary<string, JsonElement>? Resources { get; init; }
    public string? AssigneeId { get; init; }
    public string? Reason { get; init; }
    public DateTimeOffset? Timestamp { get; init; }
}

public sealed class ReturnAsyncToolResultRequest
{
    public string AsyncToolExecutionId { get; init; } = string.Empty;
    public object? Payload { get; init; }
    public DateTimeOffset? Timestamp { get; init; }
}

public sealed class AssignConversationRequest
{
    public string AssigneeType { get; init; } = string.Empty;
    public string? AssigneeId { get; init; }
    public string? Reason { get; init; }
    public DateTimeOffset? Timestamp { get; init; }
}

public sealed class AddEventRequest
{
    public ConversationEventType Type { get; init; }
    public string ParticipantId { get; init; } = string.Empty;
    public ParticipantType ParticipantType { get; init; }
    public string? Body { get; init; }
    public string? IdempotencyKey { get; init; }
    public string? MessageId { get; init; }
    public DateTimeOffset? Timestamp { get; init; }
}

public sealed class RateConversationRequest
{
    public string Type { get; init; } = string.Empty;
    public long Value { get; init; }
    public long MaxValue { get; init; }
    public long MinValue { get; init; }
    public string? Comments { get; init; }
    public DateTimeOffset? Timestamp { get; init; }
}
