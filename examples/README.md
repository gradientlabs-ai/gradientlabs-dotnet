# Examples

Each subdirectory contains a runnable console app demonstrating a Gradient Labs API feature.

## Prerequisites

- .NET 8 SDK or later
- A Gradient Labs API key (set `GRADIENT_LABS_API_KEY`)
- Optionally set `GRADIENT_LABS_BASE_URL` to point at a local stack

## Running an example

```bash
cd conversations
dotnet run
```

## Examples

| Directory | What it demonstrates |
|---|---|
| `conversations/` | Start, add a message, read, cancel a conversation |
| `tools/` | Create, read, delete a tool |
| `articles/` | Upsert and delete an article |
| `webhooks/` | Run a local HTTP server that verifies and parses webhooks |
| `procedures/` | List procedures with cursor pagination |
| `resources/` | Create ResourceType + ResourceSource, then delete both |
| `back-office-tasks/` | Create and read a back-office task |

## Webhook example

The `webhooks/` example also requires `GRADIENT_LABS_WEBHOOK_SIGNING_KEY`:

```bash
cd webhooks
GRADIENT_LABS_WEBHOOK_SIGNING_KEY=your_key dotnet run
```

Then configure your workspace's webhook URL to `http://localhost:4001/` and trigger an event.
