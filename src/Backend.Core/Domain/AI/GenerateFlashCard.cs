using System.Text.Json;
using System.Text.RegularExpressions;

public class AiFlashCard(AiUsageLogRepo _aiUsageLogRepo) : IRegisterAsInstancePerLifetime
{
    public record struct FlashCard(string Front, string Back);

    public record struct BackJson(string Text);


    public static string GetPrompt(string sourceText, string flashcards)
    {
        return @"
            Respond exclusively with a JSON array of flashcards.
            Each flashcard has two properties: 'Front' and 'Back'.
            
            Example of a JSON array:
            [
              { 'Front': 'What is the capital of Germany?', 'Back': 'Berlin' },
              { 'Front': 'What is the capital of France?', 'Back': 'Paris' }
            ]
            
            Formatting instructions:
            - Use only HTML tags for emphasis (<em>, <strong>, <u>) or lists.
            - Do not provide additional explanations.
            - Do not return any output in code blocks.
            
            'Front':
            - Contains only a single question, term, sentence, or phrase.
            'Back':
            - Contains the appropriate answer or explanation.
            
            Consider existing flashcards:
            " + flashcards + @"
            
            Important notes:
            - The language of the created flashcards must exactly match that of the given text.
            - If the given text contains mixed languages, use the dominant language.
            
            Your task:
            1. Read the following given text carefully:
               """ + sourceText + @"""
            2. Extract relevant concepts from it and formulate new potential flashcards in the same language as the given text.
               - It is very, very important that there are no duplicates.
               - Avoiding duplicates has the highest priority.
            3. Check each potential card thoroughly against existing cards and against each other:
               - Compare both 'Front' and 'Back' (ignore case, punctuation, and minor variations).
               - Ensure that no duplicates slip through.
            4. Discard any potential card that already exists in content (duplicates of existing or already created cards).
            5. Return exclusively the new, unique flashcards as a JSON array.
            6. If the given text is too short and unclear or does not make sense, return an empty JSON array.
            7. Check the result for duplicates and remove them.
            
            Remember:
            - No explanations, no introductions or summaries.
            - Respond only with the JSON array (without code blocks).
            
            CRITICAL - JSON formatting:
            - Your response MUST start with '[' and end with ']'.
            - No characters before '[' or after ']'.
            - No backticks, no Markdown code blocks.
            - No parentheses ')' or other characters after the JSON array.
            - Use exclusively double quotes for JSON properties.
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

        string promptContent = GetPrompt(text, flashcards);
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

    private const int MaxRetries = 2;

    private async Task<List<FlashCard>> GenerateFlashcardsWithTokenDeduction(string promptContent, int userId, int pageId)
    {
        for (var attempt = 1; attempt <= MaxRetries; attempt++)
        {
            var response = await ClaudeService.GetClaudeResponse(promptContent);

            if (response != null)
            {
                _aiUsageLogRepo.AddUsage(response, userId, pageId);
            }

            var flashCards = TryParseFlashCardsFromResponse(response, attempt, pageId);
            
            if (flashCards != null && flashCards.Count > 0)
            {
                if (attempt > 1)
                {
                    Log.Information(
                        "FlashCard generation succeeded on attempt {Attempt} for pageId {PageId}",
                        attempt, pageId);
                }
                return flashCards;
            }

            if (attempt < MaxRetries)
            {
                Log.Information(
                    "Retrying FlashCard generation for pageId {PageId} (attempt {NextAttempt}/{MaxRetries})",
                    pageId, attempt + 1, MaxRetries);
            }
        }

        Log.Error(
            "FlashCard generation failed after {MaxRetries} attempts for pageId {PageId}",
            MaxRetries, pageId);

        return new List<FlashCard>();
    }

    private List<FlashCard> TryParseFlashCardsFromResponse(AnthropicApiResponse response, int attempt, int pageId)
    {
        if (response is not { Role: "assistant", Content.Count: > 0 })
        {
            Log.Warning(
                "FlashCard generation returned empty or invalid response (attempt {Attempt}/{MaxRetries}) for pageId {PageId}",
                attempt, MaxRetries, pageId);
            return null;
        }

        if (string.IsNullOrWhiteSpace(response.Content[0].Text))
        {
            Log.Warning(
                "FlashCard generation returned empty text (attempt {Attempt}/{MaxRetries}) for pageId {PageId}",
                attempt, MaxRetries, pageId);
            return null;
        }

        var rawText = response.Content[0].Text;
        var normalizedJson = NormalizeFlashCardJson(rawText);

        try
        {
            var flashCards = JsonSerializer.Deserialize<List<FlashCard>>(normalizedJson);
            
            if (flashCards == null || flashCards.Count == 0)
            {
                Log.Warning(
                    "FlashCard generation returned empty array (attempt {Attempt}/{MaxRetries}) for pageId {PageId}",
                    attempt, MaxRetries, pageId);
                return null;
            }

            return flashCards;
        }
        catch (JsonException ex)
        {
            Log.Warning(
                "FlashCard JSON parse failed (attempt {Attempt}/{MaxRetries}) for pageId {PageId}. Error: {Error}. RawResponse: {RawResponse}",
                attempt, MaxRetries, pageId, ex.Message, Truncate(rawText, 500));
            return null;
        }
    }

    private static string NormalizeFlashCardJson(string rawResponseText)
    {
        if (string.IsNullOrWhiteSpace(rawResponseText))
        {
            return string.Empty;
        }

        var trimmed = rawResponseText.Trim();

        trimmed = Regex.Replace(trimmed, "^```(json)?\\s*|```\\s*$", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Multiline);

        var firstBracket = trimmed.IndexOf('[');
        var lastBracket = trimmed.LastIndexOf(']');

        if (firstBracket >= 0 && lastBracket > firstBracket)
        {
            trimmed = trimmed.Substring(firstBracket, lastBracket - firstBracket + 1);
        }

        trimmed = trimmed.Replace("“", "\"").Replace("”", "\"");
        trimmed = Regex.Replace(trimmed, "'Front'", "\"Front\"", RegexOptions.IgnoreCase);
        trimmed = Regex.Replace(trimmed, "'Back'", "\"Back\"", RegexOptions.IgnoreCase);

        return trimmed;
    }

    private static string Truncate(string text, int maxLength)
    {
        if (string.IsNullOrEmpty(text) || text.Length <= maxLength)
        {
            return text;
        }

        return text[..maxLength];
    }
}
