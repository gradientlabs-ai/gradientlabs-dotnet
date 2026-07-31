# Changelog

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
