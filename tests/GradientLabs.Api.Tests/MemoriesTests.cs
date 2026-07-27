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

    private static Memory NewMemory(string externalId, string? customType = null)
        => new()
        {
            ExternalId = externalId,
            CustomType = customType,
            CreatedAt = new DateTimeOffset(2026, 7, 1, 10, 0, 0, TimeSpan.Zero),
            Data = JsonDocument.Parse("""{"any":"json object"}""").RootElement,
        };

    [Fact]
    public async Task BatchCreateMemories_SendsPostToExpectedPathWithSnakeCaseBody()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.Accepted);
        using var client = CreateClient(handler);

        await client.Customers.BatchCreateMemoriesAsync("cust 1", new BatchCreateMemoriesRequest
        {
            Memories = new[]
            {
                NewMemory("order_123", "order"),
                NewMemory("note_456"),
            },
        });

        handler.LastRequest!.Method.Should().Be(HttpMethod.Post);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/customers/cust%201/memories");

        using var parsed = JsonDocument.Parse(handler.LastRequestBody!);
        var memories = parsed.RootElement.GetProperty("memories");
        memories.GetArrayLength().Should().Be(2);

        var first = memories[0];
        first.GetProperty("external_id").GetString().Should().Be("order_123");
        first.GetProperty("custom_type").GetString().Should().Be("order");
        first.GetProperty("created_at").GetString().Should().Be("2026-07-01T10:00:00+00:00");
        first.GetProperty("data").GetProperty("any").GetString().Should().Be("json object");
    }

    [Fact]
    public async Task BatchCreateMemories_OmitsCustomTypeWhenNull()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.Accepted);
        using var client = CreateClient(handler);

        await client.Customers.BatchCreateMemoriesAsync("c1", new BatchCreateMemoriesRequest
        {
            Memories = new[] { NewMemory("note_456") },
        });

        using var parsed = JsonDocument.Parse(handler.LastRequestBody!);
        parsed.RootElement.GetProperty("memories")[0].TryGetProperty("custom_type", out _).Should().BeFalse();
    }

    [Fact]
    public async Task BatchCreateMemories_PropagatesApiError()
    {
        var errorJson = """{"code":"invalid_argument","message":"memories must not be empty"}""";
        var handler = new FakeHttpMessageHandler(HttpStatusCode.BadRequest, errorJson);
        using var client = CreateClient(handler);

        var act = () => client.Customers.BatchCreateMemoriesAsync("c1", new BatchCreateMemoriesRequest
        {
            Memories = new[] { NewMemory("order_123") },
        });

        var ex = await act.Should().ThrowAsync<GradientLabsApiException>();
        ex.Which.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        ex.Which.ErrorCode.Should().Be(GradientLabsErrorCode.InvalidArgument);
    }

    [Fact]
    public async Task BatchCreateMemories_PropagatesConflict()
    {
        var errorJson = """{"code":"conflict","message":"a batch is already being created for this customer"}""";
        var handler = new FakeHttpMessageHandler(HttpStatusCode.Conflict, errorJson);
        using var client = CreateClient(handler);

        var act = () => client.Customers.BatchCreateMemoriesAsync("c1", new BatchCreateMemoriesRequest
        {
            Memories = new[] { NewMemory("order_123") },
        });

        var ex = await act.Should().ThrowAsync<GradientLabsApiException>();
        ex.Which.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
