namespace GradientLabs.Api;

public readonly record struct CustomerSource(string Value)
{
    public static readonly CustomerSource Dixa = new("dixa");
    public static readonly CustomerSource Intercom = new("intercom");
    public static readonly CustomerSource Freshchat = new("freshchat");
    public static readonly CustomerSource Freshdesk = new("freshdesk");
    public static readonly CustomerSource PublicApi = new("public-api");
    public static readonly CustomerSource ChatSdk = new("chat-sdk");
    public static readonly CustomerSource Salesforce = new("salesforce");
    public static readonly CustomerSource Zendesk = new("zendesk");
    public static readonly CustomerSource Livekit = new("livekit");
    public static readonly CustomerSource Twilio = new("twilio");
    public static readonly CustomerSource Talkdesk = new("talkdesk");
    public static readonly CustomerSource IntercomVoice = new("intercom-voice");
    public static readonly CustomerSource Livechat = new("livechat");
    public static readonly CustomerSource WebApp = new("web-app");
    public static readonly CustomerSource Gmail = new("gmail");
    public static readonly CustomerSource File = new("file");

    public override string ToString() => Value;
    public static implicit operator string(CustomerSource v) => v.Value;
}
