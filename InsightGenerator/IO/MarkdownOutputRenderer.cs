using InsightGenerator.Models;
using InsightGenerator.Services;
using Microsoft.Extensions.Configuration;

namespace InsightGenerator.IO;

internal sealed class MarkdownOutputRenderer : IOutputRenderer
{
    private readonly IConfiguration _config;

    public MarkdownOutputRenderer(IConfiguration config)
    {
        _config = config;
    }

    public void RenderRaw(string raw)
    {
        // No-op for raw
    }

    public void RenderInsights(Insight insights)
    {
        var markdown = MarkdownBuilder.Build(insights);

        // Print to console
        Console.WriteLine(markdown);

        // Save to file if path configured
        var path = _config["Output:FilePath"];
        if (!string.IsNullOrWhiteSpace(path))
        {
            try
            {
                File.WriteAllText(path, markdown);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nReport saved to {Path.GetFullPath(path)}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Could not write report file: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
} 