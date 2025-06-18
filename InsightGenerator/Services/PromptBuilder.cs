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
              "brief_history": string,
              "target_audience": string,
              "core_features": string[],
              "unique_selling_points": string,
              "business_model": string,
              "tech_stack_insights": string,
              "perceived_strengths": string,
              "perceived_weaknesses": string
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
              "brief_history": string,
              "target_audience": string,
              "core_features": string[],
              "unique_selling_points": string,
              "business_model": string,
              "tech_stack_insights": string,
              "perceived_strengths": string,
              "perceived_weaknesses": string
            }
            """;
        }
    }
} 