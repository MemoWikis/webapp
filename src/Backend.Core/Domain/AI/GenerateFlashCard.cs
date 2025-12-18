using System.Text.Json;
using System.Text.RegularExpressions;

public class AiFlashCard(
    AiUsageLogRepo _aiUsageLogRepo) : IRegisterAsInstancePerLifetime
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
            Gib nur die neuen Karteikarten als JSON-Array aus, ohne jede weitere Erklärung oder Codeblöcke. Überprüfe nochmal ob es inhaltiche Duplikate gibt, falls ja entferne diese, und gebe mir nur die neuen Karteikarten.";
    }

    public static string GetPromptOpus(string sourceText, string flashcards)
    {
        return @"
            Antworte ausschließlich mit einem JSON-Array von Karteikarten.
            Jede Karteikarte hat zwei Eigenschaften: 'Front' und 'Back'.
            
            Beispiel für ein JSON-Array:
            [
              { 'Front': 'Was ist die Hauptstadt von Deutschland?', 'Back': 'Berlin' },
              { 'Front': 'Was ist die Hauptstadt von Frankreich?', 'Back': 'Paris' }
            ]
            
            Formatierungshinweise:
            - Verwende nur HTML-Tags für Hervorhebungen (<em>, <strong>, <u>) oder Listen.
            - Mache keine zusätzlichen Erläuterungen.
            - Gib keinerlei Ausgabe in Code-Blöcken zurück.
            
            'Front':
            - Enthält nur eine einzige Frage, einen Begriff, einen Satz oder eine Phrase.
            'Back':
            - Enthält die passende Antwort oder Erklärung.
            
            Berücksichtige vorhandene Karteikarten:
            " + flashcards + @"
            
            Achte unbedingt darauf:
            - Die Sprache der erstellten Karteikarten muss exakt mit der des gegebenen Textes übereinstimmen.
            - Sollte der gegebene Text gemischte Sprachen enthalten, verwende die dominante Sprache.
            
            Deine Aufgabe:
            1. Lies den folgenden gegebenen Text aufmerksam:
               """ + sourceText + @"""
            2. Extrahiere daraus relevante Konzepte und formuliere neue potenzielle Karteikarten in derselben Sprache wie der gegebene Text. 
               - Es ist sehr, sehr wichtig, dass es keine Duplikate gibt. 
               - Duplikate zu vermeiden hat oberste Priorität.
            3. Prüfe jede potenzielle Karte gründlich gegen die vorhandenen Karten und untereinander:
               - Vergleiche sowohl 'Front' als auch 'Back' (ignoriere Groß- und Kleinschreibung, Satzzeichen und kleinere Abweichungen).
               - Stelle sicher, dass sich keine Duplikate einschleichen.
            4. Verwirf jede potenzielle Karte, die inhaltlich bereits existiert (Duplikate von vorhandenen oder bereits erstellten Karten).
            5. Gib ausschließlich die neuen, eindeutigen Karteikarten als JSON-Array zurück.
            6. Falls der gegebene Text zu kurz und unklar ist oder keinen Sinn macht, gib ein leeres JSON-Array zurück.
            7. Prüfe das Ergebnis auf Duplikate und entferne diese.
            
            Denke daran:
            - Keine Erklärungen, keine Einleitungen oder Zusammenfassungen.
            - Antworte nur mit dem JSON-Array (ohne Code-Blöcke).
            ";
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


    public async Task<List<FlashCard>> Generate(
        string text,
        int pageId,
        int userId,
        PermissionCheck permissionCheck,
        AiModel model = AiModel.Claude)
    {
        var existingFlashCards = GetFlashCardsOnPage(pageId, permissionCheck);
        var flashcards = JsonSerializer.Serialize(existingFlashCards);

        string promptContent = GetPromptOpus(text, flashcards);
        if (string.IsNullOrWhiteSpace(promptContent))
            return new List<FlashCard>();

        return await Generate(promptContent, model, userId, pageId);
    }

    public async Task<List<FlashCard>> Generate(string promptContent, AiModel model, int userId, int pageId)
    {
        var result = model switch
        {
            AiModel.ChatGPT => await ChatGPTService.GenerateFlashcardsAsync(promptContent),
            AiModel.Claude => await GenerateFlashcardsWithTokenDeduction(promptContent, userId, pageId),
            _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
        };

        return result;
    }

    private async Task<List<FlashCard>> GenerateFlashcardsWithTokenDeduction(string promptContent, int userId, int pageId)
    {
        var response = await ClaudeService.GetClaudeResponse(promptContent);

        if (response != null)
        {
            _aiUsageLogRepo.AddUsage(response, userId, pageId);
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
