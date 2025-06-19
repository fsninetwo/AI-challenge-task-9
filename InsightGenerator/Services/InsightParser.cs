using System.Text.Json;
using InsightGenerator.Models;

namespace InsightGenerator.Services;

internal static class InsightParser
{
    public static bool TryParse(string raw, out Insight? insight)
    {
        insight = null;

        if (string.IsNullOrWhiteSpace(raw))
            return false;

        // Remove markdown code fences if the model wrapped the JSON
        var cleaned = StripCodeFences(raw.Trim());

        try
        {
            if (TryDeserialize(cleaned, out var temp) ||
                TryDeserialize(ExtractJsonBlock(cleaned), out temp))
            {
                if (temp == null || string.IsNullOrWhiteSpace(temp.ServiceName) || string.IsNullOrWhiteSpace(temp.BriefHistory))
                    return false;

                insight = temp;
                return true;
            }
        }
        catch { }

        return false;
    }

    private static bool TryDeserialize(string json, out Insight? insight)
    {
        try
        {
            insight = JsonSerializer.Deserialize<Insight>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return insight != null;
        }
        catch
        {
            insight = null;
            return false;
        }
    }

    private static string ExtractJsonBlock(string text)
    {
        var firstBrace = text.IndexOf('{');
        var lastBrace = text.LastIndexOf('}');
        if (firstBrace >= 0 && lastBrace > firstBrace)
        {
            return text.Substring(firstBrace, lastBrace - firstBrace + 1);
        }
        return text;
    }

    private static string StripCodeFences(string text)
    {
        const string fence = "```";
        if (!text.StartsWith(fence))
            return text;

        // Find the first line break after the opening fence and the last fence occurrence.
        var firstNewline = text.IndexOf('\n');
        var lastFence = text.LastIndexOf(fence);

        if (firstNewline < 0 || lastFence <= firstNewline)
            return text; // malformed; return original

        return text.Substring(firstNewline + 1, lastFence - firstNewline - 1).Trim();
    }
} 