# GradientLabs.Api

Official C# client library for the [Gradient Labs API](https://api-docs.gradient-labs.ai).

## Installation

```bash
dotnet add package GradientLabs.Api
```

## Quick start

The API uses two separate key types — Integration and Management. Instantiate one client per key.

```csharp
using GradientLabs.Api;

// Integration key: conversations, back-office tasks, outbound, voice
using var integration = new GradientLabsIntegrationClient(new GradientLabsClientOptions
{
    ApiKey = Environment.GetEnvironmentVariable("GRADIENT_LABS_INTEGRATION_KEY")!,
});

// Management key: tools, procedures, articles, resources, etc.
using var management = new GradientLabsManagementClient(new GradientLabsClientOptions
{
    ApiKey = Environment.GetEnvironmentVariable("GRADIENT_LABS_MANAGEMENT_KEY")!,
});

// Start a conversation
var conversation = await integration.Conversations.StartAsync(new StartConversationRequest
{
    Id = "conv-001",
    CustomerId = "customer-001",
    Channel = ConversationChannel.Web,
});

// List tools
var tools = await management.Tools.ListAsync();
```

## Configuration

| Property | Type | Default | Description |
|---|---|---|---|
| `ApiKey` | `string` | — | Required. The API key for this client. |
| `BaseUri` | `Uri` | `https://api.gradient-labs.ai` | Override for local development. |
| `HttpClient` | `HttpClient?` | `null` | Supply your own; the SDK will not dispose it. |
| `HttpMessageHandler` | `HttpMessageHandler?` | `null` | Alternative to `HttpClient` for custom transports. |
| `Timeout` | `TimeSpan?` | `null` | Applied when the SDK creates its own `HttpClient`. |
| `WebhookSigningKey` | `string?` | `null` | Required to call `ParseWebhook`. |
| `WebhookLeeway` | `TimeSpan` | 5 minutes | Maximum age for accepted webhooks. |
| `TimeProvider` | `TimeProvider` | System | Override in tests. |
| `UserAgentVersion` | `string?` | Assembly version | Override in pre-release builds. |

## Error handling

```csharp
try
{
    var conv = await integration.Conversations.ReadAsync("unknown-id");
}
catch (GradientLabsApiException ex) when (ex.ErrorCode == GradientLabsErrorCode.NotFound)
{
    Console.WriteLine($"Not found. Trace: {ex.TraceId}");
}
catch (GradientLabsApiException ex) when (ex.IsRetryable)
{
    // Retry after a delay
}
catch (GradientLabsRequestException ex)
{
    // Transport failure — no response received
}
```

## Webhook verification

```csharp
var verifier = new GradientLabsWebhookVerifier(signingKey);

// In your ASP.NET Core controller or minimal API:
var body = await Request.BodyReader.ReadAsync();
var signature = Request.Headers["X-GradientLabs-Signature"].ToString();
var token = Request.Headers["X-GradientLabs-Token"].ToString();

WebhookEvent evt;
try
{
    evt = verifier.Parse(body.Buffer.FirstSpan, signature, token);
}
catch (GradientLabsWebhookSignatureException)
{
    return Results.Unauthorized();
}

switch (evt)
{
    case AgentMessageEvent msg:
        Console.WriteLine($"Message from agent: {msg.Body}");
        break;
    case ConversationHandOffEvent ho:
        Console.WriteLine($"Hand-off: {ho.Reason}");
        break;
    case UnknownWebhookEvent unknown:
        Console.WriteLine($"Unknown event type: {unknown.Type}");
        break;
}
```

Both clients also expose a `ParseWebhook` convenience method when `WebhookSigningKey` is set in options.

## Pagination

Only `Procedures.ListAsync` is paginated. All other list methods return the full result.

```csharp
string? cursor = null;
do
{
    var page = await management.Procedures.ListAsync(cursor: cursor);
    foreach (var p in page.Items)
        Console.WriteLine(p.Name);
    cursor = page.Next;
} while (cursor is not null);
```

## Examples

See [`examples/`](examples/) for runnable examples covering conversations, tools, articles, webhooks, procedures, resources, and back-office tasks.

## License

MIT
