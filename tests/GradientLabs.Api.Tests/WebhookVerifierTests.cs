using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace GradientLabs.Api.Tests;

public class WebhookVerifierTests
{
    private const string TestKey = "test_signing_key_abc123";

    private static string MakeSignatureHeader(byte[] body, string key, long? unixTs = null, bool useWrongKey = false)
    {
        var ts = unixTs ?? DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var signingKey = Encoding.UTF8.GetBytes(useWrongKey ? "wrong_key" : key);
        var tsBytes = Encoding.UTF8.GetBytes(ts.ToString());
        var dot = "."u8.ToArray();

        using var hmac = new HMACSHA256(signingKey);
        hmac.TransformBlock(tsBytes, 0, tsBytes.Length, null, 0);
        hmac.TransformBlock(dot, 0, dot.Length, null, 0);
        hmac.TransformFinalBlock(body, 0, body.Length);
        var sig = Convert.ToHexString(hmac.Hash!).ToLowerInvariant();
        return $"t={ts},v1={sig}";
    }

    private static GradientLabsWebhookVerifier CreateVerifier(string? key = TestKey)
        => new(key ?? TestKey);

    [Fact]
    public void ValidSignature_IsAccepted()
    {
        var body = """{"id":"e1","type":"agent.message","sequence_number":1,"timestamp":"2024-01-01T00:00:00Z","body":"hello","total":1,"sequence":1,"conversation":{"id":"c1","customer_id":"u1"}}"""u8.ToArray();
        var header = MakeSignatureHeader(body, TestKey);
        var verifier = CreateVerifier();

        var act = () => verifier.Verify(body, header);

        act.Should().NotThrow();
    }

    [Fact]
    public void WrongSigningKey_IsRejected()
    {
        var body = """{"id":"e1","type":"agent.message","sequence_number":1,"timestamp":"2024-01-01T00:00:00Z","body":"hi","total":1,"sequence":1,"conversation":{"id":"c1","customer_id":"u1"}}"""u8.ToArray();
        var header = MakeSignatureHeader(body, TestKey, useWrongKey: true);
        var verifier = CreateVerifier();

        var act = () => verifier.Verify(body, header);

        act.Should().Throw<GradientLabsWebhookSignatureException>();
    }

    [Fact]
    public void MissingSignatureHeader_IsRejected()
    {
        var body = "test"u8.ToArray();
        var verifier = CreateVerifier();

        var act = () => verifier.Verify(body, "");

        act.Should().Throw<GradientLabsWebhookSignatureException>();
    }

    [Theory]
    [InlineData("v1=abc")]
    [InlineData("t=abc")]
    [InlineData("")]
    [InlineData("garbage")]
    public void MalformedHeader_IsRejected(string header)
    {
        var body = "test"u8.ToArray();
        var verifier = CreateVerifier();

        var act = () => verifier.Verify(body, header);

        act.Should().Throw<GradientLabsWebhookSignatureException>();
    }

    [Fact]
    public void OldTimestamp_IsRejected()
    {
        var body = "test"u8.ToArray();
        var oldTs = DateTimeOffset.UtcNow.AddMinutes(-10).ToUnixTimeSeconds();
        var header = MakeSignatureHeader(body, TestKey, oldTs);
        var verifier = CreateVerifier();

        var act = () => verifier.Verify(body, header);

        act.Should().Throw<GradientLabsWebhookSignatureException>();
    }

    [Fact]
    public void FutureTimestamp_IsRejected()
    {
        var body = "test"u8.ToArray();
        var futureTs = DateTimeOffset.UtcNow.AddMinutes(10).ToUnixTimeSeconds();
        var header = MakeSignatureHeader(body, TestKey, futureTs);
        var verifier = CreateVerifier();

        var act = () => verifier.Verify(body, header);

        act.Should().Throw<GradientLabsWebhookSignatureException>();
    }

    [Fact]
    public void AgentMessage_ParsesCorrectly()
    {
        var body = """{"id":"e1","type":"agent.message","sequence_number":42,"timestamp":"2024-01-01T00:00:00Z","body":"Hello","total":2,"sequence":1,"is_holding":true,"conversation":{"id":"c1","customer_id":"u1"}}"""u8.ToArray();
        var header = MakeSignatureHeader(body, TestKey);
        var verifier = CreateVerifier();

        var evt = verifier.Parse(body, header);

        evt.Should().BeOfType<AgentMessageEvent>();
        var msg = (AgentMessageEvent)evt;
        msg.Body.Should().Be("Hello");
        msg.Total.Should().Be(2);
        msg.Sequence.Should().Be(1);
        msg.IsHolding.Should().BeTrue();
        msg.Conversation.Id.Should().Be("c1");
        msg.SequenceNumber.Should().Be(42L);
    }

    [Fact]
    public void ConversationHandOff_ParsesCorrectly()
    {
        var body = """{"id":"e2","type":"conversation.hand_off","sequence_number":1,"timestamp":"2024-01-01T00:00:00Z","reason_code":"escalation","reason":"Customer escalated","conversation":{"id":"c1","customer_id":"u1"}}"""u8.ToArray();
        var header = MakeSignatureHeader(body, TestKey);
        var verifier = CreateVerifier();

        var evt = verifier.Parse(body, header);

        evt.Should().BeOfType<ConversationHandOffEvent>();
        var ho = (ConversationHandOffEvent)evt;
        ho.Reason.Should().Be("escalation");
        ho.Description.Should().Be("Customer escalated");
    }

