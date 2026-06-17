# C# Client Library Plan — Gradient Labs API

## 1. Goal

Produce `GradientLabs.Api`, a zero-runtime-dependency .NET 8 client library for the Gradient Labs
public API at `https://api.gradient-labs.ai`. The library exposes two strongly-typed entry points:
`GradientLabsIntegrationClient` for integration-key endpoints (conversations, outbound, voice) and
`GradientLabsManagementClient` for management-key endpoints (articles, tools, procedures,
resources, and configuration). It is async-first with `CancellationToken` support on every method,
ships on NuGet.org, and includes a webhook verifier backed by HMAC-SHA256 with constant-time
comparison. The client does not validate key type — the server returns `permission_denied` if the
wrong key is used.

---

## 2. API Surface

The production base URL is `https://api.gradient-labs.ai`. The spec lists `http://localhost:4000`;
that is the local dev address only. The client default must be the production URL.

**Not exposed**: `GET /spec.json`.

### Key roles

| Key type | Endpoints |
|---|---|
| Integration | Conversations, BackOfficeTasks, Outbound, Voice |
| Management | Articles, Tools, Procedures, ResourceTypes, ResourceSources, HandOffTargets, Notes, TrafficGroups, Secrets, Topics, TerminologySubstitutions, IpAddresses |

### `GradientLabsIntegrationClient`

| Property | Methods |
|---|---|
| `Conversations` | `StartAsync`, `CancelAsync`, `FinishAsync`, `AddMessageAsync`, `ResumeAsync`, `ReturnAsyncToolResultAsync`, `AssignAsync`, `AddEventAsync`, `RateAsync`, `ReadAsync`, `ReadDeprecatedAsync` _(marked `[Obsolete("Use ReadAsync instead.")]`)_ |
| `BackOfficeTasks` | `CreateAsync`, `ReadAsync` |
| `Outbound` | `StartConversationAsync` |
| `Voice` | `ReadLatestCallContextAsync` |

### `GradientLabsManagementClient`

| Property | Methods |
|---|---|
| `Articles` | `UpsertAsync`, `DeleteAsync`, `SetUsageStatusAsync` |
| `Tools` | `ListAsync`, `CreateAsync`, `ReadAsync`, `UpdateAsync`, `DeleteAsync`, `ExecuteAsync` |
| `Procedures` | `ListAsync`, `ReadAsync`, `SetLimitAsync`, `ListVersionsAsync`, `SetLiveAsync`, `UnsetLiveAsync`, `SetGatedAsync`, `UnsetGatedAsync` |
| `ResourceTypes` | `ListAsync`, `CreateAsync`, `ReadAsync`, `UpdateAsync`, `DeleteAsync` |
| `ResourceSources` | `ListAsync`, `CreateAsync`, `ReadAsync`, `UpdateAsync`, `DeleteAsync`, `UpdateSchemaByExamplesAsync` |
| `HandOffTargets` | `ListAsync`, `UpsertAsync`, `DeleteAsync`, `GetDefaultAsync`, `SetDefaultAsync` |
| `Notes` | `CreateAsync`, `UpdateAsync`, `DeleteAsync`, `SetStatusAsync` |
| `TrafficGroups` | `ListAsync`, `CreateAsync`, `UpdateAsync`, `DeleteAsync`, `CreateTargetAsync`, `DeleteTargetAsync`, `CreateExclusionAsync`, `DeleteExclusionAsync` |
| `Secrets` | `ListAsync`, `WriteAsync`, `RevokeAsync` |
| `Topics` | `ListAsync`, `ReadAsync`, `UpsertArticleTopicAsync` |
| `TerminologySubstitutions` | `ListAsync`, `CreateAsync`, `ReadAsync`, `UpdateAsync`, `DeleteAsync` |
| `IpAddresses` | `ListAsync` |

### Method conventions

- Every method is async and returns `Task<T>` or `Task`.
- Every method accepts `CancellationToken cancellationToken = default` as the final parameter.
- Request models are named `*Request`; response models are named `*Response`.
- Collection methods return `IReadOnlyList<T>` unless the endpoint is paginated, in which case
  they return `Page<T>`.

