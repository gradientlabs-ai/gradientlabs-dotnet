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

    private static Memory NewMemory(string externalId, string typeName, string? typeDescription = null)
        => new()
        {
            ExternalId = externalId,
            CustomType = new MemoryCustomType { Name = typeName, Description = typeDescription },
            CreatedAt = new DateTimeOffset(2026, 7, 1, 10, 0, 0, TimeSpan.Zero),
            Data = JsonDocument.Parse("""{"any":"json object"}""").RootElement,
        };

    [Fact]
    public async Task BatchCreateMemories_PostsToV2PathWithSnakeCaseBody()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, "");
        using var client = CreateClient(handler);

        await client.Customers.BatchCreateMemoriesAsync("cust 1", new BatchCreateMemoriesRequest
        {
            Memories = [NewMemory("order_123", "order_event", "An order event"), NewMemory("note_456", "note")],
        });

        handler.LastRequest!.Method.Should().Be(HttpMethod.Post);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/customers/cust%201/memories/v2");

        using var parsed = JsonDocument.Parse(handler.LastRequestBody!);
        var memories = parsed.RootElement.GetProperty("memories");
        memories.GetArrayLength().Should().Be(2);

        var first = memories[0];
        first.GetProperty("external_id").GetString().Should().Be("order_123");
        first.GetProperty("custom_type").GetProperty("name").GetString().Should().Be("order_event");
        first.GetProperty("custom_type").GetProperty("description").GetString().Should().Be("An order event");
        first.GetProperty("created_at").GetString().Should().Be("2026-07-01T10:00:00+00:00");
        first.GetProperty("data").GetProperty("any").GetString().Should().Be("json object");

        memories[1].GetProperty("custom_type").TryGetProperty("description", out _).Should().BeFalse();
    }

    [Fact]
    public async Task BatchCreateMemoriesDeprecated_PostsStringCustomTypeToOldPath()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, "");
        using var client = CreateClient(handler);

#pragma warning disable CS0618
        await client.Customers.BatchCreateMemoriesDeprecatedAsync("c1", new BatchCreateMemoriesDeprecatedRequest
        {
            Memories =
            [
                new DeprecatedMemory
                {
                    ExternalId = "order_123",
                    CustomType = "order",
                    CreatedAt = new DateTimeOffset(2026, 7, 1, 10, 0, 0, TimeSpan.Zero),
                    Data = JsonDocument.Parse("{}").RootElement,
                },
            ],
        });
#pragma warning restore CS0618

        handler.LastRequest!.RequestUri!.AbsolutePath.Should().Be("/customers/c1/memories");
        using var parsed = JsonDocument.Parse(handler.LastRequestBody!);
        parsed.RootElement.GetProperty("memories")[0].GetProperty("custom_type").GetString().Should().Be("order");
    }

    [Fact]
    public async Task ListMemories_UnwrapsMemories()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, """{"memories":[{"external_id":"order_123","custom_type":{"name":"order_event"},"created_at":"2026-07-01T10:00:00Z","data":{"status":"shipped"}}]}""");
        using var client = CreateClient(handler);

        var memories = await client.Customers.ListMemoriesAsync("c1");

        handler.LastRequest!.Method.Should().Be(HttpMethod.Get);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/customers/c1/memories");
        var memory = memories.Single();
        memory.ExternalId.Should().Be("order_123");
        memory.CustomType.Name.Should().Be("order_event");
        memory.Data.GetProperty("status").GetString().Should().Be("shipped");
    }

    [Fact]
    public async Task BatchCreateMemories_PropagatesConflict()
    {
        var errorJson = """{"code":"already_exists","message":"a batch is already being created for this customer"}""";
        var handler = new FakeHttpMessageHandler(HttpStatusCode.Conflict, errorJson);
        using var client = CreateClient(handler);

        var act = () => client.Customers.BatchCreateMemoriesAsync("c1", new BatchCreateMemoriesRequest
        {
            Memories = [NewMemory("order_123", "order_event")],
        });

        var ex = await act.Should().ThrowAsync<GradientLabsApiException>();
        ex.Which.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
