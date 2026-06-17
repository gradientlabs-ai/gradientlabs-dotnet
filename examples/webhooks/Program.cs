// Demonstrates: verify and parse an incoming webhook payload.
// Run a local HTTP server on port 4001 and send test payloads to it.
using System.Net;
using System.Security.Cryptography;
using System.Text;
using GradientLabs.Api;

var signingKey = Environment.GetEnvironmentVariable("GRADIENT_LABS_WEBHOOK_SIGNING_KEY")
    ?? throw new Exception("Set GRADIENT_LABS_WEBHOOK_SIGNING_KEY");

var verifier = new GradientLabsWebhookVerifier(signingKey);

var listener = new HttpListener();
listener.Prefixes.Add("http://localhost:4001/");
listener.Start();
Console.WriteLine("Listening on http://localhost:4001/ — send a POST request with a signed body.");
Console.WriteLine("Press Ctrl+C to stop.");

while (true)
{
    var context = await listener.GetContextAsync();
    var req = context.Request;
    var resp = context.Response;

    if (req.HttpMethod != "POST")
    {
        resp.StatusCode = 405;
        resp.Close();
        continue;
    }

    using var ms = new MemoryStream();
    await req.InputStream.CopyToAsync(ms);
    var body = ms.ToArray();

    var sigHeader = req.Headers["X-GradientLabs-Signature"] ?? string.Empty;
    var tokenHeader = req.Headers["X-GradientLabs-Token"];

    try
    {
        var evt = verifier.Parse(body, sigHeader, tokenHeader);
        Console.WriteLine($"Received event: id={evt.Id} type={evt.Type} seq={evt.SequenceNumber} token={evt.Token ?? "(none)"}");

        switch (evt)
        {
            case AgentMessageEvent msg:
                Console.WriteLine($"  AgentMessage: body={msg.Body} conversation={msg.Conversation.Id}");
                break;
            case ConversationHandOffEvent ho:
                Console.WriteLine($"  HandOff: reason={ho.Reason} conversation={ho.Conversation.Id}");
                break;
            case UnknownWebhookEvent unknown:
                Console.WriteLine($"  Unknown event type: {unknown.Type}");
                break;
        }

        resp.StatusCode = 200;
    }
    catch (GradientLabsWebhookSignatureException ex)
    {
        Console.WriteLine($"Signature verification failed: {ex.Message}");
        resp.StatusCode = 401;
    }

    resp.Close();
}
