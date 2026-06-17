// Demonstrates: start a conversation, add a message, read it, then cancel.
using GradientLabs.Api;

var apiKey = Environment.GetEnvironmentVariable("GRADIENT_LABS_API_KEY")
    ?? throw new Exception("Set GRADIENT_LABS_API_KEY");

var baseUrl = Environment.GetEnvironmentVariable("GRADIENT_LABS_BASE_URL");

var options = new GradientLabsClientOptions
{
    ApiKey = apiKey,
    BaseUri = baseUrl is not null ? new Uri(baseUrl) : new Uri("https://api.gradient-labs.ai"),
};

using var client = new GradientLabsIntegrationClient(options);

var conversationId = $"example-{Guid.NewGuid():N}";

Console.WriteLine($"Starting conversation {conversationId}...");
var conversation = await client.Conversations.StartAsync(new StartConversationRequest
{
    Id = conversationId,
    CustomerId = "example-customer-001",
    Channel = ConversationChannel.Web,
});
Console.WriteLine($"Created: {conversation.Id} status={conversation.Status}");

Console.WriteLine("Adding message...");
await client.Conversations.AddMessageAsync(conversationId, new AddMessageRequest
{
    Id = $"msg-{Guid.NewGuid():N}",
    Body = "Hello, I need help with my order.",
    ParticipantId = "example-customer-001",
    ParticipantType = ParticipantType.Customer,
});
Console.WriteLine("Message added.");

Console.WriteLine("Reading conversation...");
var read = await client.Conversations.ReadAsync(conversationId);
Console.WriteLine($"Read: status={read.Status}");

Console.WriteLine("Cancelling...");
await client.Conversations.CancelAsync(conversationId, new CancelConversationRequest { Reason = "example" });
Console.WriteLine("Done.");
