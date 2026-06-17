namespace GradientLabs.Api;

public class GradientLabsException : Exception
{
    public GradientLabsException(string message) : base(message) { }
    public GradientLabsException(string message, Exception inner) : base(message, inner) { }
}
