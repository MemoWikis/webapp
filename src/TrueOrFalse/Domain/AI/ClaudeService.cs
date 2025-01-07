using System.Text;
using System.Text.Json;
using static AiFlashCard;

public static class ClaudeService
{
    // Falls du mehr Kontrolle über den Lebenszyklus von HttpClient brauchst, 
    // empfielt es sich, ihn entweder als Singleton oder als statische Instanz 
    // in deiner Klasse zu verwalten.
    private static readonly HttpClient httpClient = new HttpClient
    {
        BaseAddress = new Uri("https://api.anthropic.com")
    };

    static ClaudeService()
    {
        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Add("x-api-key", Settings.AnthropicApiKey);
        httpClient.DefaultRequestHeaders.Add("anthropic-version", Settings.AnthropicVersion);
    }

    public static async Task<List<FlashCard>> GenerateFlashcardsAsync(
        string text,
        int pageId,
        PermissionCheck permissionCheck
    )
    {
        // Stelle sicher, dass du immer einen gültigen Prompt hast
        string promptContent = GetPrompt(text, pageId, permissionCheck);
        if (string.IsNullOrWhiteSpace(promptContent))
            return new List<FlashCard>();

        // Request-Einstellungen
        var request = new
        {
            model = "claude-3-5-haiku-20241022",
            max_tokens = 8192,
            messages = new[]
            {
                new
                {
                    role = "user",
                    content = promptContent
                }
            }
        };

        var jsonData = JsonSerializer.Serialize(request);
        using var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        try
        {
            // Endpunkt auf "/v1/messages"
            var response = await httpClient.PostAsync("/v1/messages", content);

            // Gibt es einen Fehler im HTTP-Status, werfen wir eine Exception
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            // Versuche JSON zu deserialisieren
            var responseJson = JsonSerializer.Deserialize<AnthropicApiResponse>(responseBody);

            // Gültige Prüfung: Rolle muss "assistant" sein, Content darf nicht leer sein
            if (responseJson?.Role == "assistant"
                && responseJson.Content?.Count > 0
                && !string.IsNullOrWhiteSpace(responseJson.Content[0].Text))
            {
                // Nun versuchen wir, die JSON-Daten in eine Liste von FlashCards zu parsen
                var flashCards = JsonSerializer.Deserialize<List<FlashCard>>(responseJson.Content[0].Text);
                return flashCards ?? new List<FlashCard>();
            }

            // Hierher gelangt man nur, wenn die Antwort von Claude nicht dem erwarteten Format entspricht
            return new List<FlashCard>();
        }
        catch (HttpRequestException ex)
        {
            // Network-/HTTP-Fehler
            // Logge ggf. ex.Message oder ex.StackTrace
            return new List<FlashCard>();
        }
        catch (JsonException)
        {
            // JSON konnte nicht verarbeitet werden
            return new List<FlashCard>();
        }
    }
}
