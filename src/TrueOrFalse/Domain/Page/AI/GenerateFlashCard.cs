using OpenAI.Chat;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using TrueOrFalse;

public class AiFlashCard : IRegisterAsInstancePerLifetime
{
    public record struct FlashCard(string Front, string Back);

    public record struct BackJson(string Text);

    public static string GetPrompt(string sourceText, int pageId, PermissionCheck permissionCheck)
    {
        var existingFlashCards = GetFlashCardsOnPage(pageId, permissionCheck);
        var flashcards = JsonSerializer.Serialize(existingFlashCards);

        return @"
            Antworte mit Karteikarten als JSON-Array, das zwei Eigenschaften enthält: 'Front' und 'Back'. 
            Beispiel für JSON-Array: 
            [
                { 'Front': 'Was ist die Hauptstadt von Deutschland?', 'Back': 'Berlin' },
                { 'Front': 'Was ist die Hauptstadt von Frankreich?', 'Back': 'Paris' }
            ]
            Formatierung: Wichtige Worte können kursiv mit <em>, fett mit <strong> oder mit einem Unterstrich mit <u> per HTML-Tags formatiert werden. Listen sind auch möglich. Formatiere mit HTML-Tags.

            'Front':  
                - ist die Vorderseite der Karteikarte,
                - soll nur eine einzige Frage, ein Wort, einen Begriff, einen Satz oder eine Phrase enthalten,

            'Back'
               - ist dir Rücksteite der Karteikarte,
            
            Folgende Karteikarten exisiteren bereits: " +
               flashcards +
               @"
            Prüfe auf Duplikate. Die erstellten Karteikarten sollten keine Duplikate enthalten.
            Mache keine Erklärung, wrappe die Antwort nicht in einem Code-Block.
            Erstelle Karteikarten basierend auf diesen Text (nur fachlich passend):" +
               sourceText;
    }

    public static List<FlashCard> GetFlashCardsOnPage(int id, PermissionCheck permissionCheck)
    {
        var questions = EntityCache.GetQuestionsForPage(id)
            .Where(q => permissionCheck.CanView(q) && q.SolutionType == SolutionType.FlashCard);

        var flashcards = new List<FlashCard>();

        foreach (var question in questions)
        {
            // Strip HTML from the front
            var front = Regex.Replace(question.TextHtml, "<.*?>", string.Empty);

            var backJson = JsonSerializer.Deserialize<BackJson>(question.Solution);
            // Strip HTML from the back as well
            var back = Regex.Replace(backJson.Text, "<.*?>", string.Empty);

            if (!string.IsNullOrWhiteSpace(front) && !string.IsNullOrWhiteSpace(back))
            {
                flashcards.Add(new FlashCard(front, back));
            }
        }

        return flashcards;
    }

    public static async Task<List<FlashCard>> GenerateWithChatGPT(string text, int pageId,
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

    public static async Task<List<FlashCard>> GenerateWithClaude(string text, int pageId, PermissionCheck permissionCheck)
    {
        // RECOMMENDATION: reuse HttpClient across calls, but for brevity:
        var httpClient = new HttpClient();

        var endpointUrl = "https://api.anthropic.com/v1/messages";

        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Add("x-api-key", Settings.AnthropicApiKey);
        httpClient.DefaultRequestHeaders.Add("anthropic-version", Settings.AnthropicVersion);

        // Build request body
        var requestBody = new
        {
            model = "claude-3-5-haiku-20241022",  // e.g., "claude-3-5-sonnet-20241022"
            max_tokens = 1024,
            messages = new[]
            {
                new {
                    role = "user",
                    content = GetPrompt(text, pageId, permissionCheck)
                }
            }
        };

        var jsonData = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(endpointUrl, content);
        var responseBody = await response.Content.ReadAsStringAsync();

        // Print or log responseBody if you want to inspect it
        // Console.WriteLine(responseBody);

        try
        {
            var responseJson = JsonSerializer.Deserialize<AnthropicApiResponse>(responseBody);
            if (responseJson == null)
            {
                Console.WriteLine("Deserialization returned null.");
            }
            else
            {
                Console.WriteLine("Deserialization succeeded.");
                Console.WriteLine("Id: " + responseJson.Id);
                Console.WriteLine("Type: " + responseJson.Type);
                Console.WriteLine("Role: " + responseJson.Role);
                Console.WriteLine("Model: " + responseJson.Model);
                Console.WriteLine("Content Count: " + responseJson.Content?.Count);
                if (responseJson.Content != null && responseJson.Content.Count > 0)
                {
                    Console.WriteLine("First Content Text: " + responseJson.Content[0].Text);
                }
            }


            if (responseJson != null && responseJson.Content?.Count > 0 && !string.IsNullOrWhiteSpace(responseJson.Content[0].Text) && responseJson.Role == "assistant")
            {
                var flashCards = JsonSerializer.Deserialize<List<FlashCard>>(responseJson.Content[0].Text);
                return flashCards ?? new List<FlashCard>();
            }

            // If we get here, it means there's no assistant message or no content
            return new List<FlashCard>();
        }
        catch (JsonException)
        {
            // Couldn’t parse JSON from Claude, so return empty list
            return new List<FlashCard>();
        }
    }
}
