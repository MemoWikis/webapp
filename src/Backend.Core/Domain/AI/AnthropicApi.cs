using System.Text.Json.Serialization;

/// <summary>
/// Represents the entire response from the Anthropic /v1/messages endpoint.
/// </summary>
public class AnthropicApiResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("content")]
    public List<ContentBlock> Content { get; set; }

    [JsonPropertyName("stop_reason")]
    public string StopReason { get; set; }

    [JsonPropertyName("stop_sequence")]
    public string StopSequence { get; set; }

    [JsonPropertyName("usage")]
    public Usage Usage { get; set; }
}

/// <summary>
/// Represents one item in the "content" array of the Anthropic response.
/// Typically, the type is "text" or "tool_use" or "tool_result".
/// </summary>
public class ContentBlock
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }
}

/// <summary>
/// Represents token usage details returned by Anthropic.
/// </summary>
public class Usage
{
    [JsonPropertyName("input_tokens")]
    public int InputTokens { get; set; }

    [JsonPropertyName("cache_creation_input_tokens")]
    public int? CacheCreationInputTokens { get; set; }

    [JsonPropertyName("cache_read_input_tokens")]
    public int? CacheReadInputTokens { get; set; }

    [JsonPropertyName("output_tokens")]
    public int OutputTokens { get; set; }
}
