namespace GradientLabs.Api;

public class GradientLabsWebhookException : GradientLabsException
{
    public GradientLabsWebhookException(string message) : base(message) { }
    public GradientLabsWebhookException(string message, Exception inner) : base(message, inner) { }
}

public sealed class GradientLabsWebhookSignatureException : GradientLabsWebhookException
{
    public GradientLabsWebhookSignatureException(string message) : base(message) { }
}
