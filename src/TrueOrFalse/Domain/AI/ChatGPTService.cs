using OpenAI.Chat;
using System.Text.Json;
using static AiFlashCard;

public static class ChatGPTService
{
    public static async Task<List<FlashCard>> GenerateFlashcardsAsync(string text, int pageId,
        PermissionCheck permissionCheck)
    {
        ChatClient client = new(model: Settings.OpenAIModel, apiKey: Settings.OpenAIApiKey);
        var prompt = GetPrompt(text, pageId, permissionCheck);
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