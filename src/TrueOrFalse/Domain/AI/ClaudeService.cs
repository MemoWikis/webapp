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

    public static async Task<List<FlashCard>> GenerateFlashcardsAsync(string promptContent)
    {
        var jsonData = GetRequestJson(promptContent);
        using var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
        try
        {
            var response = await httpClient.PostAsync("https://api.anthropic.com/v1/messages", content);

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            var responseJson = JsonSerializer.Deserialize<AnthropicApiResponse>(responseBody);
            Console.WriteLine(responseBody);

            if (responseJson?.Role == "assistant"
                && responseJson.Content?.Count > 0
                && !string.IsNullOrWhiteSpace(responseJson.Content[0].Text))
            {
                var flashCards = JsonSerializer.Deserialize<List<FlashCard>>(responseJson.Content[0].Text);
                return flashCards ?? new List<FlashCard>();
            }

            return new List<FlashCard>();
        }
        catch (HttpRequestException ex)
        {
            return new List<FlashCard>();
        }
        catch (JsonException)
        {
            return new List<FlashCard>();
        }
    }
}