### Open-string enum strategy

Do **not** use C# `enum`. The API may add new values at any time and enums break on unknown values.
Use `readonly record struct` with a `string Value` and well-known constants:

```csharp
public readonly record struct ArticleStatus(string Value)
{
    public static readonly ArticleStatus Draft     = new("draft");
    public static readonly ArticleStatus Published = new("published");
    public static readonly ArticleStatus Deleted   = new("deleted");
    public static readonly ArticleStatus Excluded  = new("excluded");
    public static readonly ArticleStatus Unknown   = new("unknown");

    public override string ToString() => Value;
    public static implicit operator string(ArticleStatus v) => v.Value;
}
```

Unknown values deserialise without throwing — `new ArticleStatus("future-value")` is valid.

**All open-string enum types and their known values** (sourced from Go source constants):

| C# type | Known values |
|---|---|
| `ArticleStatus` | `draft`, `published`, `deleted`, `excluded`, `unknown` |
| `ArticleUsageStatus` | `on`, `off` |
| `ArticleVisibility` | `public`, `users`, `internal`, `unknown` |
| `AttachmentType` | `image`, `file` |
| `ConversationChannel` | `web` _(legacy)_, `email`, `voice`, `unmapped` |
| `CustomerSource` | `dixa`, `intercom`, `freshchat`, `freshdesk`, `public-api`, `chat-sdk`, `salesforce`, `zendesk`, `livekit`, `twilio`, `talkdesk`, `intercom-voice`, `livechat`, `web-app`, `gmail`, `file` |
| `ParticipantType` | `Customer`, `Agent`, `AI Agent`, `Bot` |
| `ConversationEventType` | `assigned`, `cancelled`, `finished`, `resumed`, `internal-note`, `message`, `delivered`, `read`, `rated`, `started`, `typing`, `async-tool-result` |
| `ProcedureStatus` | `unsaved`, `draft`, `live`, `archived` |
| `NoteStatus` | `draft`, `live`, `deleted` |
| `BackOfficeTaskStatus` | `pending`, `in-progress`, `completed`, `failed`, `handed-off` |
| `ResourceSourceBodyEncoding` | `application/x-www-form-urlencoded`, `application/json` |
| `AttributeCardinality` | `one`, `many` |
| `AttributeType` | `string`, `date`, `timestamp`, `boolean`, `number`, `array`, `complex` |
| `ResourceSourceRefreshStrategy` | `dynamic`, `static` |
| `ResourceSourceScope` | `global`, `local` |
| `ResourceSourceType` | `http`, `internal`, `webhook` |
| `SchemaUpdateStrategy` | `replace`, `merge` |
| `ResourceTypeRefreshStrategy` | `dynamic`, `static` |
| `ResourceTypeScope` | `global`, `local` |
| `SupportPlatform` | `dixa`, `freshchat`, `freshdesk`, `gmail`, `intercom`, `livechat`, `public-api`, `chat-sdk`, `salesforce`, `zendesk`, `livekit`, `twilio`, `talkdesk`, `intercom-voice`, `conversation-synthesizor`, `web-app` |
| `BodyEncoding` | `application/x-www-form-urlencoded`, `application/json` |
| `ParameterSource` | `llm`, `literal`, `resource` |
| `ParameterType` | `string`, `integer`, `float`, `boolean`, `date`, `timestamp` |

---

## 3. Client Configuration

Use a plain options object — idiomatic for modern C# SDK design (cf. Azure SDK, Stripe .NET).

```csharp
var integration = new GradientLabsIntegrationClient(new GradientLabsClientOptions
{
    ApiKey = Environment.GetEnvironmentVariable("GRADIENT_LABS_API_KEY")
});

var management = new GradientLabsManagementClient(new GradientLabsClientOptions
{
    ApiKey = Environment.GetEnvironmentVariable("GRADIENT_LABS_API_KEY")
});
```

