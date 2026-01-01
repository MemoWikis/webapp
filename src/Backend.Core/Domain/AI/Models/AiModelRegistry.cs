using System.Net.Http.Headers;
using System.Text.Json;

/// <summary>
/// Registry for AI models. Provides methods to fetch available models from providers
/// and returns whitelisted models from cache (with DB fallback).
/// </summary>
public class AiModelRegistry(AiModelWhitelistRepo _whitelistRepo) : IRegisterAsInstancePerLifetime
{
    private readonly HttpClient _httpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(30)
    };

    /// <summary>
    /// Get all enabled/whitelisted models (from cache)
    /// </summary>
    public List<AiModelWhitelist> GetEnabledModels()
    {
        if (AiModelCache.IsInitialized)
            return AiModelCache.GetEnabled();

        return _whitelistRepo.GetEnabled();
    }

    /// <summary>
    /// Get all whitelisted models (from cache, for admin)
    /// </summary>
    public List<AiModelWhitelist> GetAllModels()
    {
        if (AiModelCache.IsInitialized)
            return AiModelCache.GetAll();

        return _whitelistRepo.GetAll();
    }


    /// <summary>
    /// Get model by ID (from cache)
    /// </summary>
    public AiModelWhitelist? GetModel(string modelId)
    {
        if (AiModelCache.IsInitialized)
            return AiModelCache.GetByModelId(modelId);

        return _whitelistRepo.GetByModelId(modelId);
    }

    /// <summary>
    /// Get token cost multiplier for a model (defaults to 1 if not found)
    /// </summary>
    public decimal GetTokenCostMultiplier(string modelId)
    {
        var model = GetModel(modelId);
        return model?.TokenCostMultiplier ?? 1m;
    }

    /// <summary>
    /// Check if a model is whitelisted and enabled
    /// </summary>
    public bool IsModelAllowed(string modelId)
    {
        var model = GetModel(modelId);
        return model?.IsEnabled ?? false;
    }

    /// <summary>
    /// Fetch all available models from Anthropic API (for admin to import)
    /// </summary>
    public async Task<List<FetchedModel>> FetchAnthropicModelsAsync()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.anthropic.com/v1/models");
            request.Headers.Add("x-api-key", Settings.AnthropicApiKey);
            request.Headers.Add("anthropic-version", Settings.AnthropicVersion);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<AnthropicModelsResponse>(json);

            if (apiResponse?.Data == null)
            {
                return new List<FetchedModel>();
            }

            return apiResponse.Data.Select(m => new FetchedModel
            {
                ModelId = m.Id,
                DisplayName = m.DisplayName,
                Provider = AiModelProvider.Anthropic
            }).ToList();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to fetch Anthropic models");
            return new List<FetchedModel>();
        }
    }

    /// <summary>
    /// Fetch all available models from OpenAI API (for admin to import)
    /// </summary>
    public async Task<List<FetchedModel>> FetchOpenAiModelsAsync()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.openai.com/v1/models");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Settings.OpenAIApiKey);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<OpenAiModelsResponse>(json);

            if (apiResponse?.Data == null)
            {
                return new List<FetchedModel>();
            }

            // Filter to only chat models
            return apiResponse.Data
                .Where(m => m.Id.StartsWith("gpt-") || m.Id.StartsWith("o1") || m.Id.StartsWith("o3") || m.Id.StartsWith("chatgpt"))
                .Select(m => new FetchedModel
                {
                    ModelId = m.Id,
                    DisplayName = FormatOpenAiDisplayName(m.Id),
                    Provider = AiModelProvider.OpenAI
                }).ToList();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to fetch OpenAI models");
            return new List<FetchedModel>();
        }
    }

    private string FormatOpenAiDisplayName(string modelId) => modelId switch
    {
        "gpt-4o" => "GPT-4o",
        "gpt-4o-mini" => "GPT-4o Mini",
        "gpt-4-turbo" => "GPT-4 Turbo",
        "o1" => "o1 (Reasoning)",
        "o1-mini" => "o1 Mini",
        _ => modelId
    };
}

/// <summary>
/// Model info fetched from provider API
/// </summary>
public class FetchedModel
{
    public string ModelId { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public AiModelProvider Provider { get; set; }
}
