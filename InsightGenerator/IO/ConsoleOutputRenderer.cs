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

        WriteHeader("Overview:");
        Console.WriteLine(insights.Overview + "\n");

        WriteHeader("Unique Value Proposition:");
        Console.WriteLine(insights.UniqueValueProposition + "\n");

        WriteHeader("Target Audience:");
        Console.WriteLine(insights.TargetAudience + "\n");

        WriteHeader("Monetization Model:");
        Console.WriteLine(insights.MonetizationModel + "\n");

        WriteHeader("Key Features:");
        foreach (var feature in insights.KeyFeatures ?? new List<string>())
            Console.WriteLine($" - {feature}");
        Console.WriteLine();

        WriteHeader("Growth Opportunities:");
        Console.WriteLine(insights.GrowthOpportunities + "\n");

        WriteHeader("Risks & Challenges:");
        Console.WriteLine(insights.RisksChallenges + "\n");
    }
} 