using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class StartOutboundChatConversationRequest
{
    /// <summary>Your own identifier for the customer, as used in your systems. It is echoed back to you in tool and webhook payloads.</summary>
    public string CustomerId { get; init; } = string.Empty;

    /// <summary>Identifies the outbound procedure (<c>proc_…</c>) that defines what the AI agent should accomplish. It must be live and enabled for the chat channel.</summary>
    public string ProcedureId { get; init; } = string.Empty;

    /// <summary>The support platform the chat is delivered on.</summary>
    public SupportPlatform SupportPlatform { get; init; }

    /// <summary>The opening message. When omitted, the AI agent generates one based on the procedure.</summary>
    public string? Body { get; init; }

    /// <summary>Links the customer to their records in third-party support platforms. The platform named in <see cref="SupportPlatform"/> needs an identifier here, unless the customer already carries one from an earlier conversation.</summary>
    public IReadOnlyList<CustomerSupportPlatformIdentifier>? CustomerSupportPlatformIdentifiers { get; init; }

    /// <summary>Structured data made available to the AI agent as context, keyed by resource type name.</summary>
    public Dictionary<string, JsonElement>? Resources { get; init; }
}

public sealed class StartOutboundEmailConversationRequest
{
    /// <summary>Your own identifier for the customer, as used in your systems. It is echoed back to you in tool and webhook payloads.</summary>
    public string CustomerId { get; init; } = string.Empty;

    /// <summary>Identifies the outbound procedure (<c>proc_…</c>) that defines what the AI agent should accomplish. It must be live and enabled for the email channel.</summary>
    public string ProcedureId { get; init; } = string.Empty;

    /// <summary>The support platform the email is sent from.</summary>
    public SupportPlatform SupportPlatform { get; init; }

    /// <summary>The subject line of the opening email. Required if <see cref="Body"/> is set, and forbidden otherwise. When both are omitted, the AI agent writes the opening email.</summary>
    public string? Subject { get; init; }

    /// <summary>The content of the opening email. Required if <see cref="Subject"/> is set, and forbidden otherwise. When both are omitted, the AI agent writes the opening email.</summary>
    public string? Body { get; init; }

    /// <summary>Links the customer to their records in third-party support platforms. The platform named in <see cref="SupportPlatform"/> needs an identifier here, unless the customer already carries one from an earlier conversation. Zendesk requires type <c>zendesk_support_user</c>; Salesforce requires type <c>salesforce_contact_id</c>.</summary>
    public IReadOnlyList<CustomerSupportPlatformIdentifier>? CustomerSupportPlatformIdentifiers { get; init; }

    /// <summary>Structured data made available to the AI agent as context, keyed by resource type name.</summary>
    public Dictionary<string, JsonElement>? Resources { get; init; }
}

public sealed class StartOutboundPhoneConversationRequest
{
    /// <summary>Your own identifier for the customer, as used in your systems. It is echoed back to you in tool and webhook payloads.</summary>
    public string CustomerId { get; init; } = string.Empty;

    /// <summary>Identifies the outbound procedure (<c>proc_…</c>) that defines what the AI agent should accomplish. It must be live and enabled for the voice channel.</summary>
    public string ProcedureId { get; init; } = string.Empty;

    /// <summary>The customer's phone number to dial, in E.164 format (e.g. <c>+14155551234</c>).</summary>
    public string ToPhoneNumber { get; init; } = string.Empty;

    /// <summary>The caller ID to place the call from, in E.164 format. It must be a phone number already provisioned for your company.</summary>
    public string FromPhoneNumber { get; init; } = string.Empty;

    /// <summary>Links the customer to their records in third-party support platforms, which also pulls that platform's customer data into the call as context.</summary>
    public IReadOnlyList<CustomerSupportPlatformIdentifier>? CustomerSupportPlatformIdentifiers { get; init; }

    /// <summary>Structured data made available to the AI agent as context, keyed by resource type name.</summary>
    public Dictionary<string, JsonElement>? Resources { get; init; }
}

public sealed class StartOutboundConversationResponse
{
    public string ConversationId { get; init; } = string.Empty;

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}
