using System.Net;
using System.Text.Json;
using FluentAssertions;
using GradientLabs.Api.Tests.Fakes;
using Xunit;

namespace GradientLabs.Api.Tests;

public class OutboundTests
{
    private const string SuccessBody = """{"conversation_id":"conv_123"}""";

    private static GradientLabsIntegrationClient CreateClient(HttpMessageHandler handler)
        => new(new GradientLabsClientOptions
        {
            ApiKey = "test_key_abc123",
            HttpMessageHandler = handler,
            BaseUri = new Uri("https://localhost:9999"),
        });

    [Fact]
    public async Task StartChatConversation_PostsToChatPath()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, SuccessBody);
        using var client = CreateClient(handler);

        var response = await client.Outbound.StartChatConversationAsync(new StartOutboundChatConversationRequest
        {
            CustomerId = "cust_1",
            ProcedureId = "proc_1",
            SupportPlatform = SupportPlatform.Intercom,
            Body = "Hello there",
            CustomerSupportPlatformIdentifiers = new[]
            {
                new CustomerSupportPlatformIdentifier
                {
                    SupportPlatform = SupportPlatform.Intercom,
                    Type = CustomerSupportPlatformIdentifierType.IntercomUser,
                    Value = "6953e162a988d9ef0f73ef9b",
                },
            },
            Resources = new Dictionary<string, JsonElement>
            {
                ["customer_profile"] = JsonDocument.Parse("""{"tier":"premium"}""").RootElement,
            },
        });

        response.ConversationId.Should().Be("conv_123");
        handler.LastRequest!.Method.Should().Be(HttpMethod.Post);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/outbound/conversations/chat");

        var body = handler.LastRequestBody!;
        body.Should().Contain("\"customer_id\":\"cust_1\"");
        body.Should().Contain("\"procedure_id\":\"proc_1\"");
        body.Should().Contain("\"support_platform\":\"intercom\"");
        body.Should().Contain("\"body\":\"Hello there\"");
        body.Should().Contain("\"customer_support_platform_identifiers\"");
        body.Should().Contain("\"type\":\"intercom_user\"");
        body.Should().Contain("\"customer_profile\"");
    }

    [Fact]
    public async Task StartChatConversation_OmitsOptionalFieldsWhenUnset()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, SuccessBody);
        using var client = CreateClient(handler);

        await client.Outbound.StartChatConversationAsync(new StartOutboundChatConversationRequest
        {
            CustomerId = "cust_1",
            ProcedureId = "proc_1",
            SupportPlatform = SupportPlatform.Livechat,
        });

        var body = handler.LastRequestBody!;
        body.Should().NotContain("\"body\"");
        body.Should().NotContain("customer_support_platform_identifiers");
        body.Should().NotContain("\"resources\"");
    }

    [Fact]
    public async Task StartEmailConversation_PostsToEmailPathWithSubjectAndBody()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, SuccessBody);
        using var client = CreateClient(handler);

        var response = await client.Outbound.StartEmailConversationAsync(new StartOutboundEmailConversationRequest
        {
            CustomerId = "cust_1",
            ProcedureId = "proc_1",
            SupportPlatform = SupportPlatform.Zendesk,
            Subject = "Your recent order",
            Body = "We have an update for you.",
            CustomerSupportPlatformIdentifiers = new[]
            {
                new CustomerSupportPlatformIdentifier
                {
                    SupportPlatform = SupportPlatform.Zendesk,
                    Type = CustomerSupportPlatformIdentifierType.ZendeskSupportUser,
                    Value = "9876543210",
                },
            },
        });

        response.ConversationId.Should().Be("conv_123");
        handler.LastRequest!.Method.Should().Be(HttpMethod.Post);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/outbound/conversations/email");

        var body = handler.LastRequestBody!;
        body.Should().Contain("\"support_platform\":\"zendesk\"");
        body.Should().Contain("\"subject\":\"Your recent order\"");
        body.Should().Contain("\"body\":\"We have an update for you.\"");
        body.Should().Contain("\"type\":\"zendesk_support_user\"");
    }

    [Fact]
    public async Task StartPhoneConversation_PostsToPhonePath()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, SuccessBody);
        using var client = CreateClient(handler);

        var response = await client.Outbound.StartPhoneConversationAsync(new StartOutboundPhoneConversationRequest
        {
            CustomerId = "cust_1",
            ProcedureId = "proc_1",
            ToPhoneNumber = "+14155551234",
            FromPhoneNumber = "+442071838750",
        });

        response.ConversationId.Should().Be("conv_123");
        handler.LastRequest!.Method.Should().Be(HttpMethod.Post);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/outbound/conversations/phone");

        using var body = JsonDocument.Parse(handler.LastRequestBody!);
        body.RootElement.GetProperty("to_phone_number").GetString().Should().Be("+14155551234");
        body.RootElement.GetProperty("from_phone_number").GetString().Should().Be("+442071838750");
        handler.LastRequestBody.Should().NotContain("support_platform");
    }

    [Fact]
    public async Task StartPhoneConversation_PropagatesApiError()
    {
        var errorJson = """{"code":"invalid_argument","message":"from_phone_number is not provisioned"}""";
        var handler = new FakeHttpMessageHandler(HttpStatusCode.BadRequest, errorJson);
        using var client = CreateClient(handler);

        var act = () => client.Outbound.StartPhoneConversationAsync(new StartOutboundPhoneConversationRequest
        {
            CustomerId = "cust_1",
            ProcedureId = "proc_1",
            ToPhoneNumber = "+14155551234",
            FromPhoneNumber = "+442071838750",
        });

        var ex = await act.Should().ThrowAsync<GradientLabsApiException>();
        ex.Which.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        ex.Which.ErrorCode.Should().Be(GradientLabsErrorCode.InvalidArgument);
    }
}
