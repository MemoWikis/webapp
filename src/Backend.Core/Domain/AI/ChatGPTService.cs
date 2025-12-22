using OpenAI.Chat;
using System.Text.Json;
using static AiFlashCard;

public static class ChatGPTService
{
    /// <summary>
    /// Gets a response from OpenAI ChatGPT API
    /// </summary>
    public static async Task<ChatGptResponse?> GetChatGptResponse(string prompt, string? modelOverride = null)
    {
        try
        {
            var model = modelOverride ?? Settings.OpenAIModel;
            ChatClient client = new(model: model, apiKey: Settings.OpenAIApiKey);

            ChatCompletion chatCompletion = await client.CompleteChatAsync(prompt);

            if (chatCompletion.Content.Count == 0 || string.IsNullOrEmpty(chatCompletion.Content[0].Text))
                return null;

            return new ChatGptResponse
            {
                Role = "assistant",
                Text = chatCompletion.Content[0].Text,
                Model = chatCompletion.Model,
                InputTokens = chatCompletion.Usage?.InputTokenCount ?? 0,
                OutputTokens = chatCompletion.Usage?.OutputTokenCount ?? 0
            };
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Error while calling ChatGPT API with model {Model}", modelOverride ?? Settings.OpenAIModel);
            return null;
        }
    }

    public static async Task<List<FlashCard>> GenerateFlashcardsAsync(string prompt)
    {
        ChatClient client = new(model: Settings.OpenAIModel, apiKey: Settings.OpenAIApiKey);

        ChatCompletion chatCompletion = await client.CompleteChatAsync(prompt);

        if (chatCompletion.Content.Count == 0 || string.IsNullOrEmpty(chatCompletion.Content[0].Text))
            return new List<FlashCard>();

        try
        {
            var flashCards = JsonSerializer.Deserialize<List<FlashCard>>(chatCompletion.Content[0].Text);
            return flashCards ?? new List<FlashCard>();
        }
        catch (JsonException)
        {
            return new List<FlashCard>();
        }
    }
}

/// <summary>
/// Response from ChatGPT API
/// </summary>
public class ChatGptResponse
{
    public string Role { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int InputTokens { get; set; }
    public int OutputTokens { get; set; }
}