# Changelog

## [0.2.0] - 2026-07-22

Add `Customers.BatchCreateMemoriesAsync` (POST `/customers/{id}/memories`) for
batch-creating customer-scoped memories the agent can search over on demand. The
call is asynchronous (fire-and-forget) and returns no body; it may return `409
Conflict` when a batch is already being created for the same customer.

## [0.1.1] - 2026-07-17

Add `Conversations.DeleteAsync` (DELETE `/conversations/{id}`) and
`BackOfficeTasks.DeleteAsync` (DELETE `/back-office-tasks/{id}`) on the Management client.

## [0.1.0] - 2026-06-22

Initial release.
