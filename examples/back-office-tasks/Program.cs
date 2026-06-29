// Demonstrates: create a back-office task, read it back.
using System.Text.Json;
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

var taskId = $"example-task-{Guid.NewGuid():N}";
Console.WriteLine($"Creating back-office task {taskId}...");

// Replace with one of your configured agents and a procedure within it.
var task = await client.BackOfficeTasks.CreateAsync(new CreateBackOfficeTaskRequest
{
    Id = taskId,
    AgentId = "agent_12345",
    ProcedureId = "proc_12345",
    Input = new { request = "Please process this example task." },
});
Console.WriteLine($"Created: {task.Id} status={task.Status}");

Console.WriteLine("Reading back...");
var read = await client.BackOfficeTasks.ReadAsync(taskId);
Console.WriteLine($"Read: status={read.Status}");
Console.WriteLine("Done.");
