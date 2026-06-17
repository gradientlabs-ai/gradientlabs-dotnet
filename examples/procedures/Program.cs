// Demonstrates: list procedures with cursor pagination, read a specific one.
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

Console.WriteLine("Listing all procedures (paginated)...");
string? cursor = null;
var total = 0;
string? firstId = null;
do
{
    var page = await client.Procedures.ListAsync(cursor: cursor);
    total += page.Items.Count;
    if (firstId is null && page.Items.Count > 0)
        firstId = page.Items[0].Id;
    Console.WriteLine($"  Page: {page.Items.Count} items, hasNext={page.HasNextPage}");
    cursor = page.Next;
} while (cursor is not null);

Console.WriteLine($"Total: {total} procedures.");

if (firstId is not null)
{
    Console.WriteLine($"Reading first procedure {firstId}...");
    var proc = await client.Procedures.ReadAsync(firstId);
    Console.WriteLine($"  Name={proc.Name} Status={proc.Status}");
}
