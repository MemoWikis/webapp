using System.Text.Json;

public class AiPageGenerator(AiUsageLogRepo _aiUsageLogRepo) : IRegisterAsInstancePerLifetime
{
    public record struct GeneratedPage(string Title, string HtmlContent);

    public enum DifficultyLevel
    {
        ELI5 = 1,
        Beginner = 2,
        Intermediate = 3,
        Advanced = 4,
        Academic = 5
    }

    public static string GetPrompt(string userPrompt, DifficultyLevel difficultyLevel)
    {
        var difficultyDescription = difficultyLevel switch
        {
            DifficultyLevel.ELI5 => "wie für ein 5-jähriges Kind (ELI5 - Explain Like I'm 5). Verwende einfache Worte, kurze Sätze und anschauliche Beispiele.",
            DifficultyLevel.Beginner => "für Anfänger ohne Vorkenntnisse. Erkläre Grundbegriffe und verwende einfache Sprache.",
            DifficultyLevel.Intermediate => "für Lernende mit Grundkenntnissen. Du kannst Fachbegriffe verwenden, solltest sie aber kurz erklären.",
            DifficultyLevel.Advanced => "für Fortgeschrittene. Fachbegriffe können ohne ausführliche Erklärung verwendet werden.",
            DifficultyLevel.Academic => "auf akademischem Niveau. Verwende Fachsprache und gehe in die Tiefe.",
            _ => "für Lernende mit Grundkenntnissen."
        };

        return @"
            Du bist ein Experte für die Erstellung von Lernseiten.
            Erstelle eine Lernseite basierend auf der folgenden Beschreibung des Nutzers.
            
            Wichtig zur Sprache:
            - Die Sprache der erstellten Seite muss exakt mit der Sprache der Nutzerbeschreibung übereinstimmen.
            - Wenn die Beschreibung auf Deutsch ist, antworte auf Deutsch.
            - Wenn die Beschreibung auf Englisch ist, antworte auf Englisch.
            
            Schwierigkeitsgrad: " + difficultyDescription + @"
            
            Antworte ausschließlich mit einem JSON-Objekt mit zwei Eigenschaften:
            - 'Title': Ein kurzer, prägnanter Titel für die Seite (max. 60 Zeichen)
            - 'HtmlContent': Der HTML-Inhalt der Seite
            
            Beispiel:
            {
                ""Title"": ""Einführung in die Photosynthese"",
                ""HtmlContent"": ""<h2>Was ist Photosynthese?</h2><p>Photosynthese ist...</p>""
            }
            
            Formatierungsregeln für HtmlContent:
            - Verwende <h2> für Hauptüberschriften und <h3> für Unterüberschriften
            - Verwende <p> für Absätze
            - Verwende <ul> und <li> für Listen
            - Verwende <strong> für wichtige Begriffe
            - Verwende <em> für Hervorhebungen
            - KEINE Bilder, Links oder externe Ressourcen
            - KEIN <h1> Tag (wird vom System hinzugefügt)
            - Halte den Inhalt übersichtlich und gut strukturiert
            
            Nutzerbeschreibung:
            " + userPrompt + @"
            
            Antworte nur mit dem JSON-Objekt, ohne Code-Blöcke oder zusätzliche Erklärungen.";
    }

    public async Task<GeneratedPage?> Generate(
        string userPrompt,
        DifficultyLevel difficultyLevel,
        int userId,
        int pageId)
    {
        var prompt = GetPrompt(userPrompt, difficultyLevel);
        var response = await ClaudeService.GetClaudeResponse(prompt);

        if (response != null)
        {
            _aiUsageLogRepo.AddUsage(response, userId, pageId);
        }

        if (response is { Role: "assistant", Content.Count: > 0 }
            && !string.IsNullOrWhiteSpace(response.Content[0].Text))
        {
            try
            {
                var text = response.Content[0].Text.Trim();
                
                // Remove potential code block markers
                if (text.StartsWith("```json"))
                {
                    text = text.Substring(7);
                }
                if (text.StartsWith("```"))
                {
                    text = text.Substring(3);
                }
                if (text.EndsWith("```"))
                {
                    text = text.Substring(0, text.Length - 3);
                }
                text = text.Trim();

                var generatedPage = JsonSerializer.Deserialize<GeneratedPage>(text, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                return generatedPage;
            }
            catch (JsonException exception)
            {
                Log.Error(exception, "Failed to parse AI response for page generation");
                return null;
            }
        }

        return null;
    }
}
