using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class Secret
{
    public string Name { get; init; } = string.Empty;
    public DateTimeOffset Created { get; init; }
    public DateTimeOffset Updated { get; init; }
    public DateTimeOffset? Expiry { get; init; }
    public HttpDefinition? RefreshMechanismHttp { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class WriteSecretRequest
{
    public string Value { get; init; } = string.Empty;
    public DateTimeOffset? Expiry { get; init; }
    public HttpDefinition? RefreshMechanismHttp { get; init; }
}
