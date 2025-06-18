using System.Text.Json;
using InsightGenerator.Models;

namespace InsightGenerator.Services;

internal static class InsightParser
{
    public static bool TryParse(string json, out Insight? insight)
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
} 