using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using GradientLabs.Api.Serialization;

namespace GradientLabs.Api.Http;

internal sealed class HttpPipeline : IDisposable
{
    private const int MaxErrorBodyBytes = 64 * 1024;

    private readonly HttpClient _httpClient;
    private readonly bool _ownsClient;
    private readonly string _userAgent;
    private readonly string _apiKey;

    internal HttpPipeline(GradientLabsClientOptions options)
    {
        if (string.IsNullOrEmpty(options.ApiKey))
            throw new GradientLabsException("ApiKey must not be null or empty.");

        _apiKey = options.ApiKey;
        _userAgent = BuildUserAgent(options.UserAgentVersion);

        if (options.HttpClient is not null)
        {
            _httpClient = options.HttpClient;
            _ownsClient = false;
        }
        else if (options.HttpMessageHandler is not null)
        {
            _httpClient = new HttpClient(options.HttpMessageHandler, disposeHandler: false);
            _ownsClient = true;
        }
        else
        {
            _httpClient = new HttpClient();
            _ownsClient = true;
        }

        _httpClient.BaseAddress = options.BaseUri;

        if (options.Timeout.HasValue && _ownsClient)
            _httpClient.Timeout = options.Timeout.Value;
    }

    internal async Task<T> SendAsync<T>(
        HttpMethod method,
        string path,
        object? body,
        CancellationToken cancellationToken)
    {
        var response = await SendCoreAsync(method, path, body, cancellationToken).ConfigureAwait(false);
        var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            var result = JsonSerializer.Deserialize<T>(content, GradientLabsJsonOptions.Default);
            return result!;
        }
        catch (JsonException ex)
        {
            throw new GradientLabsSerializationException($"Failed to deserialize response: {ex.Message}", ex);
        }
    }

    internal async Task SendAsync(
        HttpMethod method,
        string path,
        object? body,
        CancellationToken cancellationToken)
    {
        await SendCoreAsync(method, path, body, cancellationToken).ConfigureAwait(false);
    }

    private async Task<HttpResponseMessage> SendCoreAsync(
        HttpMethod method,
        string path,
        object? body,
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(method, path);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        request.Headers.Add("User-Agent", _userAgent);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (body is not null || method == HttpMethod.Post || method == HttpMethod.Put || method == HttpMethod.Patch)
        {
            var json = body is not null
                ? JsonSerializer.Serialize(body, GradientLabsJsonOptions.Default)
                : "{}";
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        HttpResponseMessage response;
        try
        {
            response = await _httpClient
                .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            throw new GradientLabsRequestException($"Request failed: {ex.Message}", ex);
        }

        if (!response.IsSuccessStatusCode)
        {
            await ThrowApiExceptionAsync(response, cancellationToken).ConfigureAwait(false);
        }

        return response;
    }

    private static async Task ThrowApiExceptionAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        string? rawBody = null;
        try
        {
            var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
            rawBody = bytes.Length > MaxErrorBodyBytes
                ? Encoding.UTF8.GetString(bytes, 0, MaxErrorBodyBytes)
                : Encoding.UTF8.GetString(bytes);
        }
        catch { /* best effort */ }

        var errorCode = GradientLabsErrorCode.Unknown;
        string apiMessage = response.ReasonPhrase ?? "Unknown error";
        string? traceId = null;
        IReadOnlyDictionary<string, JsonElement>? details = null;

        if (rawBody is not null)
        {
            try
            {
                using var doc = JsonDocument.Parse(rawBody);
                var root = doc.RootElement;

                if (root.TryGetProperty("code", out var codeEl) && codeEl.ValueKind == JsonValueKind.String)
                    errorCode = new GradientLabsErrorCode(codeEl.GetString()!);

                if (root.TryGetProperty("message", out var msgEl) && msgEl.ValueKind == JsonValueKind.String)
                    apiMessage = msgEl.GetString()!;

                if (root.TryGetProperty("details", out var detailsEl) && detailsEl.ValueKind == JsonValueKind.Object)
                {
                    var dict = new Dictionary<string, JsonElement>();
                    foreach (var prop in detailsEl.EnumerateObject())
                        dict[prop.Name] = prop.Value.Clone();

                    if (dict.TryGetValue("trace_id", out var traceEl) && traceEl.ValueKind == JsonValueKind.String)
                        traceId = traceEl.GetString();

                    details = dict;
                }
            }
            catch { /* non-JSON body: use defaults */ }
        }

        throw new GradientLabsApiException(
            response.StatusCode,
            errorCode,
            apiMessage,
            traceId,
            details,
            rawBody);
    }

    private static string BuildUserAgent(string? versionOverride)
    {
        var version = versionOverride
            ?? typeof(HttpPipeline).Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion
                ?? "0.0.0";

        // Strip build metadata suffix like +abc1234
        var plusIndex = version.IndexOf('+');
        if (plusIndex >= 0)
            version = version[..plusIndex];

        var runtimeVersion = RuntimeInformation.FrameworkDescription;
        return $"Gradient-Labs-CSharp/{version} ({runtimeVersion})";
    }

    public void Dispose()
    {
        if (_ownsClient)
            _httpClient.Dispose();
    }
}
