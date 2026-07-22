using System.Net;
using System.Text.Json;
using FluentAssertions;
using GradientLabs.Api.Tests.Fakes;
using Xunit;

namespace GradientLabs.Api.Tests;

public class MemoriesTests
{
    private static GradientLabsIntegrationClient CreateClient(HttpMessageHandler handler)
        => new(new GradientLabsClientOptions
        {
            ApiKey = "test_key_abc123",
            HttpMessageHandler = handler,
            BaseUri = new Uri("https://localhost:9999"),
        });

    [Fact]
    public async Task BulkUploadMemories_SendsPostToExpectedPathWithSnakeCaseBody()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, """{"upload_id":"upl_1","memories_inserted":2}""");
        using var client = CreateClient(handler);

        await client.Conversations.BulkUploadMemoriesAsync("conv 1", new BulkUploadMemoriesRequest
        {
            IdempotencyKey = "key-1",
            Memories = new[]
            {
                JsonDocument.Parse("""{"kind":"order","id":"o1"}""").RootElement,
                JsonDocument.Parse("""{"kind":"note","text":"hello"}""").RootElement,
            },
            CreatedAtKeys = new[] { "created_at", "timestamp" },
        });

        handler.LastRequest!.Method.Should().Be(HttpMethod.Post);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/conversations/conv%201/memories");

        using var parsed = JsonDocument.Parse(handler.LastRequestBody!);
        var root = parsed.RootElement;
        root.GetProperty("idempotency_key").GetString().Should().Be("key-1");
        root.GetProperty("memories").GetArrayLength().Should().Be(2);
        root.GetProperty("memories")[0].GetProperty("kind").GetString().Should().Be("order");
        root.GetProperty("created_at_keys").EnumerateArray().Select(e => e.GetString())
            .Should().Equal("created_at", "timestamp");
    }

    [Fact]
    public async Task BulkUploadMemories_DeserializesResponse()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, """{"upload_id":"upl_42","memories_inserted":7}""");
        using var client = CreateClient(handler);

        var response = await client.Conversations.BulkUploadMemoriesAsync("c1", new BulkUploadMemoriesRequest
        {
            IdempotencyKey = "key-1",
            Memories = new[] { JsonDocument.Parse("""{"a":1}""").RootElement },
        });

        response.UploadId.Should().Be("upl_42");
        response.MemoriesInserted.Should().Be(7);
    }

    [Fact]
    public async Task BulkUploadMemories_OmitsCreatedAtKeysWhenNull()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, """{"upload_id":"upl_1","memories_inserted":1}""");
        using var client = CreateClient(handler);

        await client.Conversations.BulkUploadMemoriesAsync("c1", new BulkUploadMemoriesRequest
        {
            IdempotencyKey = "key-1",
            Memories = new[] { JsonDocument.Parse("""{"a":1}""").RootElement },
        });

        using var parsed = JsonDocument.Parse(handler.LastRequestBody!);
        parsed.RootElement.TryGetProperty("created_at_keys", out _).Should().BeFalse();
    }

    [Fact]
    public async Task BulkUploadMemories_PropagatesApiError()
    {
        var errorJson = """{"code":"invalid_argument","message":"missing idempotency key"}""";
        var handler = new FakeHttpMessageHandler(HttpStatusCode.BadRequest, errorJson);
        using var client = CreateClient(handler);

        var act = () => client.Conversations.BulkUploadMemoriesAsync("c1", new BulkUploadMemoriesRequest
        {
            IdempotencyKey = "key-1",
            Memories = new[] { JsonDocument.Parse("""{"a":1}""").RootElement },
        });

        var ex = await act.Should().ThrowAsync<GradientLabsApiException>();
        ex.Which.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        ex.Which.ErrorCode.Should().Be(GradientLabsErrorCode.InvalidArgument);
    }
}
