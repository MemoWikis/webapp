using System.Net;
using System.Text;
using System.Text.Json;
using static AiFlashCard;

public static class ClaudeService
{
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

    /// <summary>
    /// Gets a response from Claude API with automatic fallback to alternative model
    /// if the primary model fails (e.g., model deprecated or unavailable)
    /// </summary>
    public static async Task<AnthropicApiResponse?> GetClaudeResponse(string prompt, string? modelOverride = null)
    {
        var primaryModel = modelOverride ?? Settings.AnthropicModel;
        var fallbackModel = Settings.AnthropicFallbackModel;

        // Try primary model first
        var response = await TryGetClaudeResponse(prompt, primaryModel);
        
        if (response != null)
        {
            return response;
        }

        // If primary fails and we have a different fallback, try it
        if (!string.IsNullOrEmpty(fallbackModel) && fallbackModel != primaryModel)
        {
            Log.Warning("Primary model {PrimaryModel} failed, trying fallback {FallbackModel}", 
                primaryModel, fallbackModel);
            return await TryGetClaudeResponse(prompt, fallbackModel);
        }

        return null;
    }

    private static async Task<AnthropicApiResponse?> TryGetClaudeResponse(string prompt, string model)
    {
        var requestJson = GetRequestJson(prompt, model);

        using var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
        try
        {
            var response = await httpClient.PostAsync("/v1/messages", content);
            
            // Check for model-related errors that should trigger fallback
            if (response.StatusCode == HttpStatusCode.NotFound || 
                response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                Log.Warning("Model {Model} returned {StatusCode}: {Error}", 
                    model, response.StatusCode, errorBody);
                return null;
            }
            
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AnthropicApiResponse>(responseBody);
        }
        catch (HttpRequestException ex)
        {
            Log.Error(ex, "Error while calling Claude API with model {Model}", model);
            return null;
        }
        catch (JsonException ex)
        {
            Log.Error(ex, "Error while deserializing Claude API response");
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