### `GradientLabsClientOptions` fields

| Property | Type | Default | Notes |
|---|---|---|---|
| `ApiKey` | `string` | — | Required. Validated non-empty at construction. |
| `BaseUri` | `Uri` | `https://api.gradient-labs.ai` | Must be absolute. |
| `HttpClient` | `HttpClient?` | `null` | If supplied, SDK does not dispose it. |
| `HttpMessageHandler` | `HttpMessageHandler?` | `null` | Equivalent to Go's `WithTransport`. Mutually exclusive with `HttpClient`. |
| `Timeout` | `TimeSpan?` | `null` | Applied only when the SDK creates its own `HttpClient`. |
| `WebhookSigningKey` | `string?` | `null` | Required to call `ParseWebhook` on the client. |
| `WebhookLeeway` | `TimeSpan` | `TimeSpan.FromMinutes(5)` | Maximum age of an accepted webhook. |
| `TimeProvider` | `TimeProvider` | `TimeProvider.System` | Override in tests. |
| `UserAgentVersion` | `string?` | Assembly informational version | Override in tests and pre-release builds. |

The constructor throws `GradientLabsException` if `ApiKey` is null or empty.

---

## 4. Error Handling

### Exception hierarchy

```
Exception
└── GradientLabsException                  // base for all SDK exceptions
    ├── GradientLabsApiException           // non-2xx HTTP response from the API
    ├── GradientLabsRequestException       // transport failure before receiving a response
    ├── GradientLabsSerializationException // invalid JSON on a successful (2xx) response
    └── GradientLabsWebhookException       // webhook failure
        └── GradientLabsWebhookSignatureException // bad signature, stale timestamp, malformed header
```

### `GradientLabsApiException`

```csharp
public sealed class GradientLabsApiException : GradientLabsException
{
    public HttpStatusCode             StatusCode   { get; }
    public GradientLabsErrorCode      ErrorCode    { get; }  // from "code" field; Unknown if absent
    public string                     ApiMessage   { get; }  // from "message" field
    public string?                    TraceId      { get; }  // from details["trace_id"]
    public IReadOnlyDictionary<string, JsonElement>? Details { get; }
    public string?                    ResponseBody { get; }  // raw body, capped at 64 KiB
    public bool                       IsRetryable  { get; }  // derived from ErrorCode
}
```

The docs recommend switching on `ErrorCode` rather than HTTP status:

```csharp
catch (GradientLabsApiException ex)
    when (ex.ErrorCode == GradientLabsErrorCode.NotFound) { ... }
```

### `GradientLabsErrorCode`

Follows the same `readonly record struct` pattern as open-string enums. Includes `IsRetryable`
derived directly on the value type for use without an exception instance.

```csharp
public readonly record struct GradientLabsErrorCode(string Value)
{
    // The 10 well-known API error codes
    public static readonly GradientLabsErrorCode NotFound           = new("not_found");
    public static readonly GradientLabsErrorCode Unauthenticated    = new("unauthenticated");
    public static readonly GradientLabsErrorCode PermissionDenied   = new("permission_denied");
    public static readonly GradientLabsErrorCode InvalidArgument    = new("invalid_argument");
    public static readonly GradientLabsErrorCode FailedPrecondition = new("failed_precondition");
    public static readonly GradientLabsErrorCode ResourceExhausted  = new("resource_exhausted");
    public static readonly GradientLabsErrorCode AlreadyExists      = new("already_exists");
    public static readonly GradientLabsErrorCode Unavailable        = new("unavailable");
    public static readonly GradientLabsErrorCode DeadlineExceeded   = new("deadline_exceeded");
    public static readonly GradientLabsErrorCode Internal           = new("internal");

    // Additional retryable codes documented by the API
    public static readonly GradientLabsErrorCode Aborted            = new("aborted");
    public static readonly GradientLabsErrorCode Unknown            = new("unknown");

    public bool IsRetryable =>
        this == Unavailable || this == DeadlineExceeded ||
        this == Aborted     || this == Unknown          || this == Internal;

    public override string ToString() => Value;
    public static implicit operator string(GradientLabsErrorCode v) => v.Value;
}
```

