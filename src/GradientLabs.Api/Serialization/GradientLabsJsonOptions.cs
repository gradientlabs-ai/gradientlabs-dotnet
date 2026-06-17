using System.Text.Json;
using System.Text.Json.Serialization;

namespace GradientLabs.Api.Serialization;

internal static class GradientLabsJsonOptions
{
    public static readonly JsonSerializerOptions Default = Create();

    private static JsonSerializerOptions Create()
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = false,
        };

        options.Converters.Add(new NanosecondTimeSpanConverter());
        options.Converters.Add(new OpenStringConverter<ArticleStatus>(v => new(v)));
        options.Converters.Add(new OpenStringConverter<ArticleUsageStatus>(v => new(v)));
        options.Converters.Add(new OpenStringConverter<ArticleVisibility>(v => new(v)));
        options.Converters.Add(new OpenStringConverter<AttachmentType>(v => new(v)));
        options.Converters.Add(new OpenStringConverter<ConversationChannel>(v => new(v)));
        options.Converters.Add(new OpenStringConverter<CustomerSource>(v => new(v)));
        options.Converters.Add(new OpenStringConverter<ParticipantType>(v => new(v)));
        options.Converters.Add(new OpenStringConverter<ConversationEventType>(v => new(v)));
        options.Converters.Add(new OpenStringConverter<ProcedureStatus>(v => new(v)));
        options.Converters.Add(new OpenStringConverter<NoteStatus>(v => new(v)));
        options.Converters.Add(new OpenStringConverter<BackOfficeTaskStatus>(v => new(v)));
        options.Converters.Add(new OpenStringConverter<ResourceSourceBodyEncoding>(v => new(v)));
        options.Converters.Add(new OpenStringConverter<AttributeCardinality>(v => new(v)));
        options.Converters.Add(new OpenStringConverter<AttributeType>(v => new(v)));
        options.Converters.Add(new OpenStringConverter<ResourceTypeRefreshStrategy>(v => new(v)));
        options.Converters.Add(new OpenStringConverter<ResourceTypeScope>(v => new(v)));
        options.Converters.Add(new OpenStringConverter<ResourceSourceType>(v => new(v)));
        options.Converters.Add(new OpenStringConverter<SchemaUpdateStrategy>(v => new(v)));
        options.Converters.Add(new OpenStringConverter<SupportPlatform>(v => new(v)));
        options.Converters.Add(new OpenStringConverter<BodyEncoding>(v => new(v)));
        options.Converters.Add(new OpenStringConverter<ParameterSource>(v => new(v)));
        options.Converters.Add(new OpenStringConverter<ParameterType>(v => new(v)));
        options.Converters.Add(new OpenStringConverter<GradientLabsWebhookEventType>(v => new(v)));

        return options;
    }
}
