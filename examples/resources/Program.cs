// Demonstrates: create a ResourceType, create a ResourceSource, then delete both.
using GradientLabs.Api;

var apiKey = Environment.GetEnvironmentVariable("GRADIENT_LABS_API_KEY")
    ?? throw new Exception("Set GRADIENT_LABS_API_KEY");
var baseUrl = Environment.GetEnvironmentVariable("GRADIENT_LABS_BASE_URL");

var options = new GradientLabsClientOptions
{
    ApiKey = apiKey,
    BaseUri = baseUrl is not null ? new Uri(baseUrl) : new Uri("https://api.gradient-labs.ai"),
};

using var client = new GradientLabsManagementClient(options);

Console.WriteLine("Creating ResourceSource...");
var source = await client.ResourceSources.CreateAsync(new CreateResourceSourceRequest
{
    DisplayName = "Example Source",
    SourceType = ResourceSourceType.Webhook,
    Description = "Example webhook resource source.",
});
Console.WriteLine($"Created source: {source.Id}");

Console.WriteLine("Creating ResourceType...");
var type = await client.ResourceTypes.CreateAsync(new CreateResourceTypeRequest
{
    DisplayName = "Example Type",
    Scope = ResourceTypeScope.Global,
    RefreshStrategy = ResourceTypeRefreshStrategy.Static,
    Description = "Example resource type.",
    SourceConfig = new ResourceTypeSourceConfig
    {
        SourceId = source.Id,
        Attributes = ["name"],
        Cache = false,
    },
});
Console.WriteLine($"Created type: {type.Id}");

Console.WriteLine("Cleaning up...");
await client.ResourceTypes.DeleteAsync(type.Id);
await client.ResourceSources.DeleteAsync(source.Id);
Console.WriteLine("Done.");
