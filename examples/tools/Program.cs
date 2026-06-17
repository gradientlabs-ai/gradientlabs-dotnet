// Demonstrates: create a tool, read it back, delete it.
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

var toolId = $"example-tool-{Guid.NewGuid():N}";

Console.WriteLine($"Creating tool {toolId}...");
var tool = await client.Tools.CreateAsync(new CreateToolRequest
{
    Id = toolId,
    Name = "example_lookup",
    Description = "Looks up example data.",
    Parameters =
    [
        new ToolParameter
        {
            Name = "query",
            Description = "The search query.",
            Type = ParameterType.String,
            AllowedSources = [ParameterSource.Llm],
        }
    ],
});
Console.WriteLine($"Created: {tool.Id}");

Console.WriteLine("Reading back...");
var read = await client.Tools.ReadAsync(toolId);
Console.WriteLine($"Read: name={read.Name}");

Console.WriteLine("Deleting...");
await client.Tools.DeleteAsync(toolId);
Console.WriteLine("Done.");
