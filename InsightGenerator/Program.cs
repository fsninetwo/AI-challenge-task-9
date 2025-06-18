using InsightGenerator.IO;
using InsightGenerator.Services;

namespace InsightGenerator;

internal sealed class Program
{
    private static async Task<int> Main(string[] args)
    {
        try
        {
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                Fail("An OpenAI API key was not found. Please set the OPENAI_API_KEY environment variable and retry.");
                return 1;
            }

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

            using var client = new OpenAIClient(apiKey);
            var completion = await client.GetCompletionAsync(prompt);

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