    [Fact]
    public void ConversationFinished_ParsesCorrectly()
    {
        var body = """{"id":"e3","type":"conversation.finished","sequence_number":1,"timestamp":"2024-01-01T00:00:00Z","reason_code":"resolved","conversation":{"id":"c1","customer_id":"u1"}}"""u8.ToArray();
        var header = MakeSignatureHeader(body, TestKey);
        var verifier = CreateVerifier();

        var evt = verifier.Parse(body, header);

        evt.Should().BeOfType<ConversationFinishedEvent>();
        var fin = (ConversationFinishedEvent)evt;
        fin.Reason.Should().Be("resolved");
    }

    [Fact]
    public void ActionExecute_ParsesCorrectly()
    {
        var body = """{"id":"e4","type":"action.execute","sequence_number":1,"timestamp":"2024-01-01T00:00:00Z","action":"look_up_order","params":{"order_id":"123"},"conversation":{"id":"c1","customer_id":"u1"}}"""u8.ToArray();
        var header = MakeSignatureHeader(body, TestKey);
        var verifier = CreateVerifier();

        var evt = verifier.Parse(body, header);

        evt.Should().BeOfType<ActionExecuteEvent>();
        var ae = (ActionExecuteEvent)evt;
        ae.Action.Should().Be("look_up_order");
    }

    [Fact]
    public void ResourcePull_ParsesCorrectly()
    {
        var body = """{"id":"e5","type":"resource.pull","sequence_number":1,"timestamp":"2024-01-01T00:00:00Z","resource_type":"order","conversation":{"id":"c1","customer_id":"u1"}}"""u8.ToArray();
        var header = MakeSignatureHeader(body, TestKey);
        var verifier = CreateVerifier();

        var evt = verifier.Parse(body, header);

        evt.Should().BeOfType<ResourcePullEvent>();
        var rp = (ResourcePullEvent)evt;
        rp.ResourceType.Should().Be("order");
    }

    [Fact]
    public void TokenHeader_IsSetWhenProvided()
    {
        var body = """{"id":"e1","type":"resource.pull","sequence_number":1,"timestamp":"2024-01-01T00:00:00Z","resource_type":"x","conversation":{"id":"c1","customer_id":"u1"}}"""u8.ToArray();
        var header = MakeSignatureHeader(body, TestKey);
        var verifier = CreateVerifier();

        var evt = verifier.Parse(body, header, tokenHeader: "my-secret-token");

        evt.Token.Should().Be("my-secret-token");
    }

    [Fact]
    public void TokenHeader_IsNullWhenAbsent()
    {
        var body = """{"id":"e1","type":"resource.pull","sequence_number":1,"timestamp":"2024-01-01T00:00:00Z","resource_type":"x","conversation":{"id":"c1","customer_id":"u1"}}"""u8.ToArray();
        var header = MakeSignatureHeader(body, TestKey);
        var verifier = CreateVerifier();

        var evt = verifier.Parse(body, header);

        evt.Token.Should().BeNull();
    }

    [Fact]
    public void UnknownEventType_ReturnsUnknownWebhookEvent_WithoutThrowing()
    {
        var body = """{"id":"e99","type":"future.event","sequence_number":1,"timestamp":"2024-01-01T00:00:00Z","data":{"foo":"bar"}}"""u8.ToArray();
        var header = MakeSignatureHeader(body, TestKey);
        var verifier = CreateVerifier();

        var evt = verifier.Parse(body, header);

        evt.Should().BeOfType<UnknownWebhookEvent>();
        evt.Type.Value.Should().Be("future.event");
    }

    [Fact]
    public void SequenceNumber_IsLong()
    {
        var largeSeq = 9223372036854775807L;
        var bodyStr = $"{{\"id\":\"e1\",\"type\":\"resource.pull\",\"sequence_number\":{largeSeq},\"timestamp\":\"2024-01-01T00:00:00Z\",\"resource_type\":\"x\",\"conversation\":{{\"id\":\"c1\",\"customer_id\":\"u1\"}}}}";
        var body = System.Text.Encoding.UTF8.GetBytes(bodyStr);
        var header = MakeSignatureHeader(body, TestKey);
        var verifier = CreateVerifier();

        var evt = verifier.Parse(body, header);

        evt.SequenceNumber.Should().Be(largeSeq);
    }

    [Fact]
    public void MultipleV1Signatures_AcceptsWhenOneMatches()
    {
        var body = "test"u8.ToArray();
        var ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var validHeader = MakeSignatureHeader(body, TestKey, ts);
        var (_, validSig) = (validHeader.Split(",")[0], validHeader.Split(",")[1]);
        var headerWithExtra = $"t={ts},v1=0000000000000000000000000000000000000000000000000000000000000000,{validSig}";
        var verifier = CreateVerifier();

        var act = () => verifier.Verify(body, headerWithExtra);

        act.Should().NotThrow();
    }
}
