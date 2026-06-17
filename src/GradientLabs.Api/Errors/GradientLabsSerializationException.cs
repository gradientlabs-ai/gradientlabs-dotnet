namespace GradientLabs.Api;

public sealed class GradientLabsSerializationException : GradientLabsException
{
    public GradientLabsSerializationException(string message, Exception inner)
        : base(message, inner) { }
}
