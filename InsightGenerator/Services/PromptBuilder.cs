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

            Response instructions:
            • Produce ONLY a single valid JSON object (no markdown, no triple back-ticks).
            • Carefully read the source content and derive each field from it whenever possible.
            • Only use the string "unknown" if the source truly lacks the information; do NOT default to "unknown" for every field.
            • Do not invent facts that contradict the source.
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

            Guidelines:
            • Produce ONLY the JSON object – no markdown, no triple back-ticks.
            • Fill each field drawing from the description; you may infer logical details (e.g., target audience for a music streaming service is "music listeners").
            • Use the string "unknown" ONLY if the description provides no clues and inference would be unreasonable.
            • Ensure "core_features" is a JSON array of short strings.
            - If the service name is not explicitly provided, create a concise descriptive placeholder such as "Unnamed Digital Service" instead of "unknown".
            """;
        }
    }
} 