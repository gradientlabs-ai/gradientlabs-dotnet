using System.Net;
using System.Text.Json;
using FluentAssertions;
using GradientLabs.Api.Tests.Fakes;
using Xunit;

namespace GradientLabs.Api.Tests;

public class EndpointRoutingTests
{
    private static readonly Uri BaseUri = new("https://localhost:9999");

    private static GradientLabsManagementClient Management(HttpMessageHandler handler)
        => new(new GradientLabsClientOptions { ApiKey = "test_key", HttpMessageHandler = handler, BaseUri = BaseUri });

    private static GradientLabsIntegrationClient Integration(HttpMessageHandler handler)
        => new(new GradientLabsClientOptions { ApiKey = "test_key", HttpMessageHandler = handler, BaseUri = BaseUri });

    public static TheoryData<string, Func<GradientLabsManagementClient, Task>, string, string> ManagementCases() => new()
    {
        { "ArticleUpsert", c => c.Articles.UpsertAsync(new UpsertArticleRequest { Id = "a1" }), "POST", "/articles" },
        { "ArticleDelete", c => c.Articles.DeleteAsync("a 1"), "DELETE", "/articles/a%201" },
        { "ArticleSetUsageStatus", c => c.Articles.SetUsageStatusAsync("a1", new SetArticleUsageStatusRequest { UsageStatus = ArticleUsageStatus.Off }), "POST", "/articles/a1/usage-status" },
        { "HandOffTargetUpsert", c => c.HandOffTargets.UpsertAsync(new UpsertHandOffTargetRequest { Id = "t1", Name = "T" }), "POST", "/hand-off-targets" },
        { "ResourceSourceUpdate", c => c.ResourceSources.UpdateAsync("rs1", new UpdateResourceSourceRequest()), "PUT", "/resource-sources/rs1" },
        { "UpdateSchemaByExamples", c => c.ResourceSources.UpdateSchemaByExamplesAsync("rs1", new UpdateSchemaByExamplesRequest()), "POST", "/resource-sources/rs1/schema-by-examples" },
        { "ResourceTypeUpdate", c => c.ResourceTypes.UpdateAsync("rt1", new UpdateResourceTypeRequest()), "PUT", "/resource-types/rt1" },
        { "TerminologySubstitutionUpdate", c => c.TerminologySubstitutions.UpdateAsync("ts1", new UpdateTerminologySubstitutionRequest()), "PUT", "/terminology-substitutions/ts1" },
        { "ToolUpdate", c => c.Tools.UpdateAsync("tool1", new UpdateToolRequest()), "PUT", "/tools/tool1" },
        { "ToolReadVersion", c => c.Tools.ReadAsync("tool1", 3), "GET", "/tools/tool1?version=3" },
        { "TrafficGroupUpdate", c => c.TrafficGroups.UpdateAsync("g1", new UpdateTrafficGroupRequest { Name = "G" }), "PUT", "/traffic-groups/g1" },
        { "NoteRead", c => c.Notes.ReadAsync("n1"), "GET", "/notes/n1" },
        { "NoteList", c => c.Notes.ListAsync("cur"), "GET", "/notes?cursor=cur" },
        { "NoteSetStatus", c => c.Notes.SetStatusAsync("n1", new SetNoteStatusRequest { Status = NoteStatus.Live }), "POST", "/notes/n1/status" },
        { "KnowledgeBaseCreate", c => c.KnowledgeBases.CreateAsync(new CreateKnowledgeBaseRequest { Id = "kb", Name = "KB" }), "POST", "/knowledge-bases" },
        { "KnowledgeBaseRead", c => c.KnowledgeBases.ReadAsync("kb 1"), "GET", "/knowledge-bases/kb%201" },
        { "KnowledgeBaseUpdate", c => c.KnowledgeBases.UpdateAsync("kb1", new UpdateKnowledgeBaseRequest { Name = "KB" }), "POST", "/knowledge-bases/kb1" },
        { "FileUploadBegin", c => c.Files.BeginUploadAsync(), "POST", "/file-uploads" },
        { "FileCreate", c => c.Files.CreateAsync(new CreateFileRequest { UploadId = "u1", FileName = "a.png" }), "POST", "/files" },
        { "SecretWrite", c => c.Secrets.WriteAsync("api key", new WriteSecretRequest { Value = "v" }), "PUT", "/secrets/api%20key" },
    };

