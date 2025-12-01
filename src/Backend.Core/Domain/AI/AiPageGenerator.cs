using System.Text.Json;

public class AiPageGenerator(AiUsageLogRepo _aiUsageLogRepo, WebContentFetcher _webContentFetcher) : IRegisterAsInstancePerLifetime
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

    public enum ContentLength
    {
        Short = 1,
        Medium = 2,
        Long = 3
    }

    private static string GetDifficultyDescription(DifficultyLevel difficultyLevel)
    {
        return difficultyLevel switch
        {
            DifficultyLevel.ELI5 => "wie für ein 5-jähriges Kind (ELI5 - Explain Like I'm 5). Verwende einfache Worte, kurze Sätze und anschauliche Beispiele.",
            DifficultyLevel.Beginner => "für Anfänger ohne Vorkenntnisse. Erkläre Grundbegriffe und verwende einfache Sprache.",
            DifficultyLevel.Intermediate => "für Lernende mit Grundkenntnissen. Du kannst Fachbegriffe verwenden, solltest sie aber kurz erklären.",
            DifficultyLevel.Advanced => "für Fortgeschrittene. Fachbegriffe können ohne ausführliche Erklärung verwendet werden.",
            DifficultyLevel.Academic => "auf akademischem Niveau. Verwende Fachsprache und gehe in die Tiefe.",
            _ => "für Lernende mit Grundkenntnissen."
        };
    }

    private static string GetContentLengthDescription(ContentLength contentLength)
    {
        return contentLength switch
        {
            ContentLength.Short => "Erstelle eine KURZE Zusammenfassung. Fokussiere dich nur auf die wichtigsten Kernpunkte. Maximal 2-3 Abschnitte.",
            ContentLength.Medium => "Erstelle einen Inhalt mittlerer Länge. Decke die wichtigsten Themen ab mit angemessener Tiefe. Etwa 4-6 Abschnitte.",
            ContentLength.Long => "Erstelle einen ausführlichen, umfassenden Inhalt. Gehe ins Detail und decke alle relevanten Aspekte ab. Kürze nichts, sei so ausführlich wie möglich.",
            _ => "Erstelle einen Inhalt mittlerer Länge."
        };
    }

    public static string GetPrompt(string userPrompt, DifficultyLevel difficultyLevel, ContentLength contentLength)
    {
        var difficultyDescription = GetDifficultyDescription(difficultyLevel);
        var contentLengthDescription = GetContentLengthDescription(contentLength);

        return @"
            Du bist ein Experte für die Erstellung von Lernseiten.
            Erstelle eine Lernseite basierend auf der folgenden Beschreibung des Nutzers.
            
            Wichtig zur Sprache:
            - Die Sprache der erstellten Seite muss exakt mit der Sprache der Nutzerbeschreibung übereinstimmen.
            - Wenn die Beschreibung auf Deutsch ist, antworte auf Deutsch.
            - Wenn die Beschreibung auf Englisch ist, antworte auf Englisch.
            
            Schwierigkeitsgrad: " + difficultyDescription + @"
            
            Inhaltslänge: " + contentLengthDescription + @"
            
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

    public static string GetUrlPrompt(string pageTitle, string pageContent, string sourceUrl, DifficultyLevel difficultyLevel, ContentLength contentLength)
    {
        var difficultyDescription = GetDifficultyDescription(difficultyLevel);
        var contentLengthDescription = GetContentLengthDescription(contentLength);

        return @"
            Du bist ein Experte für die Erstellung von Lernseiten.
            Erstelle eine Lernseite basierend auf dem folgenden Webseiten-Inhalt.
            Die Quelle ist: " + sourceUrl + @"
            
            Wichtig zur Sprache:
            - Die Sprache der erstellten Seite muss exakt mit der Sprache des Quellinhalts übereinstimmen.
            - Wenn der Inhalt auf Deutsch ist, antworte auf Deutsch.
            - Wenn der Inhalt auf Englisch ist, antworte auf Englisch.
            
            Schwierigkeitsgrad: " + difficultyDescription + @"
            
            Inhaltslänge: " + contentLengthDescription + @"
            
            Antworte ausschließlich mit einem JSON-Objekt mit zwei Eigenschaften:
            - 'Title': Ein kurzer, prägnanter Titel für die Seite (max. 60 Zeichen). Basierend auf dem Originaltitel, aber kürzer und prägnanter.
            - 'HtmlContent': Der HTML-Inhalt der Seite, als gut strukturierte Lernseite aufbereitet.
            
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
            - Strukturiere den Inhalt logisch für Lernzwecke
            - Entferne irrelevante Teile wie Navigation, Werbung, etc.
            
            Originaltitel: " + pageTitle + @"
            
            Quellinhalt:
            " + pageContent + @"
            
            Antworte nur mit dem JSON-Objekt, ohne Code-Blöcke oder zusätzliche Erklärungen.";
    }

    public async Task<GeneratedPage?> Generate(
        string userPrompt,
        DifficultyLevel difficultyLevel,
        ContentLength contentLength,
        int userId,
        int pageId)
    {
        var prompt = GetPrompt(userPrompt, difficultyLevel, contentLength);
        return await ExecuteGeneration(prompt, userId, pageId);
    }

    public async Task<GeneratedPage?> GenerateFromUrl(
        string url,
        DifficultyLevel difficultyLevel,
        ContentLength contentLength,
        int userId,
        int pageId)
    {
        var fetchedContent = await _webContentFetcher.FetchAndExtract(url);
        
        if (fetchedContent == null)
        {
            Log.Warning("Failed to fetch content from URL: {Url}", url);
            return null;
        }

        var prompt = GetUrlPrompt(
            fetchedContent.Value.Title,
            fetchedContent.Value.TextContent,
            fetchedContent.Value.Url,
            difficultyLevel,
            contentLength);
        
        return await ExecuteGeneration(prompt, userId, pageId);
    }

    private async Task<GeneratedPage?> ExecuteGeneration(string prompt, int userId, int pageId)
    {
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
