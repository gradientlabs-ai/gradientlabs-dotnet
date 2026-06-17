# Changelog

## [Unreleased]

## [0.1.0] - Unreleased

### Fixed

- `ConversationsClient`: correct HTTP methods (`PUT` for cancel, finish, resume, rate, assign, return-async-tool-result) and paths (`/assignee`, `/read`, `/return-async-tool-result`)
- `BackOfficeTasksClient`: read path corrected to `/:id/read`
- `ProceduresClient`: read and set-limit use singular `/procedure/:id`; live/gated lifecycle actions use `POST`
- `NotesClient`: removed spurious `supportPlatform` parameter; all mutations use `POST /notes`
- `TopicsClient`: read uses singular `/topic/:id`; upsert uses `POST /topics`; removed spurious `supportPlatform` parameter
- `HttpPipeline`: `PUT`/`POST`/`PATCH` requests with no body now send `{}` (platform rejects bodyless mutations)
