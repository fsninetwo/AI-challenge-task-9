namespace InsightGenerator.Models;

public sealed record Insight(
    string ServiceName,
    string Overview,
    string UniqueValueProposition,
    string TargetAudience,
    string MonetizationModel,
    List<string> KeyFeatures,
    string GrowthOpportunities,
    string RisksChallenges
); 