namespace GradientLabs.Api;

public sealed class SetProcedureLimitRequest
{
    public bool? HasDailyLimit { get; init; }
    public long? MaxDailyConversations { get; init; }
}

public sealed class SetGatedVersionRequest
{
    public long MaxDailyConversations { get; init; }
    public bool Replace { get; init; }
}
