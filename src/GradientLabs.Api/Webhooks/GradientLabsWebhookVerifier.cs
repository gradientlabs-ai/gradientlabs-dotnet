using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using GradientLabs.Api.Serialization;

namespace GradientLabs.Api;

public sealed class GradientLabsWebhookVerifier
{
    private readonly byte[] _signingKey;
    private readonly TimeSpan _leeway;
    private readonly TimeProvider _timeProvider;

    public GradientLabsWebhookVerifier(string signingKey, GradientLabsClientOptions? options = null)
    {
        if (string.IsNullOrEmpty(signingKey))
            throw new GradientLabsWebhookSignatureException("Signing key must not be null or empty.");

        _signingKey = Encoding.UTF8.GetBytes(signingKey);
        _leeway = options?.WebhookLeeway ?? TimeSpan.FromMinutes(5);
        _timeProvider = options?.TimeProvider ?? TimeProvider.System;
    }

    public WebhookVerifyResult Verify(
        ReadOnlySpan<byte> rawBody,
        string signatureHeader,
        DateTimeOffset? now = null)
    {
        if (string.IsNullOrEmpty(signatureHeader))
            throw new GradientLabsWebhookSignatureException("X-GradientLabs-Signature header is missing or empty.");

        var (timestamp, signatures) = ParseHeader(signatureHeader);
        var effectiveNow = now ?? _timeProvider.GetUtcNow();

        if (Math.Abs((effectiveNow - timestamp).TotalSeconds) > _leeway.TotalSeconds)
            throw new GradientLabsWebhookSignatureException("Webhook timestamp is outside the allowed leeway window.");

        var expected = ComputeSignature(timestamp, rawBody);

        foreach (var sig in signatures)
        {
            if (CryptographicOperations.FixedTimeEquals(expected, sig))
                return new WebhookVerifyResult { Timestamp = timestamp };
        }

        throw new GradientLabsWebhookSignatureException("Webhook signature verification failed.");
    }

    public WebhookEvent Parse(
        ReadOnlySpan<byte> rawBody,
        string signatureHeader,
        string? tokenHeader = null,
        DateTimeOffset? now = null)
    {
        Verify(rawBody, signatureHeader, now);
        return ParseEvent(rawBody, tokenHeader);
    }

    private static WebhookEvent ParseEvent(ReadOnlySpan<byte> rawBody, string? token)
    {
        using var doc = JsonDocument.Parse(rawBody.ToArray());
        var root = doc.RootElement;

        var typeStr = root.TryGetProperty("type", out var typeProp)
            ? typeProp.GetString() ?? string.Empty
            : string.Empty;

        var eventType = new GradientLabsWebhookEventType(typeStr);

        var dataEl = root.TryGetProperty("data", out var dataProp)
            ? dataProp.Clone()
            : default;

        WebhookEvent evt;

        if (eventType == GradientLabsWebhookEventType.AgentMessage)
            evt = DeserializeWithEnvelope<AgentMessageEvent>(root, dataEl);
        else if (eventType == GradientLabsWebhookEventType.ConversationHandOff)
            evt = DeserializeWithEnvelope<ConversationHandOffEvent>(root, dataEl);
        else if (eventType == GradientLabsWebhookEventType.ConversationFinished)
            evt = DeserializeWithEnvelope<ConversationFinishedEvent>(root, dataEl);
        else if (eventType == GradientLabsWebhookEventType.ActionExecute)
            evt = DeserializeWithEnvelope<ActionExecuteEvent>(root, dataEl);
        else if (eventType == GradientLabsWebhookEventType.ResourcePull)
            evt = DeserializeWithEnvelope<ResourcePullEvent>(root, dataEl);
        else if (eventType == GradientLabsWebhookEventType.BackOfficeTaskComplete)
            evt = DeserializeWithEnvelope<BackOfficeTaskCompleteEvent>(root, dataEl);
        else if (eventType == GradientLabsWebhookEventType.BackOfficeTaskHandOff)
            evt = DeserializeWithEnvelope<BackOfficeTaskHandOffEvent>(root, dataEl);
        else if (eventType == GradientLabsWebhookEventType.BackOfficeTaskFail)
            evt = DeserializeWithEnvelope<BackOfficeTaskFailEvent>(root, dataEl);
        else
        {
            var unknown = new UnknownWebhookEvent { Data = dataEl };
            SetEnvelopeFields(unknown, root);
            evt = unknown;
        }

        evt.Token = token;
        return evt;
    }

    private static T DeserializeWithEnvelope<T>(JsonElement root, JsonElement dataEl) where T : WebhookEvent
    {
        T evt;
        if (dataEl.ValueKind != JsonValueKind.Undefined)
        {
            evt = JsonSerializer.Deserialize<T>(dataEl.GetRawText(), GradientLabsJsonOptions.Default)!;
        }
        else
        {
            evt = JsonSerializer.Deserialize<T>(root.GetRawText(), GradientLabsJsonOptions.Default)!;
        }
        SetEnvelopeFields(evt, root);
        return evt;
    }

    private static void SetEnvelopeFields(WebhookEvent evt, JsonElement root)
    {
        if (root.TryGetProperty("id", out var idEl))
            evt.Id = idEl.GetString() ?? string.Empty;

        if (root.TryGetProperty("type", out var typeEl))
            evt.Type = new GradientLabsWebhookEventType(typeEl.GetString() ?? string.Empty);

        if (root.TryGetProperty("sequence_number", out var seqEl))
            evt.SequenceNumber = seqEl.GetInt64();

        if (root.TryGetProperty("timestamp", out var tsEl))
            evt.Timestamp = tsEl.GetDateTimeOffset();
    }

    private byte[] ComputeSignature(DateTimeOffset timestamp, ReadOnlySpan<byte> body)
    {
        var tsBytes = Encoding.UTF8.GetBytes(timestamp.ToUnixTimeSeconds().ToString());
        var dot = "."u8;

        using var hmac = new HMACSHA256(_signingKey);
        hmac.TransformBlock(tsBytes, 0, tsBytes.Length, null, 0);
        hmac.TransformBlock(dot.ToArray(), 0, dot.Length, null, 0);
        hmac.TransformFinalBlock(body.ToArray(), 0, body.Length);
        return hmac.Hash!;
    }

    private static (DateTimeOffset timestamp, List<byte[]> signatures) ParseHeader(string header)
    {
        long? unixTs = null;
        var signatures = new List<byte[]>();

        foreach (var part in header.Split(','))
        {
            var idx = part.IndexOf('=');
            if (idx < 0)
                throw new GradientLabsWebhookSignatureException("Malformed X-GradientLabs-Signature header.");

            var key = part[..idx].Trim();
            var val = part[(idx + 1)..].Trim();

            if (key == "t")
            {
                if (!long.TryParse(val, out var ts))
                    throw new GradientLabsWebhookSignatureException("Invalid timestamp in X-GradientLabs-Signature header.");
                unixTs = ts;
            }
            else if (key == "v1")
            {
                try
                {
                    signatures.Add(Convert.FromHexString(val));
                }
                catch
                {
                    throw new GradientLabsWebhookSignatureException("Invalid hex signature in X-GradientLabs-Signature header.");
                }
            }
        }

        if (unixTs is null)
            throw new GradientLabsWebhookSignatureException("X-GradientLabs-Signature header contains no timestamp component.");

        if (signatures.Count == 0)
            throw new GradientLabsWebhookSignatureException("X-GradientLabs-Signature header contains no v1 signature.");

        return (DateTimeOffset.FromUnixTimeSeconds(unixTs.Value), signatures);
    }
}
