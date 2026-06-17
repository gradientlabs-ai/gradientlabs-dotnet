using System.Net;
using System.Text.Json;

namespace GradientLabs.Api;

public sealed class GradientLabsApiException : GradientLabsException
{
    public HttpStatusCode StatusCode { get; }
    public GradientLabsErrorCode ErrorCode { get; }
    public string ApiMessage { get; }
    public string? TraceId { get; }
    public IReadOnlyDictionary<string, JsonElement>? Details { get; }
    public string? ResponseBody { get; }
    public bool IsRetryable => ErrorCode.IsRetryable;

    public GradientLabsApiException(
        HttpStatusCode statusCode,
        GradientLabsErrorCode errorCode,
        string apiMessage,
        string? traceId,
        IReadOnlyDictionary<string, JsonElement>? details,
        string? responseBody)
        : base($"API error {(int)statusCode}: {apiMessage}")
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
        ApiMessage = apiMessage;
        TraceId = traceId;
        Details = details;
        ResponseBody = responseBody;
    }
}
