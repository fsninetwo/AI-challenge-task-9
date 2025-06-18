using System.Text.Json;

namespace InsightGenerator;

internal sealed class Program
{
    private static async Task<int> Main(string[] args)
    {
        try
        {
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: An OpenAI API key was not found. Please set the OPENAI_API_KEY environment variable and retry.");
                Console.ResetColor();
                return 1;
            }

            Console.WriteLine("===================================================");
            Console.WriteLine("  Digital Service Insight Generator (OpenAI GPT)  ");
            Console.WriteLine("===================================================\n");

            // Ask the user which input mode they prefer
            Console.WriteLine("Select input mode:");
            Console.WriteLine("1) Paste description text (e.g., an 'About Us' section)");
            Console.WriteLine("2) Enter the service\'s name");
            Console.Write("Your choice (1 or 2): ");

            string? choice;
            do
            {
                choice = Console.ReadLine();
            } while (choice != "1" && choice != "2");

            string userInput;
            if (choice == "1")
            {
                Console.WriteLine("\nPaste the service description below. Finish with a single blank line:");
                userInput = ReadMultiLineInput();
            }
            else
            {
                Console.Write("\nEnter the service name: ");
                userInput = Console.ReadLine() ?? string.Empty;
            }

            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Input cannot be empty.");
                Console.ResetColor();
                return 1;
            }

            var prompt = BuildPrompt(userInput);

            Console.WriteLine("\nGenerating insights with OpenAI...\n");

            using var client = new OpenAIClient(apiKey);
            var completion = await client.GetCompletionAsync(prompt);

            Console.WriteLine("Raw AI Output:\n----------------");
            Console.WriteLine(completion);

            Console.WriteLine("\nParsed Insights:\n----------------");
            if (TryParseInsights(completion, out var insights))
            {
                DisplayInsights(insights);
            }
            else
            {
                Console.WriteLine("Unable to parse JSON. Displaying raw output above.");
            }

            return 0;
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Unexpected error: {ex.Message}\n{ex.StackTrace}");
            Console.ResetColor();
            return -1;
        }
    }

    private static string ReadMultiLineInput()
    {
        var lines = new List<string>();
        string? line;
        while (true)
        {
            line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
                break;
            lines.Add(line);
        }

        return string.Join('\n', lines);
    }

    private static string BuildPrompt(string input)
    {
        return $$"""
        As an expert product analyst, extract concise, structured insights about a digital service.

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

    private static bool TryParseInsights(string json, out Insight? insights)
    {
        try
        {
            insights = JsonSerializer.Deserialize<Insight>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return insights != null;
        }
        catch
        {
            insights = null;
            return false;
        }
    }

    private static void DisplayInsights(Insight insights)
    {
        void WriteHeader(string header)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(header);
            Console.ResetColor();
        }

        WriteHeader($"Service: {insights.ServiceName}\n");
        WriteHeader("Overview:");
        Console.WriteLine(insights.Overview + "\n");

        WriteHeader("Unique Value Proposition:");
        Console.WriteLine(insights.UniqueValueProposition + "\n");

        WriteHeader("Target Audience:");
        Console.WriteLine(insights.TargetAudience + "\n");

        WriteHeader("Monetization Model:");
        Console.WriteLine(insights.MonetizationModel + "\n");

        WriteHeader("Key Features:");
        foreach (var feature in insights.KeyFeatures ?? new List<string>())
            Console.WriteLine($" - {feature}");
        Console.WriteLine();

        WriteHeader("Growth Opportunities:");
        Console.WriteLine(insights.GrowthOpportunities + "\n");

        WriteHeader("Risks & Challenges:");
        Console.WriteLine(insights.RisksChallenges + "\n");
    }

    private record Insight(
        string ServiceName,
        string Overview,
        string UniqueValueProposition,
        string TargetAudience,
        string MonetizationModel,
        List<string> KeyFeatures,
        string GrowthOpportunities,
        string RisksChallenges
    );
} 