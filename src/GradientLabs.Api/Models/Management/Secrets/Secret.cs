using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class Secret
{
    public string Name { get; init; } = string.Empty;
    public DateTimeOffset Created { get; init; }
    public DateTimeOffset Updated { get; init; }
    public DateTimeOffset? Expiry { get; init; }
    public SecretRefreshMechanismHttp? RefreshMechanismHttp { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class WriteSecretRequest
{
    public string Value { get; init; } = string.Empty;
    public DateTimeOffset? Expiry { get; init; }
    public SecretRefreshMechanismHttp? RefreshMechanismHttp { get; init; }
}

public sealed class SecretRefreshMechanismHttp
{
    /// <summary>The HTTP request that fetches a fresh value for the secret.</summary>
    public SecretRefreshRequestDefinition RequestDefinition { get; init; } = new();

    /// <summary>The name of the field in the response that holds the new value, such as <c>access_token</c>.</summary>
    public string ResponseParamName { get; init; } = string.Empty;

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class SecretRefreshRequestDefinition
{
    public string Method { get; init; } = string.Empty;
    public string UrlTemplate { get; init; } = string.Empty;
    public HttpBodyDefinition? Body { get; init; }
    public IReadOnlyDictionary<string, string>? HeaderTemplates { get; init; }
    public SecretRefreshTimeout? Retry { get; init; }
}

public sealed class SecretRefreshTimeout
{
    public long? RequestTimeoutSeconds { get; init; }
}

internal sealed class ListSecretsResponse
{
    public IReadOnlyList<Secret> Secrets { get; init; } = [];
}
