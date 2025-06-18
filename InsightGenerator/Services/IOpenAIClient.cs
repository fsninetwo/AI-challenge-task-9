using System.Threading.Tasks;

namespace InsightGenerator;

public interface IOpenAIClient
{
    Task<string> GetCompletionAsync(string prompt, double temperature = 0.2);
} 