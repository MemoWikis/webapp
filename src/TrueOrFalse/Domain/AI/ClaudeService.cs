using System.Text;
using System.Text.Json;
using static AiFlashCard;

public static class ClaudeService
{
    private static readonly HttpClient httpClient = new HttpClient
    {
        BaseAddress = new Uri("https://api.anthropic.com")
    };

    static ClaudeService()
    {
        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Add("x-api-key", Settings.AnthropicApiKey);
        httpClient.DefaultRequestHeaders.Add("anthropic-version", Settings.AnthropicVersion);
    }

    public static async Task<AnthropicApiResponse?> GetClaudeResponse(string jsonData)
    {
        using var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
        try
        {
            var response = await httpClient.PostAsync("/v1/messages", content);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AnthropicApiResponse>(responseBody);
        }
        catch (HttpRequestException ex)
        {
            Logg.r.Error(ex, "Error while calling Claude API");
            return null;
        }
        catch (JsonException)
        {
            Logg.r.Error("Error while deserializing Claude API response");
            return null;
        }
    }

    public static string GetRequestJson(string promptContent, string model = "")
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

    public static async Task<List<FlashCard>> GenerateFlashcardsAsync(string promptContent, int userId, int pageId)
    {
        var jsonData = GetRequestJson(promptContent);
        var response = await GetClaudeResponse(jsonData);

        if (response is { Role: "assistant", Content.Count: > 0 }
            && !string.IsNullOrWhiteSpace(response.Content[0].Text))
        {
            var flashCards = JsonSerializer.Deserialize<List<FlashCard>>(response.Content[0].Text);
            return flashCards ?? new List<FlashCard>();
        }

        return new List<FlashCard>();
    }
}
