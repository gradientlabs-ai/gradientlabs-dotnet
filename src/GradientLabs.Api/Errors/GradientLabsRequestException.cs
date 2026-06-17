namespace GradientLabs.Api;

public sealed class GradientLabsRequestException : GradientLabsException
{
    public GradientLabsRequestException(string message, Exception inner)
        : base(message, inner) { }
}