**Retryable error codes** (per API docs): `unavailable`, `deadline_exceeded`, `aborted`,
`unknown`, `internal`.

### `GradientLabsWebhookSignatureException`

Thrown on bad signature, malformed `X-GradientLabs-Signature` header, stale timestamp, or
missing signing key. Separate subclass so callers can respond with HTTP 401 vs HTTP 500 cleanly.

---

## 5. Webhook Support

### `GradientLabsWebhookVerifier`

Exposes both low-level (verify only) and high-level (verify + parse) methods, mirroring the Go
client's pattern:

```csharp
public sealed class GradientLabsWebhookVerifier
{
    public GradientLabsWebhookVerifier(string signingKey, GradientLabsClientOptions? options = null);

    // Low-level: verify signature only, return timestamp metadata
    public WebhookVerifyResult Verify(
        ReadOnlySpan<byte> rawBody,
        string signatureHeader,
        DateTimeOffset? now = null);

    // High-level: verify + parse + token passthrough
    public WebhookEvent Parse(
        ReadOnlySpan<byte> rawBody,
        string signatureHeader,
        string? tokenHeader = null,
        DateTimeOffset? now = null);
}
```

Both clients also expose a convenience method when `WebhookSigningKey` is set in options:

```csharp
WebhookEvent ParseWebhook(ReadOnlySpan<byte> rawBody, string signatureHeader, string? tokenHeader = null);
```

### Verification algorithm

1. Require a non-empty signing key. Throw `GradientLabsWebhookSignatureException` if absent.
2. Require a non-empty `X-GradientLabs-Signature` header. Throw if absent or empty.
3. Parse comma-separated pairs. Require `t=<unix>` and at least one `v1=<hex>`.
4. Reject if `|now − timestamp| > leeway`. Default leeway: 5 minutes.
5. Compute HMAC-SHA256 of `<timestamp_string>.<raw_body_bytes>` using signing key bytes.
6. Decode each `v1` value from hex. Compare each with `CryptographicOperations.FixedTimeEquals`.
7. Accept if any signature matches. Throw `GradientLabsWebhookSignatureException` if none match.
8. **Never** reserialise the JSON body before verification — use the exact raw bytes received.

Verification always happens **before** type discrimination.

### Event envelope

All webhook events share:

| Property | Type |
|---|---|
| `Id` | `string` |
| `Type` | `GradientLabsWebhookEventType` |
| `SequenceNumber` | `long` |
| `Timestamp` | `DateTimeOffset` |
| `Token` | `string?` — `X-GradientLabs-Token` passthrough; `null` when header absent |

### Supported event types

| Wire value | C# event class |
|---|---|
| `agent.message` | `AgentMessageEvent` |
| `conversation.hand_off` | `ConversationHandOffEvent` |
| `conversation.finished` | `ConversationFinishedEvent` |
| `action.execute` | `ActionExecuteEvent` |
| `resource.pull` | `ResourcePullEvent` |
| `back-office-task.complete` | `BackOfficeTaskCompleteEvent` |
| `back-office-task.hand-off` | `BackOfficeTaskHandOffEvent` |
| `back-office-task.fail` | `BackOfficeTaskFailEvent` |

### Unknown event handling

Unknown `type` values do **not** throw after signature verification. Return
`UnknownWebhookEvent` with the raw `type` string, common envelope fields, and `Data` as
`JsonElement` for the caller to inspect.

### Webhook retry behaviour (document for consumers)

- Conversation events: up to 36 retries, maximum 1-hour retry interval.
- Action events: up to 36 retries, maximum 1-minute retry interval.
- Delivery timeout: 10 seconds.
- Use `Id` and `SequenceNumber` for idempotency and ordering.

---

## 6. Pagination

The only paginated endpoint in the current spec is `GET /procedures`.

