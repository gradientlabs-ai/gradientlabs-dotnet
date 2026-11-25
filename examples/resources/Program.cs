// Demonstrates: create a ResourceSource and a ResourceType on a branch, then delete both.
// New resource sources and types are created on a branch (set GRADIENT_LABS_BRANCH_NAME) and go live when it is merged.
using GradientLabs.Api;

var apiKey = Environment.GetEnvironmentVariable("GRADIENT_LABS_API_KEY")
    ?? throw new Exception("Set GRADIENT_LABS_API_KEY");
var baseUrl = Environment.GetEnvironmentVariable("GRADIENT_LABS_BASE_URL");
var branchName = Environment.GetEnvironmentVariable("GRADIENT_LABS_BRANCH_NAME")
    ?? throw new Exception("Set GRADIENT_LABS_BRANCH_NAME to a branch created in the Gradient Labs app");

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
    BranchName = branchName,
    SourceType = ResourceSourceType.Webhook,
    Description = "Example webhook resource source.",
});
Console.WriteLine($"Created source: {source.Id}");

Console.WriteLine("Creating ResourceType...");
var type = await client.ResourceTypes.CreateAsync(new CreateResourceTypeRequest
{
    DisplayName = "Example Type",
    BranchName = branchName,
    Scope = ResourceTypeScope.Global,
    RefreshStrategy = ResourceTypeRefreshStrategy.Static,
    Description = "Example resource type.",
    SourceConfig = new ResourceTypeSourceConfig
    {
        SourceId = source.Id,
        Attributes = ["name"],
        Cache = "10m",
    },
});
Console.WriteLine($"Created type: {type.Id}");

Console.WriteLine("Cleaning up...");
await client.ResourceTypes.DeleteAsync(type.Id);
await client.ResourceSources.DeleteAsync(source.Id);
Console.WriteLine("Done.");
