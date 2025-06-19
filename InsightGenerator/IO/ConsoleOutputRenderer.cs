using InsightGenerator.Models;

namespace InsightGenerator.IO;

internal sealed class ConsoleOutputRenderer : IOutputRenderer
{
    public void RenderRaw(string raw)
    {
        Console.WriteLine("Raw AI Output:\n----------------");
        Console.WriteLine(raw);
    }

    public void RenderInsights(Insight insights)
    {
        Console.WriteLine("\nParsed Insights:\n----------------");
        void WriteHeader(string header)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(header);
            Console.ResetColor();
        }

        WriteHeader($"Service: {insights.ServiceName}\n");

        WriteHeader("Brief History:");
        Console.WriteLine(insights.BriefHistory + "\n");

        WriteHeader("Target Audience:");
        Console.WriteLine(insights.TargetAudience + "\n");

        WriteHeader("Core Features:");
        foreach (var feature in insights.CoreFeatures ?? new List<string>())
            Console.WriteLine($" - {feature}");
        Console.WriteLine();

        WriteHeader("Unique Selling Points:");
        Console.WriteLine(insights.UniqueSellingPoints + "\n");

        WriteHeader("Business Model:");
        Console.WriteLine(insights.BusinessModel + "\n");

        WriteHeader("Tech Stack Insights:");
        Console.WriteLine(insights.TechStackInsights + "\n");

        WriteHeader("Perceived Strengths:");
        Console.WriteLine(insights.PerceivedStrengths + "\n");

        WriteHeader("Perceived Weaknesses:");
        Console.WriteLine(insights.PerceivedWeaknesses + "\n");
    }
} 