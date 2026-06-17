namespace GradientLabs.Api;

public readonly record struct SupportPlatform(string Value)
{
    public static readonly SupportPlatform Dixa = new("dixa");
    public static readonly SupportPlatform Freshchat = new("freshchat");
    public static readonly SupportPlatform Freshdesk = new("freshdesk");
    public static readonly SupportPlatform Gmail = new("gmail");
    public static readonly SupportPlatform Intercom = new("intercom");
    public static readonly SupportPlatform Livechat = new("livechat");
    public static readonly SupportPlatform PublicApi = new("public-api");
    public static readonly SupportPlatform ChatSdk = new("chat-sdk");
    public static readonly SupportPlatform Salesforce = new("salesforce");
    public static readonly SupportPlatform Zendesk = new("zendesk");
    public static readonly SupportPlatform Livekit = new("livekit");
    public static readonly SupportPlatform Twilio = new("twilio");
    public static readonly SupportPlatform Talkdesk = new("talkdesk");
    public static readonly SupportPlatform IntercomVoice = new("intercom-voice");
    public static readonly SupportPlatform ConversationSynthesizor = new("conversation-synthesizor");
    public static readonly SupportPlatform WebApp = new("web-app");

    public override string ToString() => Value;
    public static implicit operator string(SupportPlatform v) => v.Value;
}
