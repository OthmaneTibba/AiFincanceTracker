using AiFinanceTracker.Server.Functions;

using Azure.AI.Vision.ImageAnalysis;
using Microsoft.Azure.Functions.Worker;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenAI_API;
using System.Security.Policy;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication(builder =>
    {
        builder.UseMiddleware<ExceptionHandler>();
       
      
    })
    .ConfigureServices(services =>
    {
        string? connectionString = Environment.GetEnvironmentVariable("CosmosDbConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentException($"{nameof(connectionString)} cannot be null");
        services.AddCosmosDbClient(connectionString);
        string? openaiKey = Environment.GetEnvironmentVariable("OpenaiKey");
        if(string.IsNullOrEmpty(openaiKey)) throw new ArgumentException($"{nameof(openaiKey)} cannot be null");
        OpenAIAPI openAIAPI = new(openaiKey);


        services.AddScoped(_ => openAIAPI);
        string? imageAnalysisKey = Environment.GetEnvironmentVariable("ImageAnalysisKey");
        string? imageAnalysisUrl = Environment.GetEnvironmentVariable("ImageAnalysisUrl");
        if (string.IsNullOrEmpty(imageAnalysisUrl)) throw new ArgumentException($"{nameof(imageAnalysisUrl)} cannot be null");
        if (string.IsNullOrEmpty(imageAnalysisKey)) throw new ArgumentException($"{nameof(imageAnalysisKey)} cannot be null");


        services.AddScoped(_ => new ImageAnalysisClient(new Uri(imageAnalysisUrl), new Azure.AzureKeyCredential(imageAnalysisKey)));
        services.AddRepositories();
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

host.Run();
