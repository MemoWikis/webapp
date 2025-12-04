using System.Text;
using System.Text.Json;
using static AiFlashCard;

public static class ClaudeService
{
    private static readonly string ClaudeSonnetModel = Settings.AnthropicModel;

    private static readonly HttpClient httpClient = new HttpClient
    {
        BaseAddress = new Uri("https://api.anthropic.com"),
        Timeout = TimeSpan.FromMinutes(5)
    };

    static ClaudeService()
    {
        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Add("x-api-key", Settings.AnthropicApiKey);
        httpClient.DefaultRequestHeaders.Add("anthropic-version", Settings.AnthropicVersion);
    }

    public static async Task<AnthropicApiResponse?> GetClaudeResponse(string prompt)
    {
        var requestJson = GetRequestJson(prompt, ClaudeSonnetModel);

        using var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
        try
        {
            var response = await httpClient.PostAsync("/v1/messages", content);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AnthropicApiResponse>(responseBody);
        }
        catch (HttpRequestException ex)
        {
            Log.Error(ex, "Error while calling Claude API");
            return null;
        }
        catch (JsonException)
        {
            Log.Error("Error while deserializing Claude API response");
            return null;
        }
    }

    private static string GetRequestJson(string promptContent, string model = "")
    {
        if (string.IsNullOrWhiteSpace(model))
            model = Settings.AnthropicModel;

        var request = new
        {
            model,
            max_tokens = 8192,
            messages = new[]
            {
                new
                {
                    role = "user",
                    content = promptContent
                }
            }
        };

        return JsonSerializer.Serialize(request);
    }

    public static async Task<List<FlashCard>> GenerateFlashcardsAsync(string promptContent, int userId, int pageId, AiUsageLogRepo aiUsageLogRepo)
    {
        var response = await GetClaudeResponse(promptContent);

        if (response != null)
        {
            aiUsageLogRepo.AddUsage(response, userId, pageId);
        }

        if (response is { Role: "assistant", Content.Count: > 0 }
            && !string.IsNullOrWhiteSpace(response.Content[0].Text))
        {
            var flashCards = JsonSerializer.Deserialize<List<FlashCard>>(response.Content[0].Text);
            return flashCards ?? new List<FlashCard>();
        }

        return new List<FlashCard>();
    }
}
