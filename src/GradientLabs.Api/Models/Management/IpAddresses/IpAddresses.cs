using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class IpAddressesResponse
{
    public IReadOnlyList<string> Api { get; init; } = [];
    public IReadOnlyList<string> Egress { get; init; } = [];

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}
