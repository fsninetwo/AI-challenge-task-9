using InsightGenerator.IO;
using InsightGenerator.Services;
using Microsoft.Extensions.Configuration;

namespace InsightGenerator;

internal sealed class Program
{
    private static async Task<int> Main(string[] args)
    {
        try
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var apiKey = configuration["OpenAI:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey) || apiKey.Contains("${"))
                apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                Fail("OpenAI API key not found. Provide it via appsettings.json (OpenAI:ApiKey) or OPENAI_API_KEY env variable.");
                return 1;
            }

            var model = configuration["OpenAI:Model"] ?? "gpt-3.5-turbo";
            var temperatureStr = configuration["OpenAI:Temperature"];
            _ = double.TryParse(temperatureStr, out var temperature);
            if (temperature <= 0)
                temperature = 0.2;

            Console.WriteLine("===================================================");
            Console.WriteLine("  Digital Service Insight Generator (OpenAI GPT)  ");
            Console.WriteLine("===================================================\n");

            // Resolve modules
            IInputProvider inputProvider = new ConsoleInputProvider();
            IOutputRenderer outputRenderer = new ConsoleOutputRenderer();

            var (text, isServiceName) = inputProvider.GetInput();

            if (string.IsNullOrWhiteSpace(text))
            {
                Fail("Input cannot be empty.");
                return 1;
            }

            var prompt = PromptBuilder.Build(text, isServiceName);

            Console.WriteLine("\nGenerating insights with OpenAI...\n");

            using var client = new OpenAIClient(apiKey, model);
            var completion = await client.GetCompletionAsync(prompt, temperature);

            outputRenderer.RenderRaw(completion);

            if (InsightParser.TryParse(completion, out var insights) && insights != null)
            {
                outputRenderer.RenderInsights(insights);
            }
            else
            {
                Console.WriteLine("Unable to parse JSON. Displaying raw output above.");
            }

            return 0;
        }
        catch (Exception ex)
        {
            Fail($"Unexpected error: {ex.Message}\n{ex.StackTrace}");
            return -1;
        }
    }

    private static void Fail(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Error: " + message);
        Console.ResetColor();
    }
} 