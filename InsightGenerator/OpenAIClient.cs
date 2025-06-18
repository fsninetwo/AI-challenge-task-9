using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace InsightGenerator;

/// <summary>
/// Minimal wrapper around OpenAI's chat completion endpoint.
/// </summary>
internal sealed class OpenAIClient : IOpenAIClient, IDisposable
{
    private const string DefaultModel = "gpt-3.5-turbo";
    private readonly HttpClient _httpClient;
    private readonly string _model;

    public OpenAIClient(HttpClient httpClient, string apiKey, string? model = null)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("API key must be provided", nameof(apiKey));

        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        _model = string.IsNullOrWhiteSpace(model) ? DefaultModel : model!;
    }

    public async Task<string> GetCompletionAsync(string prompt, double temperature = 0.2)
    {
        var body = new
        {
            model = _model,
            messages = new[]
            {
                new { role = "system", content = "You are a helpful assistant that provides concise, structured analysis about digital services." },
                new { role = "user", content = prompt }
            },
            temperature
        };

        var json = JsonSerializer.Serialize(body);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        using var response = await _httpClient.PostAsync("chat/completions", content);

        var responseJson = await response.Content.ReadAsStringAsync();

        // If the request failed, attempt to surface a helpful error message to the caller.
        if (!response.IsSuccessStatusCode)
        {
            var status = (int)response.StatusCode;
            string? serverMessage = null;

            try
            {
                using var errDoc = JsonDocument.Parse(responseJson);
                if (errDoc.RootElement.TryGetProperty("error", out var errNode) &&
                    errNode.TryGetProperty("message", out var msgNode))
                {
                    serverMessage = msgNode.GetString();
                }
            }
            catch
            {
                // Ignore JSON parsing errors; we'll fall back to raw response body.
            }

            var reason = string.IsNullOrWhiteSpace(serverMessage) ? response.ReasonPhrase : serverMessage;
            throw new InvalidOperationException($"OpenAI request failed with status {status} ({response.ReasonPhrase}): {reason}");
        }

        using var doc = JsonDocument.Parse(responseJson);
        var root = doc.RootElement;

        var messageContent = root.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
        if (string.IsNullOrWhiteSpace(messageContent))
            throw new InvalidOperationException("Received empty completion from OpenAI.");

        return messageContent.Trim();
    }

    public void Dispose()
    {
        _httpClient.Dispose();
        GC.SuppressFinalize(this);
    }
} 