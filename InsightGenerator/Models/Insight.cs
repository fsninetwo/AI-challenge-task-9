namespace InsightGenerator.Models;

public sealed record Insight(
    string ServiceName,
    string BriefHistory,
    string TargetAudience,
    List<string> CoreFeatures,
    string UniqueSellingPoints,
    string BusinessModel,
    string TechStackInsights,
    string PerceivedStrengths,
    string PerceivedWeaknesses
); 