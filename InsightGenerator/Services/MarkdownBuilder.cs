using InsightGenerator.Models;
using System.Text;

namespace InsightGenerator.Services;

internal static class MarkdownBuilder
{
    public static string Build(Insight insight)
    {
        var sb = new StringBuilder();

        // Header with service name and divider
        sb.AppendLine($"# {insight.ServiceName} – Digital Service Analysis");
        sb.AppendLine("\n---\n");

        // Brief History section with formatting for milestones
        sb.AppendLine("## 📅 Brief History");
        sb.AppendLine(FormatParagraph(insight.BriefHistory));
        sb.AppendLine();

        // Target Audience with bullet points if multiple segments
        sb.AppendLine("## 👥 Target Audience");
        var audiences = (insight.TargetAudience ?? string.Empty)
            .Split(',', ';')
            .Select(a => a.Trim())
            .Where(a => !string.IsNullOrWhiteSpace(a));
        
        if (audiences.Count() > 1)
        {
            foreach (var audience in audiences)
                sb.AppendLine($"- {audience}");
        }
        else
        {
            sb.AppendLine(FormatParagraph(insight.TargetAudience));
        }
        sb.AppendLine();

        // Core Features with numbered list
        sb.AppendLine("## ⭐ Core Features");
        if (insight.CoreFeatures?.Any() == true)
        {
            for (int i = 0; i < insight.CoreFeatures.Count; i++)
            {
                var feature = insight.CoreFeatures[i] ?? string.Empty;
                sb.AppendLine($"{i + 1}. {feature.TrimStart('-', '*', '•').Trim()}");
            }
        }
        sb.AppendLine();

        // Unique Selling Points with emphasis
        sb.AppendLine("## 🎯 Unique Selling Points");
        sb.AppendLine(FormatParagraph(insight.UniqueSellingPoints));
        sb.AppendLine();

        // Business Model with clear structure
        sb.AppendLine("## 💰 Business Model");
        sb.AppendLine(FormatParagraph(insight.BusinessModel));
        sb.AppendLine();

        // Tech Stack with bullet points if multiple technologies
        sb.AppendLine("## 🔧 Tech Stack Insights");
        var techItems = (insight.TechStackInsights ?? string.Empty)
            .Split(',', ';')
            .Select(t => t.Trim())
            .Where(t => !string.IsNullOrWhiteSpace(t));
        
        if (techItems.Count() > 1)
        {
            foreach (var tech in techItems)
                sb.AppendLine($"- {tech}");
        }
        else
        {
            sb.AppendLine(FormatParagraph(insight.TechStackInsights));
        }
        sb.AppendLine();

        // Strengths and Weaknesses in a clear comparison
        sb.AppendLine("## 📊 Strengths & Weaknesses");
        sb.AppendLine("\n### Strengths ✅");
        sb.AppendLine(FormatParagraph(insight.PerceivedStrengths));
        sb.AppendLine("\n### Weaknesses ⚠️");
        sb.AppendLine(FormatParagraph(insight.PerceivedWeaknesses));
        sb.AppendLine();

        // Footer
        sb.AppendLine("---");
        sb.AppendLine($"*Analysis generated at {DateTime.Now:yyyy-MM-dd HH:mm:ss}*");

        return sb.ToString();
    }

    private static string FormatParagraph(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return "_No information available_";

        // Clean up any markdown list markers at the start of lines
        return content.Split('\n')
            .Select(line => line.TrimStart('-', '*', '•').Trim())
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Aggregate((a, b) => a + "\n" + b);
    }
} 