```csharp
public sealed class Page<T>
{
    public required IReadOnlyList<T>              Items                { get; init; }
    public string?                                Next                 { get; init; }
    public string?                                Prev                 { get; init; }
    public bool HasNextPage => Next is not null;
    public bool HasPrevPage => Prev is not null;

    [JsonExtensionData]
    public IDictionary<string, JsonElement>?      AdditionalProperties { get; init; }
}
```

Cursor strings are opaque — do not parse them. No automatic enumeration in v1; callers iterate
manually:

```csharp
string? cursor = null;
do
{
    var page = await client.Procedures.ListAsync(cursor: cursor, cancellationToken);
    // process page.Items
    cursor = page.Next;
} while (cursor is not null);
```

---

## 7. Repo Structure

```
gradientlabs-dotnet/
├── .editorconfig
├── .gitignore
├── CHANGELOG.md
├── CSHARP_CLIENT_PLAN.md
├── Directory.Build.props          # shared assembly metadata, nullable, TreatWarningsAsErrors
├── Directory.Packages.props       # centralised package version management
├── GradientLabs.Api.sln
├── LICENSE                        # MIT
├── README.md
├── global.json                    # pins .NET SDK version
├── .github/
│   ├── CODEOWNERS
│   └── workflows/
│       ├── ci.yml
│       └── publish.yml
├── src/
│   └── GradientLabs.Api/
│       ├── GradientLabs.Api.csproj
│       ├── GradientLabsIntegrationClient.cs
│       ├── GradientLabsManagementClient.cs
│       ├── GradientLabsClientOptions.cs
│       ├── Errors/
│       │   ├── GradientLabsException.cs
│       │   ├── GradientLabsApiException.cs
│       │   ├── GradientLabsErrorCode.cs
│       │   ├── GradientLabsRequestException.cs
│       │   ├── GradientLabsSerializationException.cs
│       │   └── GradientLabsWebhookException.cs       # + GradientLabsWebhookSignatureException
│       ├── Http/
│       │   ├── HttpPipeline.cs        # internal: auth, User-Agent, error parsing
│       │   └── QueryBuilder.cs        # internal: builds query strings
│       ├── Models/
│       │   ├── Common/
│       │   │   └── Page.cs
│       │   ├── Integration/
│       │   │   ├── BackOfficeTasks/
│       │   │   ├── Conversations/
│       │   │   ├── Outbound/
│       │   │   └── Voice/
│       │   └── Management/
│       │       ├── Articles/
│       │       ├── HandOffTargets/
│       │       ├── IpAddresses/
│       │       ├── Notes/
│       │       ├── Procedures/
│       │       ├── ResourceSources/
│       │       ├── ResourceTypes/
│       │       ├── Secrets/
│       │       ├── TerminologySubstitutions/
│       │       ├── Tools/
│       │       ├── Topics/
│       │       └── TrafficGroups/
│       ├── Serialization/
│       │   ├── GradientLabsJsonOptions.cs      # snake_case + converters
│       │   ├── NanosecondTimeSpanConverter.cs
│       │   └── OpenStringConverter.cs          # shared converter for readonly record structs
│       ├── StringEnums/
│       │   └── (one file per open-string enum type)
│       └── Webhooks/
│           ├── GradientLabsWebhookVerifier.cs
│           ├── WebhookVerifyResult.cs
│           ├── WebhookEvent.cs
│           ├── GradientLabsWebhookEventType.cs
│           ├── UnknownWebhookEvent.cs
│           └── Events/
│               ├── AgentMessageEvent.cs
│               ├── ConversationHandOffEvent.cs
│               ├── ConversationFinishedEvent.cs
│               ├── ActionExecuteEvent.cs
│               ├── ResourcePullEvent.cs
│               ├── BackOfficeTaskCompleteEvent.cs
│               ├── BackOfficeTaskHandOffEvent.cs
│               └── BackOfficeTaskFailEvent.cs
├── tests/
│   └── GradientLabs.Api.Tests/
│       ├── GradientLabs.Api.Tests.csproj
│       ├── Fakes/
│       │   └── FakeHttpMessageHandler.cs
│       ├── HttpPipelineTests.cs
│       ├── SerializationTests.cs
│       ├── WebhookVerifierTests.cs
│       └── PaginationTests.cs
└── examples/
    ├── README.md
    ├── conversations/
    ├── tools/
    ├── articles/
    ├── webhooks/
    ├── procedures/
    ├── resources/
    └── back-office-tasks/
```

