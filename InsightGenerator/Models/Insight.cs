namespace InsightGenerator.Models;
using System.Text.Json.Serialization;

public sealed record Insight(
    [property: JsonPropertyName("service_name")] string ServiceName,
    [property: JsonPropertyName("brief_history")] string BriefHistory,
    [property: JsonPropertyName("target_audience")] string TargetAudience,
    [property: JsonPropertyName("core_features")] List<string> CoreFeatures,
    [property: JsonPropertyName("unique_selling_points")] string UniqueSellingPoints,
    [property: JsonPropertyName("business_model")] string BusinessModel,
    [property: JsonPropertyName("tech_stack_insights")] string TechStackInsights,
    [property: JsonPropertyName("perceived_strengths")] string PerceivedStrengths,
    [property: JsonPropertyName("perceived_weaknesses")] string PerceivedWeaknesses
); 