# Changelog

## [0.2.0] - 2026-11-25

Brings the library in line with the whole public API.

**Breaking.** Calls that no longer matched the API now do; some signatures changed to match.

| Was | Now |
|---|---|
| `Articles.UpsertAsync(supportPlatform, request)` sent `PUT /support-platforms/{platform}/articles/{id}` | `Articles.UpsertAsync(request)` sends `POST /articles` |
| `Articles.DeleteAsync(supportPlatform, articleId)` | `Articles.DeleteAsync(articleId)` sends `DELETE /articles/{id}` |
| `Articles.SetUsageStatusAsync(supportPlatform, articleId, request)` sent `PUT` | `Articles.SetUsageStatusAsync(articleId, request)` sends `POST /articles/{id}/usage-status` |
| `Conversations.ReadDeprecatedAsync(conversationId, customerId)` | `Conversations.ReadDeprecatedAsync(conversationId)`: the API takes no customer ID |
| `Notes.SetStatusAsync` returned `Task<Note>` | Returns `Task`: the API returns no body |
| `TrafficGroups.CreateTargetAsync` and `CreateExclusionAsync` returned `TrafficGroup` | Return the created `TrafficGroupTarget` |
| `ProcedureVersion.Author` was a `UserDetails` | A `string` |
| `ResourceTypeSourceConfig.Cache` was a `bool` | A duration `string` such as `"10m"`, or `"never"` |
| `Secret.RefreshMechanismHttp` and `WriteSecretRequest.RefreshMechanismHttp` were an `HttpDefinition` | A `SecretRefreshMechanismHttp` holding `RequestDefinition` and `ResponseParamName` |
| `CreateNoteRequest.Body` was a non-null `string` | `string?`: send exactly one of `Body` and `WebpageUrl` |
| `ExecuteToolRequest.Token` was a non-null `string` | `string?`, left out when null |
| `ActionExecuteEvent.Conversation` and `ResourcePullEvent.Conversation` were never null | Null when the event is for a back-office task, which is in the new `BackOfficeTask` |

Fixed requests and responses that never worked against the API:

- `HandOffTargets.UpsertAsync` now sends `POST`; the update methods on tools, resource types, resource
  sources, terminology substitutions and traffic groups now send `PUT`.
- `Voice.ReadLatestCallContextAsync` now calls `/voice/latest-call-context/{phone_number}`, and
  `ResourceSources.UpdateSchemaByExamplesAsync` calls `/resource-sources/{id}/schema-by-examples`.
- The list methods read the API's response envelopes, and `Procedures.ListAsync` reads its
  `pagination` cursors. Topics and procedure versions read the API's capitalised field names.
- `Procedures.SetLimitAsync` returns the updated `Procedure`; `Secrets.WriteAsync` returns the `Secret`.
- Back-office task webhook events carry the task in `BackOfficeTask`. Their `Conversation` and `Data`
  properties were never set and are now obsolete.

Added:

- `KnowledgeBases` on the Management client: `ListAsync`, `CreateAsync`, `ReadAsync`, `UpdateAsync`.
- `Files` on the Management client: `BeginUploadAsync`, `CreateAsync`, `ListAsync`.
- `Customers` on the Integration client: `ListMemoriesAsync`, `BatchCreateMemoriesAsync` (`POST
  /customers/{id}/memories/v2`) and the obsolete `BatchCreateMemoriesDeprecatedAsync`.
- `Notes.ListAsync` and `Notes.ReadAsync`.
- `Conversations.ReadAsync(conversationId, supportPlatform)` and `Tools.ReadAsync(toolId, version)` overloads.
- The `conversation.start` webhook event (`ConversationStartEvent`) and `ConversationStartWebhookResponse`
  to reply to it.
- Fields: `KnowledgeBaseIds` on articles and notes, `Attributes` on notes, `Ref` on hand-off targets,
  `BranchId`/`BranchName` on tool, resource type and resource source creation, `Managed` on resource
  sources, `TransferMode` on tool parameters, `Retry` on HTTP definitions, `ParameterSets` on tool
  updates, `LiveHandoffUrl` on voice call context, `LastCustomerMessageId` on agent messages, and
  `CustomerSource`, `Metadata` and `BackOfficeTask` on action and resource webhook events.
- Values: `ParameterType` `StringArray`, `Json`, `Duration`, `Attachment`; `ParameterTransferMode`;
  `SupportPlatform.Kustomer`; `CustomerSupportPlatformIdentifierType` `SalesforceContactEmail` and
  `SalesforceSuppliedEmail`.

## [0.1.2] - 2026-07-31

**Breaking.** `POST /outbound/conversations` has been removed from the API and replaced by one
endpoint per channel. `Outbound.StartConversationAsync` and `StartOutboundConversationRequest` are
gone; use the channel-specific methods instead.

| Was | Now |
|---|---|
| `StartConversationAsync` with `Channel = ConversationChannel.Web` | `StartChatConversationAsync(StartOutboundChatConversationRequest)` |
| `StartConversationAsync` with `Channel = ConversationChannel.Email` | `StartEmailConversationAsync(StartOutboundEmailConversationRequest)` |
| `StartConversationAsync` with `Channel = ConversationChannel.Voice` | `StartPhoneConversationAsync(StartOutboundPhoneConversationRequest)` |

- `CustomerSource` has no replacement and the type has been removed. `CustomerId` is now always your
  own identifier for the customer; link them to third-party platforms with
  `CustomerSupportPlatformIdentifiers` (Zendesk requires type `zendesk_support_user`, Salesforce
  `salesforce_contact_id`).
- `SupportPlatform` is required on chat and email — the highest-priority platform is no longer
  selected for you.
- Phone takes `ToPhoneNumber` and `FromPhoneNumber` and has no support platform field.

## [0.1.1] - 2026-07-17

Add `Conversations.DeleteAsync` (DELETE `/conversations/{id}`) and
`BackOfficeTasks.DeleteAsync` (DELETE `/back-office-tasks/{id}`) on the Management client.

## [0.1.0] - 2026-06-22

Initial release.
