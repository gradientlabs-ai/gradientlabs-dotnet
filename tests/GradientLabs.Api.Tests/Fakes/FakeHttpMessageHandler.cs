using System.Net;
using System.Text;

namespace GradientLabs.Api.Tests.Fakes;

internal sealed class FakeHttpMessageHandler : HttpMessageHandler
{
    private HttpResponseMessage _response;

    public HttpRequestMessage? LastRequest { get; private set; }
    public bool WasDisposed { get; private set; }

    public FakeHttpMessageHandler(
        HttpStatusCode status = HttpStatusCode.OK,
        string body = "{}",
        string contentType = "application/json")
    {
        _response = new HttpResponseMessage(status)
        {
            Content = new StringContent(body, Encoding.UTF8, contentType)
        };
    }

    public void SetResponse(HttpStatusCode status, string body, string contentType = "application/json")
    {
        _response = new HttpResponseMessage(status)
        {
            Content = new StringContent(body, Encoding.UTF8, contentType)
        };
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        LastRequest = request;
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(_response);
    }

    protected override void Dispose(bool disposing)
    {
        WasDisposed = true;
        base.Dispose(disposing);
    }
}

internal sealed class ThrowingHttpMessageHandler : HttpMessageHandler
{
    private readonly Exception _exception;

    public ThrowingHttpMessageHandler(Exception exception)
    {
        _exception = exception;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        => Task.FromException<HttpResponseMessage>(_exception);
}