---

## 8. Build and Dependency Plan

### `GradientLabs.Api.csproj`

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <LangVersion>12.0</LangVersion>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
    <Deterministic>true</Deterministic>

    <PackageId>GradientLabs.Api</PackageId>
    <Version>0.1.0</Version>
    <Authors>Gradient Labs</Authors>
    <Description>Official C# client library for the Gradient Labs API.</Description>
    <PackageLicenseExpression>MIT</PackageLicenseExpression>
    <PackageReadmeFile>README.md</PackageReadmeFile>
    <RepositoryUrl>https://github.com/gradientlabs-ai/gradientlabs-dotnet</RepositoryUrl>
    <RepositoryType>git</RepositoryType>

    <PublishRepositoryUrl>true</PublishRepositoryUrl>
    <EmbedUntrackedSources>true</EmbedUntrackedSources>
    <IncludeSymbols>true</IncludeSymbols>
    <SymbolPackageFormat>snupkg</SymbolPackageFormat>
    <ContinuousIntegrationBuild Condition="'$(CI)' == 'true'">true</ContinuousIntegrationBuild>
  </PropertyGroup>
</Project>
```

**Runtime dependencies: none.** Every capability — `HttpClient`, `System.Text.Json`,
`System.Security.Cryptography`, `CryptographicOperations.FixedTimeEquals`, `TimeProvider` — is
in the .NET 8 BCL.

**Test dependencies:**

| Package | Reason |
|---|---|
| `xunit` | Standard .NET test framework |
| `xunit.runner.visualstudio` | VS/Rider integration |
| `Microsoft.NET.Test.Sdk` | Required test SDK |
| `coverlet.collector` | Code coverage |
| `FluentAssertions` | Readable assertions |

### Model conventions

- Response models: `sealed` classes, nullable annotations enabled, `[JsonExtensionData]` on
  `IDictionary<string, JsonElement>?` for forward compatibility with new API fields.
- Request models: `sealed` classes with `init`-only properties.

### HTTP pipeline conventions

- All API methods are `async`, return `Task<T>` / `Task`.
- Every method accepts `CancellationToken cancellationToken = default`.
- Use `HttpClient.SendAsync(..., HttpCompletionOption.ResponseHeadersRead, cancellationToken)`.
- Use `ConfigureAwait(false)` on all internal awaits — library code must not capture a
  synchronisation context.
- If an external `HttpClient` is supplied, do not dispose it.
- If the SDK creates its own `HttpClient`, dispose it with the client.
- Buffer error response bodies up to 64 KiB; truncate beyond that.

### Duration handling

Go `time.Duration` = int64 nanoseconds in JSON. Map to `TimeSpan` via
`NanosecondTimeSpanConverter`:

```csharp
// Read: TimeSpan ticks are 100 ns each
public static TimeSpan FromNanoseconds(long ns) => TimeSpan.FromTicks(ns / 100);
// Write: check for overflow before multiplying
public static long ToNanoseconds(TimeSpan ts) => checked(ts.Ticks * 100L);
```

TimeSpan precision is 100 ns; Go duration precision is 1 ns. Sub-100ns values are truncated on
read. Throw `JsonException` on write overflow.

### JSON serialization

Use `System.Text.Json` with a shared internal `GradientLabsJsonOptions` instance. The API uses
snake_case JSON — `JsonSerializerDefaults.Web` alone is not sufficient; the naming policy must
be explicitly set:

```csharp
new JsonSerializerOptions(JsonSerializerDefaults.Web)
{
    PropertyNamingPolicy       = JsonNamingPolicy.SnakeCaseLower,
    DefaultIgnoreCondition     = JsonIgnoreCondition.WhenWritingNull,
    PropertyNameCaseInsensitive = false,
}
// + NanosecondTimeSpanConverter
// + OpenStringConverter for all readonly record struct enum types
```

---

## 9. Testing Plan

All tests run without network access. HTTP behaviour is tested via `FakeHttpMessageHandler`.

### `HttpPipelineTests`

- `Authorization: Bearer <key>` is present on every request
- `User-Agent` matches `Gradient-Labs-CSharp/<version> (.NET/<runtime>)`
- `Content-Type: application/json` is set when a request body is present; absent on GET
- `Accept: application/json` is always present
- A non-2xx response with a valid error body yields `GradientLabsApiException` with correct
  `StatusCode`, `ErrorCode`, `ApiMessage`, `TraceId`, `Details`, and `ResponseBody`
- `IsRetryable` is `true` for `unavailable`, `deadline_exceeded`, `aborted`, `unknown`,
  `internal`; `false` for `not_found`, `unauthenticated`, `permission_denied`,
  `invalid_argument`, `failed_precondition`, `resource_exhausted`, `already_exists`
- A non-2xx response with a non-JSON body still yields `GradientLabsApiException` (no secondary
  exception during error parsing)
- Transport failure throws `GradientLabsRequestException`
- Invalid JSON on a 2xx response throws `GradientLabsSerializationException`
- Cancelling the token propagates `OperationCanceledException`
- External `HttpClient` is not disposed when the SDK client is disposed

### `SerializationTests`

- Every open-string enum type serialises each known value to its exact API string
- Every open-string enum type deserialises each known value
- Every open-string enum type preserves an unknown string value without throwing
- `ParticipantType` preserves exact casing and spaces (`AI Agent`)
- `ConversationChannel.Web` serialises to `"web"` (legacy value)
- `[JsonExtensionData]` captures unknown response fields
- Null request properties are omitted
- `NanosecondTimeSpanConverter` reads nanoseconds into ticks using `ns / 100`
- `NanosecondTimeSpanConverter` writes ticks as `Ticks * 100` using `checked`
- Duration conversion truncates sub-100ns values on read
- Duration conversion throws `JsonException` on write overflow

### `WebhookVerifierTests`

- Valid signature is accepted
- Multiple `v1` signatures: accepts when any one matches
- Wrong signing key rejected with `GradientLabsWebhookSignatureException`
- Missing signature header rejected
- Malformed header (no `t=`, no `v1=`, empty) rejected
- Timestamp too old (outside leeway) rejected
- Timestamp too far in future (outside leeway) rejected
- Verifies against raw body bytes — not reserialized JSON
- Uses `CryptographicOperations.FixedTimeEquals` for constant-time comparison
- Each of the 8 known event types deserialises correctly from a hardcoded JSON payload
- `X-GradientLabs-Token` present on `Token` when header is provided
- `X-GradientLabs-Token` is `null` when header is absent
- Unknown event type returns `UnknownWebhookEvent` without throwing
- `SequenceNumber` is `long`

### `PaginationTests`

- `Page<T>` with `next` set: `HasNextPage == true`, `HasPrevPage == false`
- `Page<T>` with no cursors: both `false`
- `[JsonExtensionData]` on `Page<T>` captures unknown envelope fields
- Manual iteration pattern requests second page using first page's `Next`

---

## 10. CI Plan

### `.github/workflows/ci.yml`

Triggers on `push` to `main` and `pull_request` targeting `main`.

Matrix: `os: [ubuntu-latest, windows-latest, macos-latest]` × `dotnet: ['8.0.x', '10.0.x']`
(validates the library is consumable from .NET 10 without breaking changes).

```yaml
name: CI
on:
  push:
    branches: [main]
  pull_request:

