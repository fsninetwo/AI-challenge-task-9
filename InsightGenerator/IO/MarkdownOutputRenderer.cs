using InsightGenerator.Models;
using InsightGenerator.Services;
using Microsoft.Extensions.Configuration;

namespace InsightGenerator.IO;

internal sealed class MarkdownOutputRenderer : IOutputRenderer
{
    private readonly IConfiguration _config;
    private static readonly object _fileLock = new object();

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
                // Use a lock to prevent multiple threads from writing simultaneously
                lock (_fileLock)
                {
                    // Add timestamp separator
                    var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    var serviceLabel = string.IsNullOrWhiteSpace(insights.ServiceName) ? "(unknown)" : insights.ServiceName;
                    var separator = $"\n\n---\n### Generated at {timestamp} | Input: {serviceLabel}\n\n";
                    
                    // Append the new content with timestamp
                    File.AppendAllText(path, separator + markdown);
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nResults appended to {Path.GetFullPath(path)}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Could not write to output file: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
} 