    [Theory]
    [MemberData(nameof(ManagementCases))]
    public async Task ManagementOperation_UsesContractMethodAndPath(string operation, Func<GradientLabsManagementClient, Task> call, string method, string pathAndQuery)
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{}");
        using var client = Management(handler);

        await call(client);

        handler.LastRequest!.Method.Method.Should().Be(method, operation);
        handler.LastRequest.RequestUri!.PathAndQuery.Should().Be(pathAndQuery, operation);
    }

    public static TheoryData<string, Func<GradientLabsIntegrationClient, Task>, string, string> IntegrationCases() => new()
    {
        { "VoiceCallContextRead", c => c.Voice.ReadLatestCallContextAsync("+447700900123", lookbackSeconds: 120, includeLargeFields: true), "GET", "/voice/latest-call-context/%2B447700900123?lookback_seconds=120&include_large_fields=true" },
        { "ConversationRead", c => c.Conversations.ReadAsync("c1"), "GET", "/conversations/c1/read" },
        { "ConversationReadSupportPlatform", c => c.Conversations.ReadAsync("c1", "intercom"), "GET", "/conversations/c1/read?support_platform=intercom" },
#pragma warning disable CS0618
        { "ConversationReadDeprecated", c => c.Conversations.ReadDeprecatedAsync("c1"), "GET", "/conversations/c1" },
        { "CustomerMemoriesBatchCreate", c => c.Customers.BatchCreateMemoriesDeprecatedAsync("cust1", new BatchCreateMemoriesDeprecatedRequest()), "POST", "/customers/cust1/memories" },
#pragma warning restore CS0618
        { "CustomerMemoriesBatchCreateV2", c => c.Customers.BatchCreateMemoriesAsync("cust 1", new BatchCreateMemoriesRequest()), "POST", "/customers/cust%201/memories/v2" },
    };

    [Theory]
    [MemberData(nameof(IntegrationCases))]
    public async Task IntegrationOperation_UsesContractMethodAndPath(string operation, Func<GradientLabsIntegrationClient, Task> call, string method, string pathAndQuery)
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, """{"started_at":"2024-01-01T00:00:00Z","id":"c1","customer_id":"u1","channel":"web","status":"active","agent_is_active":true,"created":"2024-01-01T00:00:00Z","updated":"2024-01-01T00:00:00Z"}""");
        using var client = Integration(handler);

        await call(client);

        handler.LastRequest!.Method.Method.Should().Be(method, operation);
        handler.LastRequest.RequestUri!.PathAndQuery.Should().Be(pathAndQuery, operation);
    }

    [Fact]
    public async Task ListEndpoints_UnwrapTheirEnvelopes()
    {
        var handler = new FakeHttpMessageHandler();
        using var client = Management(handler);

        handler.SetResponse(HttpStatusCode.OK, """{"targets":[{"id":"t1","name":"Billing","ref":"billing-team"}]}""");
        (await client.HandOffTargets.ListAsync()).Single().Ref.Should().Be("billing-team");

        handler.SetResponse(HttpStatusCode.OK, """{"tools":[{"id":"tool1","name":"n","description":"d","parameters":[],"branch_name":"b"}]}""");
        (await client.Tools.ListAsync()).Single().BranchName.Should().Be("b");

        handler.SetResponse(HttpStatusCode.OK, """{"resource_sources":[{"id":"rs1","display_name":"S","source_type":"internal","created":"2024-01-01T00:00:00Z","updated":"2024-01-01T00:00:00Z","available_refresh_strategies":["dynamic"],"available_scopes":["global"],"managed":{"enabled":true,"key":"intercom.contact"}}]}""");
        (await client.ResourceSources.ListAsync()).Single().Managed!.Key.Should().Be("intercom.contact");

        handler.SetResponse(HttpStatusCode.OK, """{"resource_types":[{"id":"rt1","display_name":"T","scope":"global","refresh_strategy":"static","is_enabled":true,"created":"2024-01-01T00:00:00Z","updated":"2024-01-01T00:00:00Z","source_config":{"source_id":"rs1","attributes":["a"],"cache":"10m"}}]}""");
        (await client.ResourceTypes.ListAsync()).Single().SourceConfig!.Cache.Should().Be("10m");

        handler.SetResponse(HttpStatusCode.OK, """{"secrets":[{"name":"s1","created":"2024-01-01T00:00:00Z","updated":"2024-01-01T00:00:00Z","refresh_mechanism_http":{"request_definition":{"method":"POST","url_template":"https://auth.example.com/token","retry":{"request_timeout_seconds":30}},"response_param_name":"access_token"}}]}""");
        var secret = (await client.Secrets.ListAsync()).Single();
        secret.RefreshMechanismHttp!.ResponseParamName.Should().Be("access_token");
        secret.RefreshMechanismHttp.RequestDefinition.Retry!.RequestTimeoutSeconds.Should().Be(30);

        handler.SetResponse(HttpStatusCode.OK, """{"substitutions":[{"id":"ts1","blocked":"b","blocked_description":"bd","replacement":"r","created":"2024-01-01T00:00:00Z","updated":"2024-01-01T00:00:00Z"}]}""");
        (await client.TerminologySubstitutions.ListAsync()).Single().Id.Should().Be("ts1");

        handler.SetResponse(HttpStatusCode.OK, """{"traffic_groups":[{"id":"g1","name":"G","targets":[],"excluded_targets":[]}]}""");
        (await client.TrafficGroups.ListAsync()).Single().Id.Should().Be("g1");

        handler.SetResponse(HttpStatusCode.OK, """{"knowledge_bases":[{"id":"help-centre","gradient_labs_id":"kb_1","name":"Help","created":"2024-01-01T00:00:00Z","updated":"2024-01-01T00:00:00Z"}]}""");
        (await client.KnowledgeBases.ListAsync()).Single().GradientLabsId.Should().Be("kb_1");
    }

    [Fact]
    public async Task PagedLists_MapPaginationCursors()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, """{"notes":[{"gradient_labs_id":"note_1","id":"n1","title":"T","status":"live","last_modified_by":"me","created":"2024-01-01T00:00:00Z","updated":"2024-01-01T00:00:00Z","attributes":{"Locale":["UK"]}}],"pagination":{"next":"next_cursor"}}""");
        using var client = Management(handler);

        var notes = await client.Notes.ListAsync();
        notes.Items.Single().Attributes!["Locale"].Should().Equal("UK");
        notes.Next.Should().Be("next_cursor");
        notes.HasPrevPage.Should().BeFalse();

        handler.SetResponse(HttpStatusCode.OK, """{"files":[{"id":"file_1","ref":"id-card-front","file_name":"id-card-front.png","content_type":"image/png","size_bytes":1024,"created":"2024-01-01T00:00:00Z"}],"pagination":{}}""");
        var files = await client.Files.ListAsync(cursor: "c2");
        files.Items.Single().SizeBytes.Should().Be(1024);
        files.HasNextPage.Should().BeFalse();
        handler.LastRequest!.RequestUri!.PathAndQuery.Should().Be("/files?cursor=c2");
    }

    [Fact]
    public async Task Procedures_VersionsAndLimitUseContractShapes()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, """{"Versions":[{"Name":"V1","Description":"d","Version":2,"Author":"jo@example.com","Created":"2024-01-01T00:00:00Z","Gated":true,"GatedConfig":{"MaxDailyConversations":50},"Live":false}]}""");
        using var client = Management(handler);

        var version = (await client.Procedures.ListVersionsAsync("proc_1")).Single();
        version.Version.Should().Be(2);
        version.Author.Should().Be("jo@example.com");
        version.GatedConfig!.MaxDailyConversations.Should().Be(50);

        handler.SetResponse(HttpStatusCode.OK, """{"procedure":{"id":"proc_1","name":"P","description":"d","status":"live","has_daily_limit":true,"max_daily_conversations":100,"author":{"email":"jo@example.com"},"created":"2024-01-01T00:00:00Z","updated":"2024-01-01T00:00:00Z"}}""");
        var procedure = await client.Procedures.SetLimitAsync("proc_1", new SetProcedureLimitRequest { HasDailyLimit = true, MaxDailyConversations = 100 });
        procedure.MaxDailyConversations.Should().Be(100);
        handler.LastRequest!.RequestUri!.AbsolutePath.Should().Be("/procedure/proc_1/limit");
    }

    [Fact]
    public async Task Topics_ReadPascalCaseFields()
    {
        var topic = """{"Source":"public-api","ExternalID":"t1","Name":"Billing","Visibility":"public","ParentExternalID":"root","Created":"2024-01-01T00:00:00Z","LastEdited":"2024-01-02T00:00:00Z","LastSeen":"2024-01-03T00:00:00Z","Data":"e30=","PublicURL":"https://help.example.com/t1"}""";
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Topics\":[" + topic + "]}");
        using var client = Management(handler);

        var listed = (await client.Topics.ListAsync()).Single();
        listed.ExternalId.Should().Be("t1");
        listed.ParentExternalId.Should().Be("root");
        listed.PublicUrl.Should().Be("https://help.example.com/t1");
        listed.Visibility.Should().Be(ArticleVisibility.Public);

        handler.SetResponse(HttpStatusCode.OK, topic);
        var read = await client.Topics.ReadAsync("t1");
        read.Name.Should().Be("Billing");
        handler.LastRequest!.RequestUri!.AbsolutePath.Should().Be("/topic/t1");
    }

    [Fact]
    public async Task TrafficGroupTargetAndExclusion_ReturnTheTarget()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, """{"target_type":"procedure","target_id":"proc_1"}""");
        using var client = Management(handler);

        var target = await client.TrafficGroups.CreateTargetAsync("g1", new CreateTrafficGroupTargetRequest { TargetType = "procedure", TargetId = "proc_1" });
        target.TargetId.Should().Be("proc_1");
        handler.LastRequest!.RequestUri!.AbsolutePath.Should().Be("/traffic-groups/g1/targets");

        var exclusion = await client.TrafficGroups.CreateExclusionAsync("g1", new CreateTrafficGroupExclusionRequest { TargetType = "procedure", TargetId = "proc_1" });
        exclusion.TargetType.Should().Be("procedure");
        handler.LastRequest!.RequestUri!.AbsolutePath.Should().Be("/traffic-groups/g1/exclusions");
    }

    [Fact]
    public async Task FileUploadBegin_ReadsUploadInstructions()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, """{"upload_id":"u1","upload_url":"https://storage.example.com/u1","headers":{"Content-Type":"image/png"},"max_bytes":5242880,"content_type":"image/png","expires_at":"2024-01-15T10:45:00Z"}""");
        using var client = Management(handler);

        var upload = await client.Files.BeginUploadAsync(new BeginFileUploadRequest { ContentType = "image/png" });

        upload.UploadId.Should().Be("u1");
        upload.Headers["Content-Type"].Should().Be("image/png");
        upload.MaxBytes.Should().Be(5242880);
        handler.LastRequestBody.Should().Be("""{"content_type":"image/png"}""");
    }

    [Fact]
    public async Task AddMessage_ReadsTheStoredMessage()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, """{"id":"m1","body":"Hi","participant_id":"p1","participant_type":"Customer"}""");
        using var client = Integration(handler);

        var message = await client.Conversations.AddMessageAsync("c1", new AddMessageRequest
        {
            Id = "m1",
            Body = "Hi",
            ParticipantId = "p1",
            ParticipantType = ParticipantType.Customer,
            Attachments = [new Attachment { Type = AttachmentType.File, FileName = "receipt.txt", Description = "Receipt" }],
        });

        message.Id.Should().Be("m1");
        message.ParticipantType.Should().Be(ParticipantType.Customer);
        using var request = JsonDocument.Parse(handler.LastRequestBody!);
        request.RootElement.GetProperty("attachments")[0].TryGetProperty("url", out _).Should().BeFalse();
    }

    [Fact]
    public async Task CreateBackOfficeTask_SendsInlineAttachmentContents()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, """{"id":"task1"}""");
        using var client = Integration(handler);

        await client.BackOfficeTasks.CreateAsync(new CreateBackOfficeTaskRequest
        {
            Id = "task1",
            AgentId = "agent_1",
            ProcedureId = "proc_1",
            Input = null,
            Attachments = [new BackOfficeTaskAttachment { FileName = "note.txt", Base64Contents = "aGk=" }],
        });

        using var request = JsonDocument.Parse(handler.LastRequestBody!);
        request.RootElement.GetProperty("input").ValueKind.Should().Be(JsonValueKind.Null);
        request.RootElement.GetProperty("attachments")[0].GetProperty("base_64_contents").GetString().Should().Be("aGk=");
    }

    [Fact]
    public async Task ReturnAsyncToolResult_SendsNullPayload()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{}");
        using var client = Integration(handler);

        await client.Conversations.ReturnAsyncToolResultAsync("c1", new ReturnAsyncToolResultRequest
        {
            AsyncToolExecutionId = "execution_1",
            Payload = null,
        });

        using var request = JsonDocument.Parse(handler.LastRequestBody!);
        request.RootElement.GetProperty("payload").ValueKind.Should().Be(JsonValueKind.Null);
    }

    [Fact]
    public async Task RequestBodies_CarryNewFields()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, """{"id":"x","gradient_labs_id":"g","title":"t","status":"draft","last_modified_by":"me","created":"2024-01-01T00:00:00Z","updated":"2024-01-01T00:00:00Z","name":"n","description":"d","parameters":[],"display_name":"d","scope":"global","refresh_strategy":"static","is_enabled":true}""");
        using var client = Management(handler);

        await client.Notes.CreateAsync(new CreateNoteRequest
        {
            Id = "n1",
            Title = "T",
            WebpageUrl = "https://example.com",
            Attributes = new Dictionary<string, IReadOnlyList<string>> { ["Locale"] = ["UK"] },
            KnowledgeBaseIds = ["help-centre"],
        });
        using (var note = JsonDocument.Parse(handler.LastRequestBody!))
        {
            note.RootElement.TryGetProperty("body", out _).Should().BeFalse();
            note.RootElement.GetProperty("attributes").GetProperty("Locale")[0].GetString().Should().Be("UK");
            note.RootElement.GetProperty("knowledge_base_ids")[0].GetString().Should().Be("help-centre");
        }

        await client.Articles.UpsertAsync(new UpsertArticleRequest { Id = "a1", KnowledgeBaseIds = ["help-centre"] });
        handler.LastRequestBody.Should().Contain("\"knowledge_base_ids\":[\"help-centre\"]");

        await client.HandOffTargets.UpsertAsync(new UpsertHandOffTargetRequest { Id = "t1", Name = "T", Ref = "billing-team" });
        handler.LastRequestBody.Should().Contain("\"ref\":\"billing-team\"");

        await client.Tools.CreateAsync(new CreateToolRequest
        {
            BranchName = "copy-tools",
            Parameters = [new ToolParameter { Name = "doc", Type = ParameterType.Attachment, TransferMode = ParameterTransferMode.TemporaryUrl }],
            Http = new HttpDefinition { Method = "GET", UrlTemplate = "https://example.com", Retry = new HttpRetryConfig { MaxAttempts = 3, RequestTimeoutSeconds = 10 } },
        });
        using (var tool = JsonDocument.Parse(handler.LastRequestBody!))
        {
            tool.RootElement.TryGetProperty("id", out _).Should().BeFalse();
            tool.RootElement.GetProperty("branch_name").GetString().Should().Be("copy-tools");
            tool.RootElement.GetProperty("parameters")[0].GetProperty("transfer_mode").GetString().Should().Be("temporary_url");
            tool.RootElement.GetProperty("parameters")[0].GetProperty("type").GetString().Should().Be("attachment");
            tool.RootElement.GetProperty("http").GetProperty("retry").GetProperty("request_timeout_seconds").GetInt64().Should().Be(10);
        }

        await client.Tools.ExecuteAsync("tool1", new ExecuteToolRequest { Arguments = [new ToolArgument { Name = "a", Value = "b" }] });
        handler.LastRequestBody.Should().NotContain("token");

        await client.ResourceTypes.CreateAsync(new CreateResourceTypeRequest
        {
            DisplayName = "T",
            BranchId = "branch_1",
            SourceConfig = new ResourceTypeSourceConfig { SourceId = "rs1", Attributes = ["a"], Cache = "never" },
        });
        using (var rt = JsonDocument.Parse(handler.LastRequestBody!))
        {
            rt.RootElement.GetProperty("branch_id").GetString().Should().Be("branch_1");
            rt.RootElement.GetProperty("source_config").GetProperty("cache").GetString().Should().Be("never");
        }
    }

    [Fact]
    public async Task VoiceCallContext_ReadsLiveHandoffUrl()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, """{"started_at":"2024-01-01T00:00:00Z","live_handoff_url":"https://app.gradient-labs.ai/acme/main/live-handoffs/+447700900123"}""");
        using var client = Integration(handler);

        var context = await client.Voice.ReadLatestCallContextAsync("+447700900123");

        context.LiveHandoffUrl.Should().Be("https://app.gradient-labs.ai/acme/main/live-handoffs/+447700900123");
    }
}