jobs:
  test:
    name: ${{ matrix.os }} / .NET ${{ matrix.dotnet }}
    runs-on: ${{ matrix.os }}
    strategy:
      fail-fast: false
      matrix:
        os: [ubuntu-latest, windows-latest, macos-latest]
        dotnet: ['8.0.x', '10.0.x']
    steps:
      - uses: actions/checkout@v4
        with:
          fetch-depth: 0       # required for SourceLink
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: ${{ matrix.dotnet }}
      - run: dotnet restore
      - run: dotnet format --verify-no-changes --no-restore
      - run: dotnet build --configuration Release --no-restore -warnaserror
      - run: dotnet test --configuration Release --no-build
```

Required status check name for branch protection: `test`.

### `.github/workflows/publish.yml`

Triggers on pushed tags matching `v*.*.*`. Derives version from the git tag.
`ContinuousIntegrationBuild=true` enables deterministic builds and SourceLink.

```yaml
name: Publish
on:
  push:
    tags: ['v*.*.*']

permissions:
  contents: read

jobs:
  publish:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
        with:
          fetch-depth: 0
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '10.0.x'
      - run: dotnet restore
      - run: dotnet test --configuration Release
      - name: Pack
        shell: bash
        run: |
          VERSION="${GITHUB_REF_NAME#v}"
          dotnet pack src/GradientLabs.Api/GradientLabs.Api.csproj \
            --configuration Release \
            --output artifacts \
            -p:PackageVersion="$VERSION" \
            -p:ContinuousIntegrationBuild=true
      - run: dotnet nuget push "artifacts/*.nupkg" --api-key "${{ secrets.NUGET_API_KEY }}" --source https://api.nuget.org/v3/index.json --skip-duplicate
