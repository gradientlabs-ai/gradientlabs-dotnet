using System.Text.Json;
using FluentAssertions;
using GradientLabs.Api.Serialization;
using Xunit;

namespace GradientLabs.Api.Tests;

public class SerializationTests
{
    [Fact]
    public void ArticleStatus_KnownValues_RoundTrip()
    {
        var statuses = new[] { ArticleStatus.Draft, ArticleStatus.Published, ArticleStatus.Deleted, ArticleStatus.Excluded, ArticleStatus.Unknown };
        foreach (var s in statuses)
        {
            var json = JsonSerializer.Serialize(s, GradientLabsJsonOptions.Default);
            var roundTripped = JsonSerializer.Deserialize<ArticleStatus>(json, GradientLabsJsonOptions.Default);
            roundTripped.Should().Be(s);
        }
    }

    [Fact]
    public void ArticleStatus_UnknownValue_IsPreserved()
    {
        var json = "\"future-value\"";
        var s = JsonSerializer.Deserialize<ArticleStatus>(json, GradientLabsJsonOptions.Default);
        s.Value.Should().Be("future-value");
    }

    [Fact]
    public void ParticipantType_AiAgent_PreservesExactCasing()
    {
        var json = "\"AI Agent\"";
        var pt = JsonSerializer.Deserialize<ParticipantType>(json, GradientLabsJsonOptions.Default);
        pt.Should().Be(ParticipantType.AiAgent);
        pt.Value.Should().Be("AI Agent");

        var serialized = JsonSerializer.Serialize(pt, GradientLabsJsonOptions.Default);
        serialized.Should().Be("\"AI Agent\"");
    }

    [Fact]
    public void ConversationChannel_Web_SerializesToLowercase()
    {
        var json = JsonSerializer.Serialize(ConversationChannel.Web, GradientLabsJsonOptions.Default);
        json.Should().Be("\"web\"");
    }

    [Fact]
    public void JsonExtensionData_CapturesUnknownFields()
    {
        var json = """{"id":"c1","customer_id":"u1","status":"open","agent_is_active":false,"channel":"web","created":"2024-01-01T00:00:00Z","updated":"2024-01-01T00:00:00Z","future_field":"future_value"}""";
        var conv = JsonSerializer.Deserialize<Conversation>(json, GradientLabsJsonOptions.Default)!;
        conv.AdditionalProperties.Should().ContainKey("future_field");
    }

    [Fact]
    public void NullRequestProperties_AreOmitted()
    {
        var req = new CancelConversationRequest { Reason = null };
        var json = JsonSerializer.Serialize(req, GradientLabsJsonOptions.Default);
        json.Should().NotContain("reason");
    }

    [Fact]
    public void FinishConversationRequest_ReasonCode_SerializesToSnakeCase()
    {
        var req = new FinishConversationRequest { ReasonCode = "customer-ended-chat" };
        var json = JsonSerializer.Serialize(req, GradientLabsJsonOptions.Default);
        json.Should().Contain("\"reason_code\":\"customer-ended-chat\"");
    }

    [Fact]
    public void FinishConversationRequest_NullReasonCode_IsOmitted()
    {
        var req = new FinishConversationRequest { Reason = "resolved" };
        var json = JsonSerializer.Serialize(req, GradientLabsJsonOptions.Default);
        json.Should().NotContain("reason_code");
    }

    [Fact]
    public void NanosecondTimeSpanConverter_ReadsNanoseconds()
    {
        var json = "1_000_000_000".Replace("_", "");
        var ts = JsonSerializer.Deserialize<TimeSpan>(json, GradientLabsJsonOptions.Default);
        ts.Should().Be(TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void NanosecondTimeSpanConverter_WritesTicks()
    {
        var ts = TimeSpan.FromSeconds(2);
        var json = JsonSerializer.Serialize(ts, GradientLabsJsonOptions.Default);
        json.Should().Be("2000000000");
    }

    [Fact]
    public void NanosecondTimeSpanConverter_TruncatesSubHundredNs()
    {
        var ns = 150L;
        var json = ns.ToString();
        var ts = JsonSerializer.Deserialize<TimeSpan>(json, GradientLabsJsonOptions.Default);
        ts.Ticks.Should().Be(1);
    }

    [Fact]
    public void NanosecondTimeSpanConverter_OverflowThrowsJsonException()
    {
        var ts = TimeSpan.MaxValue;
        var act = () => JsonSerializer.Serialize(ts, GradientLabsJsonOptions.Default);
        act.Should().Throw<Exception>();
    }

    [Theory]
    [InlineData("draft")]
    [InlineData("published")]
    [InlineData("deleted")]
    [InlineData("excluded")]
    [InlineData("unknown")]
    public void ArticleStatus_AllKnownValues_Deserialize(string value)
    {
        var json = $"\"{value}\"";
        var s = JsonSerializer.Deserialize<ArticleStatus>(json, GradientLabsJsonOptions.Default);
        s.Value.Should().Be(value);
    }

    [Fact]
    public void CustomerSupportPlatformIdentifiers_SerializeToSnakeCase()
    {
        var req = new StartConversationRequest
        {
            Id = "c1",
            CustomerId = "u1",
            Channel = ConversationChannel.Web,
            CustomerSupportPlatformIdentifiers = new[]
            {
                new CustomerSupportPlatformIdentifier
                {
                    SupportPlatform = SupportPlatform.Intercom,
                    Type = CustomerSupportPlatformIdentifierType.IntercomUser,
                    Value = "6953e162a988d9ef0f73ef9b",
                },
                new CustomerSupportPlatformIdentifier
                {
                    SupportPlatform = SupportPlatform.Freshdesk,
                    Value = "12345",
                },
            },
        };

        var json = JsonSerializer.Serialize(req, GradientLabsJsonOptions.Default);

        json.Should().Contain("\"customer_support_platform_identifiers\"");
        json.Should().Contain("\"support_platform\":\"intercom\"");
        json.Should().Contain("\"type\":\"intercom_user\"");
        json.Should().Contain("\"value\":\"6953e162a988d9ef0f73ef9b\"");
        json.Should().Contain("\"support_platform\":\"freshdesk\"");
        json.Should().Contain("\"value\":\"12345\"");
    }

    [Fact]
    public void CustomerSupportPlatformIdentifiers_TypeOmittedWhenNull()
    {
        var identifier = new CustomerSupportPlatformIdentifier
        {
            SupportPlatform = SupportPlatform.Freshchat,
            Value = "abc123",
        };

        var json = JsonSerializer.Serialize(identifier, GradientLabsJsonOptions.Default);

        json.Should().NotContain("\"type\"");
    }

    [Fact]
    public void CustomerSupportPlatformIdentifiers_NullOnRequest_IsOmitted()
    {
        var req = new StartConversationRequest { Id = "c1", CustomerId = "u1", Channel = ConversationChannel.Web };

        var json = JsonSerializer.Serialize(req, GradientLabsJsonOptions.Default);

        json.Should().NotContain("customer_support_platform_identifiers");
    }

    [Fact]
    public void CustomerSupportPlatformIdentifierType_RoundTrips()
    {
        var json = JsonSerializer.Serialize(CustomerSupportPlatformIdentifierType.SalesforceAccountId, GradientLabsJsonOptions.Default);
        json.Should().Be("\"salesforce_account_id\"");

        var roundTripped = JsonSerializer.Deserialize<CustomerSupportPlatformIdentifierType>(json, GradientLabsJsonOptions.Default);
        roundTripped.Should().Be(CustomerSupportPlatformIdentifierType.SalesforceAccountId);
    }
}
