using InsightGenerator.IO;
using InsightGenerator.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace InsightGenerator;

internal sealed class Program
{
    public static async Task<int> Main(string[] args)
    {
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(config =>
            {
                config.SetBasePath(AppContext.BaseDirectory)
                      .AddJsonFile("appsettings.json", optional: true)
                      .AddEnvironmentVariables();
            })
            .ConfigureLogging(logging =>
            {
                // Remove default console logger noise (e.g., HttpClient diagnostic messages)
                logging.ClearProviders();
                logging.AddSimpleConsole();
                logging.SetMinimumLevel(LogLevel.Warning);
            })
            .ConfigureServices((context, services) =>
            {
                IConfiguration config = context.Configuration;

                // Register configuration so it's injectable
                services.AddSingleton(config);

                // Register I/O modules
                services.AddSingleton<IInputProvider, ConsoleInputProvider>();
                services.AddSingleton<IOutputRenderer, MarkdownOutputRenderer>();

                // Register OpenAI client with HttpClientFactory and settings
                services.AddHttpClient<IOpenAIClient, OpenAIClient>(client => { });

                services.AddSingleton(provider =>
                {
                    var cfg = provider.GetRequiredService<IConfiguration>();
                    var apiKey = cfg["OpenAI:ApiKey"];
                    if (string.IsNullOrWhiteSpace(apiKey) || apiKey.Contains("${"))
                        apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
                    if (string.IsNullOrWhiteSpace(apiKey))
                    {
                        throw new InvalidOperationException("OpenAI API Key is missing. Configure it via appsettings.json or environment variable.");
                    }

                    var model = cfg["OpenAI:Model"] ?? "gpt-3.5-turbo";
                    var baseUrl = cfg["OpenAI:BaseUrl"];

                    // Resolve HttpClient via factory
                    var httpFactory = provider.GetRequiredService<IHttpClientFactory>();
                    var httpClient = httpFactory.CreateClient(nameof(OpenAIClient));

                    // Optional organization header if provided in configuration
                    var orgId = cfg["OpenAI:OrganizationId"];
                    if (!string.IsNullOrWhiteSpace(orgId))
                    {
                        httpClient.DefaultRequestHeaders.Add("OpenAI-Organization", orgId);
                    }

                    return new OpenAIClient(httpClient, apiKey, model, baseUrl) as IOpenAIClient;
                });

                // Register application orchestrator
                services.AddSingleton<InsightGeneratorApp>();
            })
            .Build();

        try
        {
            Console.WriteLine("===================================================");
            Console.WriteLine("  Digital Service Insight Generator (OpenAI GPT)  ");
            Console.WriteLine("===================================================\n");

            var app = host.Services.GetRequiredService<InsightGeneratorApp>();
            return await app.RunAsync();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Fatal error: {ex.Message}\n{ex.StackTrace}");
            Console.ResetColor();
            return -1;
        }
    }
} 