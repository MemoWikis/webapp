using System.Text.RegularExpressions;

class GenerateAiPage_tests : BaseTestHarness
{
    private const int DefaultUserId = 1;
    private const int DefaultPageId = 1;

    /// <summary>
    /// Helper to ask Claude to validate content properties.
    /// Returns true/false based on Claude's assessment.
    /// </summary>
    private static async Task<bool> AskClaude(string question)
    {
        var response = await ClaudeService.GetClaudeResponse(question + "\nAnswer only with 'true' or 'false', no explanation.");
        return response?.Content[0].Text.Trim().ToLower() == "true";
    }

    [Test]
    public async Task Generate_content_length_and_structure_validation()
    {
        // Arrange
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var webContentFetcher = R<WebContentFetcher>();
        var aiPageGenerator = new AiPageGenerator(aiUsageLogRepo, webContentFetcher);

        var prompt = "Explain the water cycle";

        // Act - Generate short and long content
        var shortResult = await aiPageGenerator.Generate(
            prompt,
            AiPageGenerator.DifficultyLevel.Intermediate,
            AiPageGenerator.ContentLength.Short,
            DefaultUserId,
            DefaultPageId);

        var longResult = await aiPageGenerator.Generate(
            prompt,
            AiPageGenerator.DifficultyLevel.Intermediate,
            AiPageGenerator.ContentLength.Long,
            DefaultUserId,
            DefaultPageId);

        // Use AI to validate content lengths
        var shortIsShort = await AskClaude($@"Is this a SHORT, concise summary (max 2-3 sections, brief overview only)?
            Content: {shortResult?.HtmlContent}");

        var longIsLong = await AskClaude($@"Is this a LONG, comprehensive, detailed content (many sections, in-depth coverage)?
            Content: {longResult?.HtmlContent}");

        // Validate HTML structure
        var htmlContent = longResult?.HtmlContent ?? "";
        var openH2Count = Regex.Matches(htmlContent, "<h2").Count;
        var closeH2Count = Regex.Matches(htmlContent, "</h2>").Count;

        await Verify(new
        {
            ShortContent = new
            {
                Generated = shortResult != null,
                HasTitle = !string.IsNullOrEmpty(shortResult?.Title),
                AiConfirmsIsShort = shortIsShort
            },
            LongContent = new
            {
                Generated = longResult != null,
                HasTitle = !string.IsNullOrEmpty(longResult?.Title),
                AiConfirmsIsLong = longIsLong
            },
            HtmlStructure = new
            {
                ContainsParagraphs = htmlContent.Contains("<p>"),
                DoesNotContainH1 = !htmlContent.Contains("<h1>"),
                H2TagsBalanced = openH2Count == closeH2Count,
                NoImages = !htmlContent.Contains("<img"),
                NoScriptTags = !htmlContent.Contains("<script")
            }
        });
    }

    [Test]
    public async Task Generate_respects_prompt_language()
    {
        // Arrange
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var webContentFetcher = R<WebContentFetcher>();
        var aiPageGenerator = new AiPageGenerator(aiUsageLogRepo, webContentFetcher);

        // Test with different language prompts
        var germanPrompt = "Erstelle eine Lernseite über die Grundlagen der deutschen Grammatik";
        var frenchPrompt = "Créer une page d'apprentissage sur l'histoire de la Révolution française";
        var englishPrompt = "Create a learning page about the history of the Roman Empire";

        // Act
        var germanResult = await aiPageGenerator.Generate(
            germanPrompt, AiPageGenerator.DifficultyLevel.Beginner,
            AiPageGenerator.ContentLength.Short, DefaultUserId, DefaultPageId);

        var frenchResult = await aiPageGenerator.Generate(
            frenchPrompt, AiPageGenerator.DifficultyLevel.Beginner,
            AiPageGenerator.ContentLength.Short, DefaultUserId, DefaultPageId);

        var englishResult = await aiPageGenerator.Generate(
            englishPrompt, AiPageGenerator.DifficultyLevel.Beginner,
            AiPageGenerator.ContentLength.Short, DefaultUserId, DefaultPageId);

        // Use AI to validate languages
        var isGerman = await AskClaude($"Is this text written in German? Title: {germanResult?.Title} Content: {germanResult?.HtmlContent}");
        var isFrench = await AskClaude($"Is this text written in French? Title: {frenchResult?.Title} Content: {frenchResult?.HtmlContent}");
        var isEnglish = await AskClaude($"Is this text written in English? Title: {englishResult?.Title} Content: {englishResult?.HtmlContent}");

        await Verify(new
        {
            German = new { Generated = germanResult != null, AiConfirmsLanguage = isGerman },
            French = new { Generated = frenchResult != null, AiConfirmsLanguage = isFrench },
            English = new { Generated = englishResult != null, AiConfirmsLanguage = isEnglish }
        });
    }

    [Test]
    public async Task Generate_adjusts_complexity_for_difficulty_level()
    {
        // Arrange
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var webContentFetcher = R<WebContentFetcher>();
        var aiPageGenerator = new AiPageGenerator(aiUsageLogRepo, webContentFetcher);

        var prompt = "Explain quantum physics";

        // Act - Generate at extreme difficulty levels
        var eli5Result = await aiPageGenerator.Generate(
            prompt, AiPageGenerator.DifficultyLevel.ELI5,
            AiPageGenerator.ContentLength.Short, DefaultUserId, DefaultPageId);

        var academicResult = await aiPageGenerator.Generate(
            prompt, AiPageGenerator.DifficultyLevel.Academic,
            AiPageGenerator.ContentLength.Short, DefaultUserId, DefaultPageId);

        // Use AI to validate complexity
        var eli5IsSimple = await AskClaude($@"Is this text written for a 5-year-old child (ELI5 style)?
            Look for: simple words, short sentences, easy examples, no jargon.
            Content: {eli5Result?.HtmlContent}");

        var academicIsAdvanced = await AskClaude($@"Is this text written at an academic/expert level?
            Look for: technical terminology, complex concepts, scholarly language.
            Content: {academicResult?.HtmlContent}");

        await Verify(new
        {
            ELI5 = new { Generated = eli5Result != null, AiConfirmsSimpleLanguage = eli5IsSimple },
            Academic = new { Generated = academicResult != null, AiConfirmsAdvancedLanguage = academicIsAdvanced }
        });
    }

    [Test]
    public async Task GenerateFromUrl_respects_source_language()
    {
        // Arrange
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var webContentFetcher = R<WebContentFetcher>();
        var aiPageGenerator = new AiPageGenerator(aiUsageLogRepo, webContentFetcher);

        // Use French and English Wikipedia to test language matching
        var frenchUrl = "https://fr.wikipedia.org/wiki/Photosynthèse";
        var englishUrl = "https://en.wikipedia.org/wiki/Photosynthesis";
        var invalidUrl = "https://this-domain-definitely-does-not-exist-12345.com/page";

        // Act
        var frenchResult = await aiPageGenerator.GenerateFromUrl(
            frenchUrl, AiPageGenerator.DifficultyLevel.Intermediate,
            AiPageGenerator.ContentLength.Medium, DefaultUserId, DefaultPageId);

        var englishResult = await aiPageGenerator.GenerateFromUrl(
            englishUrl, AiPageGenerator.DifficultyLevel.Intermediate,
            AiPageGenerator.ContentLength.Medium, DefaultUserId, DefaultPageId);

        var invalidResult = await aiPageGenerator.GenerateFromUrl(
            invalidUrl, AiPageGenerator.DifficultyLevel.Intermediate,
            AiPageGenerator.ContentLength.Medium, DefaultUserId, DefaultPageId);

        // Use AI to validate languages and content relevance
        var frenchIsFrench = await AskClaude($"Is this text written in French? Title: {frenchResult?.Title} Content: {frenchResult?.HtmlContent}");
        var englishIsEnglish = await AskClaude($"Is this text written in English? Title: {englishResult?.Title} Content: {englishResult?.HtmlContent}");

        var frenchIsAboutPhotosynthesis = await AskClaude($"Is this content about photosynthesis (plants, light, chlorophyll, energy)? Content: {frenchResult?.HtmlContent}");
        var englishIsAboutPhotosynthesis = await AskClaude($"Is this content about photosynthesis (plants, light, chlorophyll, energy)? Content: {englishResult?.HtmlContent}");

        await Verify(new
        {
            FrenchWikipedia = new
            {
                Generated = frenchResult != null,
                AiConfirmsIsFrench = frenchIsFrench,
                AiConfirmsTopicRelevant = frenchIsAboutPhotosynthesis
            },
            EnglishWikipedia = new
            {
                Generated = englishResult != null,
                AiConfirmsIsEnglish = englishIsEnglish,
                AiConfirmsTopicRelevant = englishIsAboutPhotosynthesis
            },
            InvalidUrl = new
            {
                ReturnsNull = invalidResult == null
            }
        });
    }
}
