using System.Net;
using FluentAssertions;
using GradientLabs.Api.Tests.Fakes;
using Xunit;

namespace GradientLabs.Api.Tests;

public class HttpPipelineTests
{
    private static GradientLabsIntegrationClient CreateClient(HttpMessageHandler handler)
        => new(new GradientLabsClientOptions
        {
            ApiKey = "test_key_abc123",
            HttpMessageHandler = handler,
            BaseUri = new Uri("https://localhost:9999"),
        });

    [Fact]
    public async Task AuthHeader_IsPresentOnEveryRequest()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, """{"id":"c1","customer_id":"u1","status":"open","agent_is_active":false,"channel":"web","created":"2024-01-01T00:00:00Z","updated":"2024-01-01T00:00:00Z"}""");
        using var client = CreateClient(handler);

        await client.Conversations.ReadAsync("c1");

        handler.LastRequest!.Headers.Authorization!.Scheme.Should().Be("Bearer");
        handler.LastRequest.Headers.Authorization.Parameter.Should().Be("test_key_abc123");
    }

    [Fact]
    public async Task UserAgent_MatchesExpectedFormat()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, """{"id":"c1","customer_id":"u1","status":"open","agent_is_active":false,"channel":"web","created":"2024-01-01T00:00:00Z","updated":"2024-01-01T00:00:00Z"}""");
        using var client = new GradientLabsIntegrationClient(new GradientLabsClientOptions
        {
            ApiKey = "test_key",
            HttpMessageHandler = handler,
            BaseUri = new Uri("https://localhost:9999"),
            UserAgentVersion = "1.2.3",
        });

        await client.Conversations.ReadAsync("c1");

        var ua = handler.LastRequest!.Headers.UserAgent.ToString();
        ua.Should().StartWith("Gradient-Labs-CSharp/1.2.3");
        ua.Should().Contain(".NET");
    }

    [Fact]
    public async Task ContentType_IsPresentWhenBodyExists()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, """{"id":"c1","customer_id":"u1","status":"open","agent_is_active":false,"channel":"web","created":"2024-01-01T00:00:00Z","updated":"2024-01-01T00:00:00Z"}""");
        using var client = CreateClient(handler);

        await client.Conversations.StartAsync(new StartConversationRequest
        {
            Id = "c1",
            CustomerId = "u1",
            Channel = ConversationChannel.Web,
        });

        handler.LastRequest!.Content!.Headers.ContentType!.MediaType.Should().Be("application/json");
    }

    [Fact]
    public async Task ContentType_IsAbsentForGet()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, """{"id":"c1","customer_id":"u1","status":"open","agent_is_active":false,"channel":"web","created":"2024-01-01T00:00:00Z","updated":"2024-01-01T00:00:00Z"}""");
        using var client = CreateClient(handler);

        await client.Conversations.ReadAsync("c1");

        handler.LastRequest!.Content.Should().BeNull();
    }

    [Fact]
    public async Task AcceptHeader_IsAlwaysPresent()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, """{"id":"c1","customer_id":"u1","status":"open","agent_is_active":false,"channel":"web","created":"2024-01-01T00:00:00Z","updated":"2024-01-01T00:00:00Z"}""");
        using var client = CreateClient(handler);

        await client.Conversations.ReadAsync("c1");

        handler.LastRequest!.Headers.Accept.Should().Contain(h => h.MediaType == "application/json");
    }

    [Fact]
    public async Task Non2xx_ThrowsGradientLabsApiExceptionWithCorrectFields()
    {
        var errorJson = """{"code":"not_found","message":"Conversation not found","details":{"trace_id":"trace-abc"}}""";
        var handler = new FakeHttpMessageHandler(HttpStatusCode.NotFound, errorJson);
        using var client = CreateClient(handler);

        var act = () => client.Conversations.ReadAsync("missing");

        var ex = await act.Should().ThrowAsync<GradientLabsApiException>();
        ex.Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
        ex.Which.ErrorCode.Should().Be(GradientLabsErrorCode.NotFound);
        ex.Which.ApiMessage.Should().Be("Conversation not found");
        ex.Which.TraceId.Should().Be("trace-abc");
        ex.Which.ResponseBody.Should().Contain("not_found");
    }

    [Theory]
    [InlineData("unavailable", true)]
    [InlineData("deadline_exceeded", true)]
    [InlineData("aborted", true)]
    [InlineData("unknown", true)]
    [InlineData("internal", true)]
    [InlineData("not_found", false)]
    [InlineData("unauthenticated", false)]
    [InlineData("permission_denied", false)]
    [InlineData("invalid_argument", false)]
    [InlineData("failed_precondition", false)]
    [InlineData("resource_exhausted", false)]
    [InlineData("already_exists", false)]
    public async Task IsRetryable_IsCorrectForEachErrorCode(string code, bool expected)
    {
        var errorJson = $$$"""{"code":"{{{code}}}","message":"err"}""";
        var handler = new FakeHttpMessageHandler(HttpStatusCode.InternalServerError, errorJson);
        using var client = CreateClient(handler);

        var act = () => client.Conversations.ReadAsync("x");

        var ex = await act.Should().ThrowAsync<GradientLabsApiException>();
        ex.Which.IsRetryable.Should().Be(expected);
    }

    [Fact]
    public async Task Non2xx_WithNonJsonBody_StillThrowsGradientLabsApiException()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.BadGateway, "Bad Gateway", "text/plain");
        using var client = CreateClient(handler);

        var act = () => client.Conversations.ReadAsync("x");

        await act.Should().ThrowAsync<GradientLabsApiException>();
    }

    [Fact]
    public async Task TransportFailure_ThrowsGradientLabsRequestException()
    {
        var handler = new ThrowingHttpMessageHandler(new HttpRequestException("Connection refused"));
        using var client = CreateClient(handler);

        var act = () => client.Conversations.ReadAsync("x");

        await act.Should().ThrowAsync<GradientLabsRequestException>();
    }

    [Fact]
    public async Task InvalidJsonOn2xx_ThrowsGradientLabsSerializationException()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, "this is not json");
        using var client = CreateClient(handler);

        var act = () => client.Conversations.ReadAsync("x");

        await act.Should().ThrowAsync<GradientLabsSerializationException>();
    }

    [Fact]
    public async Task CancellationToken_PropagatesOperationCanceledException()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var handler = new FakeHttpMessageHandler();
        using var client = CreateClient(handler);

        var act = () => client.Conversations.ReadAsync("x", cts.Token);

        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public void ExternalHttpClient_IsNotDisposedWhenClientIsDisposed()
    {
        var handler = new FakeHttpMessageHandler();
        var httpClient = new HttpClient(handler);

        var client = new GradientLabsIntegrationClient(new GradientLabsClientOptions
        {
            ApiKey = "test_key",
            HttpClient = httpClient,
        });

        client.Dispose();

        handler.WasDisposed.Should().BeFalse();
        httpClient.Dispose();
    }
}
