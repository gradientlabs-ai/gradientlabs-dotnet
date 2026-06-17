// Demonstrates: upsert an article, delete it.
using System.Text.Json;
using GradientLabs.Api;

var apiKey = Environment.GetEnvironmentVariable("GRADIENT_LABS_API_KEY")
    ?? throw new Exception("Set GRADIENT_LABS_API_KEY");
var baseUrl = Environment.GetEnvironmentVariable("GRADIENT_LABS_BASE_URL");
var supportPlatform = Environment.GetEnvironmentVariable("GRADIENT_LABS_SUPPORT_PLATFORM")
    ?? throw new Exception("Set GRADIENT_LABS_SUPPORT_PLATFORM (e.g. 'intercom')");

var options = new GradientLabsClientOptions
{
    ApiKey = apiKey,
    BaseUri = baseUrl is not null ? new Uri(baseUrl) : new Uri("https://api.gradient-labs.ai"),
};

using var client = new GradientLabsManagementClient(options);

var articleId = $"example-article-{Guid.NewGuid():N}";

Console.WriteLine($"Upserting article {articleId}...");
await client.Articles.UpsertAsync(supportPlatform, new UpsertArticleRequest
{
    Id = articleId,
    AuthorId = "author-001",
    Title = "Example Article",
    Body = "This is the article body.",
    Visibility = ArticleVisibility.Internal,
    Status = ArticleStatus.Published,
    Created = DateTimeOffset.UtcNow,
    LastEdited = DateTimeOffset.UtcNow,
});
Console.WriteLine("Upserted.");

Console.WriteLine("Deleting...");
await client.Articles.DeleteAsync(supportPlatform, articleId);
Console.WriteLine("Done.");