```

---

## 11. Implementation Order

Implement in this sequence. Compile and type-check after each step.

1. **Project scaffolding** — solution, `.csproj`, `Directory.Build.props`,
   `Directory.Packages.props`, `global.json`, `.gitignore`, `LICENSE`, `CHANGELOG.md`
2. **HTTP pipeline** — `HttpPipeline`, `GradientLabsClientOptions`, full exception hierarchy
   (`GradientLabsException`, `GradientLabsApiException`, `GradientLabsErrorCode`,
   `GradientLabsRequestException`, `GradientLabsSerializationException`,
   `GradientLabsWebhookException`, `GradientLabsWebhookSignatureException`)
3. **Serialization** — `GradientLabsJsonOptions`, `NanosecondTimeSpanConverter`,
   `OpenStringConverter`, all open-string enum types in `StringEnums/`
4. **Webhook verifier** — `GradientLabsWebhookVerifier`, all 8 event types,
   `UnknownWebhookEvent`, `WebhookVerifyResult`
5. **Conversations** _(end-to-end first; validates the full HTTP pipeline)_
6. **BackOfficeTasks, Outbound, Voice** _(remaining integration-key groups)_
7. **Articles, Tools, Procedures** _(knowledge/config management)_
8. **ResourceTypes, ResourceSources** _(resource configuration)_
9. **HandOffTargets, TrafficGroups, Topics** _(routing/config)_
10. **Notes, Secrets, TerminologySubstitutions, IpAddresses** _(administrative)_
11. **`Page<T>` and pagination wiring** — cursor support on Procedures
12. **Tests** — `HttpPipelineTests`, `SerializationTests`, `WebhookVerifierTests`,
    `PaginationTests`; confirm all pass
13. **Integration smoke test** against local stack (see build skill instructions)
14. **Examples** — one per directory: conversations, tools, articles, webhooks, procedures,
    resources, back-office-tasks
15. **CI workflows** — `ci.yml`, `publish.yml`, `CODEOWNERS`, branch protection
16. **README** — installation, quick start, configuration, error handling, webhook verification

---

## Next Steps

Once this plan is reviewed and approved:

1. Create a new GitHub repo under `gradientlabs-ai` named **`gradientlabs-dotnet`**
2. Clone it locally, move `CSHARP_CLIENT_PLAN.md` into it, and commit to `main`
3. Run `/wearegradient-dev:gl-api-client-build <path-to-cloned-repo>` to build the client
