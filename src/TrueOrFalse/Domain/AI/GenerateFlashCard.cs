using System.Text.Json;
using System.Text.RegularExpressions;
using TrueOrFalse;
public class AiFlashCard : IRegisterAsInstancePerLifetime
{
    public record struct FlashCard(string Front, string Back);

    public record struct BackJson(string Text);

    public static string GetPromptO1(string sourceText, string flashcards)
    {
        return @"
            Antworte mit Karteikarten als JSON-Array, das zwei Eigenschaften enthält: 'Front' und 'Back'. 
            Beispiel für JSON-Array:
            [
                { 'Front': 'Was ist die Hauptstadt von Deutschland?', 'Back': 'Berlin' },
                { 'Front': 'Was ist die Hauptstadt von Frankreich?', 'Back': 'Paris' }
            ]
            Formatierung: Wichtige Worte können kursiv mit <em>, fett mit <strong> oder mit einem Unterstrich <u> per HTML-Tags formatiert werden. 
            Listen sind auch möglich. Formatiere mit HTML-Tags.
            
            Regeln:
            1. 'Front' ist die Vorderseite der Karteikarte und soll nur eine einzige Frage, ein Wort, einen Begriff, einen Satz oder eine Phrase enthalten.
            2. 'Back' ist die Rückseite der Karteikarte.
            
            Folgende Karteikarten existieren bereits:
            " + flashcards + @"
            
            Wichtig:
            - Prüfe vor dem Erstellen neuer Karteikarten, ob bereits eine Karteikarte mit derselben 'Front' ODER demselben 'Back' existiert. 
              Wenn ja, erstelle KEINE Duplikate oder leicht umformulierte Varianten. 
            - Erstelle nur Karteikarten, die inhaltlich zum folgenden Text passen und sich nicht inhaltlich wiederholen:
            " + sourceText + @"
            
            Gib nur die neuen Karteikarten als JSON-Array aus, ohne jede weitere Erklärung oder Codeblöcke.";
    }

    public static string GetPromptOpus(string sourceText, string flashcards)
    {
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
               - ist die Rückseite der Karteikarte,
            
            Folgende Karteikarten existieren bereits: " +
                   flashcards +
                   @"
            Prüfe sorgfältig auf Duplikate, indem du den Inhalt der Vorder- und Rückseite vergleichst. Die erstellten Karteikarten dürfen keine Duplikate enthalten, weder untereinander noch zu den bereits existierenden Karten.

            Gehe wie folgt vor:
            1. Extrahiere die relevanten Informationen aus dem Quelltext.
            2. Formuliere daraus potenzielle Karteikarten. 
            3. Vergleiche jede potenzielle Karteikarte mit den bereits existierenden Karten und den zuvor erstellten potenziellen Karten.
            4. Wenn es ein Duplikat gibt, verwirf die Karte. Wenn es kein Duplikat ist, füge die Karte zum Ergebnis-Array hinzu.
            5. Wiederhole die Schritte 3-4 für jede potenzielle Karteikarte.

            Mache keine Erklärung, wrappe die Antwort nicht in einem Code-Block. 
            Erstelle Karteikarten basierend auf diesem Text (nur fachlich passend) und erstelle keine inhaltlichen Duplikate zu den existierenden Karteikarten:" +
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


    public static async Task<List<FlashCard>> Generate(
        string text,
        int pageId,
        PermissionCheck permissionCheck,
        AiModel model = AiModel.Claude)
    {
        var existingFlashCards = GetFlashCardsOnPage(pageId, permissionCheck);
        var flashcards = JsonSerializer.Serialize(existingFlashCards);

        string promptContent = GetPromptOpus(text, flashcards);
        if (string.IsNullOrWhiteSpace(promptContent))
            return new List<FlashCard>();

        return await Generate(promptContent, model);
    }

    public static async Task<List<FlashCard>> Generate(string promptContent, AiModel model)
    {
        return model switch
        {
            AiModel.ChatGPT => await ChatGPTService.GenerateFlashcardsAsync(promptContent),
            AiModel.Claude => await ClaudeService.GenerateFlashcardsAsync(promptContent),
            _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
        };
    }
}
