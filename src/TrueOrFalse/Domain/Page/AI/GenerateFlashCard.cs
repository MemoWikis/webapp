using OpenAI.Chat;
using System.Text.Json;
using System.Text.RegularExpressions;
using TrueOrFalse;

public class AiFlashCard(PermissionCheck _permissionCheck) : IRegisterAsInstancePerLifetime
{
    public record struct FlashCard(string Front, string Back);
    public record struct BackJson(string Text);

    public static string GetPrompt(string flashcards, string sourceText)
    {
        return @"
            Antworte mit Karteikarten als JSON-Array, das zwei Eigenschaften enthält: 'Front' und 'Back'. 
            Beispiel für JSON-Array: 
            [
                { 'Front': 'Was ist die Hauptstadt von Deutschland?', 'Back': 'Berlin' },
                { 'Front': 'Was ist die Hauptstadt von Frankreich?', 'Back': 'Paris' }
            ]
            Formatierung: Wichtige Wortre können krusiv, fett unterstrichen markiert werden. Listen sind auch möglich. Verwende HTML Tags.  

            'Front':  
                - ist die Vorderseite der Karteikarte,
                - soll nur eine einzige Frage, ein Wort, einen Begriff, einen Satz oder eine Phrase enthalten,

            'Back'
               - ist dir Rücksteite der Karteikarte,
            
            Folgend Karteikarten exisiteren bereits: " +
            flashcards +
            @"
            Prüfe auf Duplikate. Die erstellten Karteikarten sollten keine Duplikate enthalten.
            Mache keine Erklärung, wrappe die Antwort nicht in einem Code-Block.
            Erstelle Karteikarten basierend auf diesen Text:" +
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

    public static async Task<List<FlashCard>> Generate(string text, int pageId, PermissionCheck permissionCheck)
    {
        var existingFlashCards = GetFlashCardsOnPage(pageId, permissionCheck);
        var flashCardsString = JsonSerializer.Serialize(existingFlashCards);

        ChatClient client = new(model: "gpt-4o-mini", apiKey: Settings.OpenAIApiKey);
        ChatCompletion chatCompletion = await client.CompleteChatAsync(GetPrompt(flashCardsString, text));

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
