using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api;

public sealed class KnowledgeBase
{
    /// <summary>Your own identifier for this knowledge base, used in <c>KnowledgeBaseIds</c> on articles and notes.</summary>
    public string Id { get; init; } = string.Empty;

    public string GradientLabsId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public DateTimeOffset Created { get; init; }
    public DateTimeOffset Updated { get; init; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

public sealed class CreateKnowledgeBaseRequest
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
}

public sealed class UpdateKnowledgeBaseRequest
{
    public string Name { get; init; } = string.Empty;
}

internal sealed class ListKnowledgeBasesResponse
{
    public IReadOnlyList<KnowledgeBase> KnowledgeBases { get; init; } = [];
}
