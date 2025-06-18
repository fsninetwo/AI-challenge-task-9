namespace InsightGenerator.Services;

internal static class PromptBuilder
{
    public static string Build(string input, bool isServiceName)
    {
        if (isServiceName)
        {
            return $$"""
            You are an expert product strategist.

            Task: Provide concise, structured insights about the digital service **{input}** based on your existing knowledge (do NOT assume additional context from the user).

            Respond ONLY in valid JSON with the following schema:
            {
              "service_name": string,
              "overview": string,
              "unique_value_proposition": string,
              "target_audience": string,
              "monetization_model": string,
              "key_features": string[],
              "growth_opportunities": string,
              "risks_challenges": string
            }
            """;
        }
        else
        {
            return $$"""
            As an expert product analyst, extract concise, structured insights about a digital service from the text provided below.

            Input:
            ###
            {input}
            ###

            Respond ONLY in valid JSON with the following schema:
            {
              "service_name": string,
              "overview": string,
              "unique_value_proposition": string,
              "target_audience": string,
              "monetization_model": string,
              "key_features": string[],
              "growth_opportunities": string,
              "risks_challenges": string
            }
            """;
        }
    }
} 