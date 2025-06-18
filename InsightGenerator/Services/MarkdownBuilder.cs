using InsightGenerator.Models;
using System.Text;

namespace InsightGenerator.Services;

internal static class MarkdownBuilder
{
    public static string Build(Insight insight)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"# {insight.ServiceName} – Product Insight Report\n");

        AppendSection("Brief History", insight.BriefHistory);
        AppendSection("Target Audience", insight.TargetAudience);

        sb.AppendLine("## Core Features");
        foreach (var feat in insight.CoreFeatures ?? new List<string>())
            sb.AppendLine($"- {feat}");
        sb.AppendLine();

        AppendSection("Unique Selling Points", insight.UniqueSellingPoints);
        AppendSection("Business Model", insight.BusinessModel);
        AppendSection("Tech Stack Insights", insight.TechStackInsights);
        AppendSection("Perceived Strengths", insight.PerceivedStrengths);
        AppendSection("Perceived Weaknesses", insight.PerceivedWeaknesses);

        return sb.ToString();

        void AppendSection(string title, string? content)
        {
            sb.AppendLine($"## {title}");
            sb.AppendLine(content);
            sb.AppendLine();
        }
    }
} 