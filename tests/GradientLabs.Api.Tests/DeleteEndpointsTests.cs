using System.Net;
using FluentAssertions;
using GradientLabs.Api.Tests.Fakes;
using Xunit;

namespace GradientLabs.Api.Tests;

public class DeleteEndpointsTests
{
    private static GradientLabsManagementClient CreateClient(HttpMessageHandler handler)
        => new(new GradientLabsClientOptions
        {
            ApiKey = "test_key_abc123",
            HttpMessageHandler = handler,
            BaseUri = new Uri("https://localhost:9999"),
        });

    [Fact]
    public async Task DeleteConversation_SendsDeleteToExpectedPath()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.Accepted, "");
        using var client = CreateClient(handler);

        await client.Conversations.DeleteAsync("conv 1");

        handler.LastRequest!.Method.Should().Be(HttpMethod.Delete);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/conversations/conv%201");
        handler.LastRequest.Content.Should().BeNull();
    }

    [Fact]
    public async Task DeleteBackOfficeTask_SendsDeleteToExpectedPath()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.Accepted, "");
        using var client = CreateClient(handler);

        await client.BackOfficeTasks.DeleteAsync("task 1");

        handler.LastRequest!.Method.Should().Be(HttpMethod.Delete);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/back-office-tasks/task%201");
        handler.LastRequest.Content.Should().BeNull();
    }

    [Fact]
    public async Task DeleteConversation_PropagatesApiError()
    {
        var errorJson = """{"code":"failed_precondition","message":"Conversation is active"}""";
        var handler = new FakeHttpMessageHandler(HttpStatusCode.BadRequest, errorJson);
        using var client = CreateClient(handler);

        var act = () => client.Conversations.DeleteAsync("c1");

        var ex = await act.Should().ThrowAsync<GradientLabsApiException>();
        ex.Which.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        ex.Which.ErrorCode.Should().Be(GradientLabsErrorCode.FailedPrecondition);
    }

    [Fact]
    public async Task DeleteBackOfficeTask_PropagatesNotFound()
    {
        var errorJson = """{"code":"not_found","message":"Task not found"}""";
        var handler = new FakeHttpMessageHandler(HttpStatusCode.NotFound, errorJson);
        using var client = CreateClient(handler);

        var act = () => client.BackOfficeTasks.DeleteAsync("missing");

        var ex = await act.Should().ThrowAsync<GradientLabsApiException>();
        ex.Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
        ex.Which.ErrorCode.Should().Be(GradientLabsErrorCode.NotFound);
    }
}
