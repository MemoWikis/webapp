using System.Text.Json.Serialization;

/// <summary>
/// Represents an AI model's metadata and configuration
/// </summary>
public class AiModelInfo
{
    public string Id { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public AiModelProvider Provider { get; set; }
    
    /// <summary>
    /// Whether this is an alias (e.g., "claude-sonnet-4-latest") that auto-updates
    /// </summary>
    public bool IsAlias { get; set; }
    
    /// <summary>
    /// The actual model ID this alias points to (if IsAlias is true)
    /// </summary>
    public string? ResolvedModelId { get; set; }
    
    public int MaxOutputTokens { get; set; }
    public int ContextWindow { get; set; }
    
    /// <summary>
    /// Cost per 1M input tokens in USD
    /// </summary>
    public decimal InputTokenCostPer1M { get; set; }
    
    /// <summary>
    /// Cost per 1M output tokens in USD
    /// </summary>
    public decimal OutputTokenCostPer1M { get; set; }
    
    public bool IsEnabled { get; set; } = true;
    public bool IsDefault { get; set; }
    
    /// <summary>
    /// When this model was last verified as available from the provider
    /// </summary>
    public DateTime? LastVerified { get; set; }
    
    /// <summary>
    /// If model fetch failed, this contains the error
    /// </summary>
    public string? LastError { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Response from Anthropic's /v1/models endpoint
/// </summary>
public class AnthropicModelsResponse
{
    [JsonPropertyName("data")]
    public List<AnthropicModelData> Data { get; set; } = new();
    
    [JsonPropertyName("has_more")]
    public bool HasMore { get; set; }
    
    [JsonPropertyName("first_id")]
    public string? FirstId { get; set; }
    
    [JsonPropertyName("last_id")]
    public string? LastId { get; set; }
}

public class AnthropicModelData
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    
    [JsonPropertyName("display_name")]
    public string DisplayName { get; set; } = string.Empty;
    
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
    
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Response from OpenAI's /v1/models endpoint
/// </summary>
public class OpenAiModelsResponse
{
    [JsonPropertyName("object")]
    public string Object { get; set; } = string.Empty;
    
    [JsonPropertyName("data")]
    public List<OpenAiModelData> Data { get; set; } = new();
}

public class OpenAiModelData
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    
    [JsonPropertyName("object")]
    public string Object { get; set; } = string.Empty;
    
    [JsonPropertyName("created")]
    public long Created { get; set; }
    
    [JsonPropertyName("owned_by")]
    public string OwnedBy { get; set; } = string.Empty;
}
