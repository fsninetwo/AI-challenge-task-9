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
        var (text, isServiceName) = _inputProvider.GetInput();
        if (string.IsNullOrWhiteSpace(text))
        {
            Fail("Input cannot be empty.");
            return 1;
        }

        var prompt = PromptBuilder.Build(text, isServiceName);

        var temperatureStr = _configuration["OpenAI:Temperature"];
        _ = double.TryParse(temperatureStr, out var temperature);
        if (temperature <= 0)
            temperature = 0.2;

        _outputRenderer.RenderRaw("Generating insights with OpenAI...\n");

        var completion = await _openAI.GetCompletionAsync(prompt, temperature);

        _outputRenderer.RenderRaw(completion);

        if (InsightParser.TryParse(completion, out var insights) && insights != null)
        {
            _outputRenderer.RenderInsights(insights);
        }
        else
        {
            Console.WriteLine("Unable to parse JSON. Displaying raw output above.");
        }

        return 0;
    }

    private static void Fail(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Error: " + message);
        Console.ResetColor();
    }
} 