using System.Net;
using System.Text.Json;
using FluentAssertions;
using GradientLabs.Api.Serialization;
using GradientLabs.Api.Tests.Fakes;
using Xunit;

namespace GradientLabs.Api.Tests;

public class PaginationTests
{
    [Fact]
    public void Page_WithNext_HasNextPageTrue_HasPrevPageFalse()
    {
        var json = """{"items":[],"next":"cursor_abc"}""";
        var page = JsonSerializer.Deserialize<Page<Procedure>>(json, GradientLabsJsonOptions.Default)!;
        page.HasNextPage.Should().BeTrue();
        page.HasPrevPage.Should().BeFalse();
        page.Next.Should().Be("cursor_abc");
    }

    [Fact]
    public void Page_WithNoCursors_BothFalse()
    {
        var json = """{"items":[]}""";
        var page = JsonSerializer.Deserialize<Page<Procedure>>(json, GradientLabsJsonOptions.Default)!;
        page.HasNextPage.Should().BeFalse();
        page.HasPrevPage.Should().BeFalse();
    }

    [Fact]
    public void Page_JsonExtensionData_CapturesUnknownFields()
    {
        var json = """{"items":[],"next":null,"future_envelope_field":"value"}""";
        var page = JsonSerializer.Deserialize<Page<Procedure>>(json, GradientLabsJsonOptions.Default)!;
        page.AdditionalProperties.Should().ContainKey("future_envelope_field");
    }

    [Fact]
    public async Task ManualIteration_RequestsSecondPageUsingFirstPageNext()
    {
        var firstPage = """{"items":[{"id":"p1","name":"P1","status":"live","created":"2024-01-01T00:00:00Z","updated":"2024-01-01T00:00:00Z","has_daily_limit":false}],"next":"cursor_page2"}""";
        var secondPage = """{"items":[{"id":"p2","name":"P2","status":"live","created":"2024-01-01T00:00:00Z","updated":"2024-01-01T00:00:00Z","has_daily_limit":false}]}""";

        var callCount = 0;
        var handler = new FakeHttpMessageHandler();
        handler.SetResponse(HttpStatusCode.OK, firstPage);

        using var client = new GradientLabsManagementClient(new GradientLabsClientOptions
        {
            ApiKey = "test_key",
            HttpMessageHandler = handler,
            BaseUri = new Uri("https://localhost:9999"),
        });

        var page1 = await client.Procedures.ListAsync();
        callCount++;
        page1.Items.Should().HaveCount(1);
        page1.Next.Should().Be("cursor_page2");

        handler.SetResponse(HttpStatusCode.OK, secondPage);
        var page2 = await client.Procedures.ListAsync(cursor: page1.Next);
        callCount++;

        var lastRequest = handler.LastRequest!;
        lastRequest.RequestUri!.Query.Should().Contain("cursor=cursor_page2");
        page2.Items.Should().HaveCount(1);
        page2.HasNextPage.Should().BeFalse();
    }
}
