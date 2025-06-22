using InsightGenerator.IO;
using InsightGenerator.Models;
using Microsoft.Extensions.Configuration;
using InsightGenerator;

namespace InsightGenerator.Services;

internal sealed class InsightGeneratorApp
{
    private readonly IInputProvider _inputProvider;
    private readonly IOutputRenderer _outputRenderer;
    private readonly IOpenAIClient _openAI;
    private readonly IConfiguration _configuration;

    public InsightGeneratorApp(IInputProvider inputProvider,
        IOutputRenderer outputRenderer,
        IOpenAIClient openAI,
        IConfiguration configuration)
    {
        _inputProvider = inputProvider;
        _outputRenderer = outputRenderer;
        _openAI = openAI;
        _configuration = configuration;
    }

    public async Task<int> RunAsync()
    {
        while (true)
        {
            var (text, isServiceName) = _inputProvider.GetInput();
            if (string.IsNullOrWhiteSpace(text))
            {
                _outputRenderer.RenderRaw("Goodbye!\n");
                break; // Exit requested
            }

            var prompt = PromptBuilder.Build(text, isServiceName);

            var temperatureStr = _configuration["OpenAI:Temperature"];
            _ = double.TryParse(temperatureStr, out var temperature);
            if (temperature <= 0)
                temperature = 0.2;

            _outputRenderer.RenderRaw("Generating insights with OpenAI → please wait\n\n");

            // Spinner while waiting
            using var cts = new CancellationTokenSource();
            var spinnerTask = Task.Run(() => Spinner(cts.Token));

            string completion;
            try
            {
                completion = await _openAI.GetCompletionAsync(prompt, temperature);
            }
            finally
            {
                cts.Cancel();
                await spinnerTask;
                Console.WriteLine();
            }

            _outputRenderer.RenderRaw(completion);

            if (InsightParser.TryParse(completion, out var insights) && insights != null)
            {

                // Fallback: if the parsed object lacks a valid service name, but the user provided
                // one explicitly, use the input as the service name so the markdown output is not blank.
                if (isServiceName && !string.IsNullOrWhiteSpace(text))
                {
                    var desired = text.Trim();
                    if (string.IsNullOrWhiteSpace(insights.ServiceName) ||
                        insights.ServiceName.Equals("unknown", StringComparison.OrdinalIgnoreCase) ||
                        !insights.ServiceName.Equals(desired, StringComparison.OrdinalIgnoreCase))
                    {
                        insights = insights with { ServiceName = desired };
                    }
                }

                _outputRenderer.RenderInsights(insights);
            }
            else
            {
                Console.WriteLine("Unable to parse JSON. Displaying raw output above.");
            }

            Console.WriteLine(); // blank line before next iteration
        }

        return 0;
    }

    private static void Fail(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Error: " + message);
        Console.ResetColor();
    }

    private static void Spinner(CancellationToken token)
    {
        var sequence = new[] { '|', '/', '-', '\\' };
        int idx = 0;
        while (!token.IsCancellationRequested)
        {
            Console.Write($"\r{sequence[idx++ % sequence.Length]} Generating...");
            Thread.Sleep(100);
        }
        // Clear spinner line when done
        Console.Write("\r                 \r");
    }
} 