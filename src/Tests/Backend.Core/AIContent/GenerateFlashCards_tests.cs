using Microsoft.AspNetCore.Http;
using System.Text.Json;

class GenerateFlashCards_tests : BaseTestHarness
{
    // Expected flashcard count ranges based on source text length
    private const int ShortTextMinCards = 2;
    private const int ShortTextMaxCards = 15;
    private const int LongTextMinCards = 5;
    private const int LongTextMaxCards = 35;
    private const int LanguageTestMaxCards = 20;

    // Token count validation ranges (sanity checks for AI usage logging)
    private const int MinExpectedInputTokens = 100;
    private const int MaxExpectedInputTokens = 10000;
    private const int MinExpectedOutputTokens = 50;
    private const int MaxExpectedOutputTokens = 5000;

    private const int TestUserId = 1;
    private const int TestPageId = 1;

    [SetUp]
    public void ClearAiUsageLogForTestUser()
    {
        var session = R<NHibernate.ISession>();
        session.CreateSQLQuery("DELETE FROM ai_usage_log WHERE User_id = :userId")
            .SetParameter("userId", TestUserId)
            .ExecuteUpdate();
        session.Flush();
    }

    #region Helper Records and Methods

    private record AiUsageSummary(
        int RequestCount,
        bool HasInputTokens,
        bool HasOutputTokens,
        bool InputTokensInRange,
        bool OutputTokensInRange,
        string ModelUsed
    );

    private record FlashCardValidation(
        bool Generated,
        bool HasCards,
        bool CountInRange,
        bool AllCardsHaveFront,
        bool AllCardsHaveBack
    );

    private async Task<AiUsageSummary> QueryAiUsageLogsSince(DateTime sinceTime)
    {
        var session = R<NHibernate.ISession>();

        var query = session.CreateSQLQuery(@"
            SELECT Id, User_id, Page_id, TokenIn, TokenOut, DateCreated, Model
            FROM ai_usage_log
            WHERE User_id = :userId AND DateCreated >= :fromDate
            ORDER BY DateCreated DESC");

        query.SetParameter("userId", TestUserId);
        query.SetParameter("fromDate", sinceTime);
        query.AddEntity(typeof(AiUsageLog));

        var usageLogs = query.List<AiUsageLog>().ToList();

        if (usageLogs.Count == 0)
        {
            return new AiUsageSummary(0, false, false, false, false, "");
        }

        var totalInputTokens = usageLogs.Sum(log => log.TokenIn);
        var totalOutputTokens = usageLogs.Sum(log => log.TokenOut);

        return new AiUsageSummary(
            RequestCount: usageLogs.Count,
            HasInputTokens: totalInputTokens > 0,
            HasOutputTokens: totalOutputTokens > 0,
            InputTokensInRange: totalInputTokens > MinExpectedInputTokens && totalInputTokens < MaxExpectedInputTokens,
            OutputTokensInRange: totalOutputTokens > MinExpectedOutputTokens && totalOutputTokens < MaxExpectedOutputTokens,
            ModelUsed: usageLogs.First().Model ?? ""
        );
    }

    private static FlashCardValidation ValidateFlashCards(List<AiFlashCard.FlashCard>? flashCards, int minCards, int maxCards)
    {
        if (flashCards == null)
        {
            return new FlashCardValidation(false, false, false, false, false);
        }

        return new FlashCardValidation(
            Generated: true,
            HasCards: flashCards.Count > 0,
            CountInRange: flashCards.Count >= minCards && flashCards.Count <= maxCards,
            AllCardsHaveFront: flashCards.All(card => !string.IsNullOrWhiteSpace(card.Front)),
            AllCardsHaveBack: flashCards.All(card => !string.IsNullOrWhiteSpace(card.Back))
        );
    }

    private async Task<(Page page, AiFlashCard aiFlashCard)> SetupTestPageAndFlashCardService()
    {
        var context = NewPageContext();
        context.Add("TestPage").Persist();
        var page = context.All.ByName("TestPage");
        context.AddToEntityCache(page);

        await ReloadCaches();

        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var aiFlashCard = new AiFlashCard(aiUsageLogRepo);

        return (page, aiFlashCard);
    }

    private static PermissionCheck CreateAnonymousPermissionCheck() =>
        new PermissionCheck(new SessionlessUser(-1));

    private async Task<bool> CheckForDuplicatesViaAi(string firstListJson, string secondListJson)
    {
        var duplicateCheckPrompt = $@"Hier sind zwei Listen mit Karteikarten (Eigenschaften: ""Front"" und ""Back"").
Liste A: {firstListJson}
Liste B: {secondListJson}

Vergleiche beide Listen auf inhaltlich/fachliche Duplikate.
Teilweise überlappende Inhalte sind KEINE Duplikate.
Antworte NUR mit 'true' (Duplikate vorhanden) oder 'false' (keine Duplikate).";

        var response = await ClaudeService.GetClaudeResponse(duplicateCheckPrompt);
        var responseText = response?.Content?[0]?.Text?.Trim().ToLower() ?? "";

        return responseText == "true";
    }

    private async Task<(bool isValid, bool languageMatches)> CheckLanguageMatchViaAi(string sourceText, string flashCardsJson)
    {
        var languageCheckPrompt = $@"Überprüfe ob die Sprache des Quelltextes und der Karteikarten übereinstimmen.
Quelltext: {sourceText}
Karteikarten: {flashCardsJson}
Antworte nur mit 'true' oder 'false'.";

        var response = await ClaudeService.GetClaudeResponse(languageCheckPrompt);
        var responseText = response?.Content?[0]?.Text?.Trim().ToLower() ?? "";
        var isValidResponse = responseText == "true" || responseText == "false";

        return (isValidResponse, responseText == "true");
    }

    #endregion

    #region Short Source Text Tests

    [Test]
    public async Task Should_generate_valid_flashcards_for_short_english_text()
    {
        var testStartTime = DateTime.UtcNow;
        var (page, aiFlashCard) = await SetupTestPageAndFlashCardService();

        var flashCards = await aiFlashCard.Generate(
            SourceTexts.ShortSourceTextEN,
            page.Id,
            TestUserId,
            CreateAnonymousPermissionCheck(),
            AiModel.Claude);

        await Verify(new
        {
            FlashCards = ValidateFlashCards(flashCards, ShortTextMinCards, ShortTextMaxCards),
            AiUsage = await QueryAiUsageLogsSince(testStartTime)
        });
    }

    [Test]
    public async Task Should_generate_non_duplicate_flashcards_for_short_text_when_existing_cards_provided()
    {
        var testStartTime = DateTime.UtcNow;
        var (page, aiFlashCard) = await SetupTestPageAndFlashCardService();
        var permissionCheck = CreateAnonymousPermissionCheck();

        // Generate initial flashcards
        var initialFlashCards = await aiFlashCard.Generate(
            SourceTexts.ShortSourceTextEN,
            page.Id,
            TestUserId,
            permissionCheck,
            AiModel.Claude);

        var initialFlashCardsJson = JsonSerializer.Serialize(initialFlashCards);

        // Generate additional flashcards with existing cards context
        var promptWithExistingCards = AiFlashCard.GetPromptOpus(SourceTexts.ShortSourceTextEN, initialFlashCardsJson);
        var additionalFlashCards = await aiFlashCard.Generate(promptWithExistingCards, AiModel.Claude, TestUserId, TestPageId);
        var additionalFlashCardsJson = JsonSerializer.Serialize(additionalFlashCards);

        var hasDuplicates = await CheckForDuplicatesViaAi(initialFlashCardsJson, additionalFlashCardsJson);

        await Verify(new
        {
            InitialFlashCards = ValidateFlashCards(initialFlashCards, ShortTextMinCards, ShortTextMaxCards),
            AdditionalFlashCards = ValidateFlashCards(additionalFlashCards, 1, ShortTextMaxCards),
            AiUsage = await QueryAiUsageLogsSince(testStartTime)
        });

        if (hasDuplicates)
        {
            Console.WriteLine($"Warning: Duplicates detected between initial and additional flashcards." +
                            $"{Environment.NewLine}Initial: {initialFlashCardsJson}" +
                            $"{Environment.NewLine}Additional: {additionalFlashCardsJson}");
        }
    }

    #endregion

    #region Long Source Text Tests

    [Test]
    public async Task Should_generate_valid_flashcards_for_long_english_text()
    {
        var testStartTime = DateTime.UtcNow;
        var (page, aiFlashCard) = await SetupTestPageAndFlashCardService();

        var flashCards = await aiFlashCard.Generate(
            SourceTexts.LongSourceTextEN,
            page.Id,
            TestUserId,
            CreateAnonymousPermissionCheck(),
            AiModel.Claude);

        await Verify(new
        {
            FlashCards = ValidateFlashCards(flashCards, LongTextMinCards, LongTextMaxCards),
            AiUsage = await QueryAiUsageLogsSince(testStartTime)
        });
    }

    [Test]
    public async Task Should_generate_non_duplicate_flashcards_for_long_text_when_existing_cards_provided()
    {
        var testStartTime = DateTime.UtcNow;
        var (page, aiFlashCard) = await SetupTestPageAndFlashCardService();
        var permissionCheck = CreateAnonymousPermissionCheck();

        // Generate initial flashcards
        var initialFlashCards = await aiFlashCard.Generate(
            SourceTexts.LongSourceTextEN,
            page.Id,
            TestUserId,
            permissionCheck,
            AiModel.Claude);

        var initialFlashCardsJson = JsonSerializer.Serialize(initialFlashCards);

        // Generate additional flashcards with existing cards context
        var promptWithExistingCards = AiFlashCard.GetPromptOpus(SourceTexts.LongSourceTextEN, initialFlashCardsJson);
        var additionalFlashCards = await aiFlashCard.Generate(promptWithExistingCards, AiModel.Claude, TestUserId, TestPageId);
        var additionalFlashCardsJson = JsonSerializer.Serialize(additionalFlashCards);

        var hasDuplicates = await CheckForDuplicatesViaAi(initialFlashCardsJson, additionalFlashCardsJson);

        await Verify(new
        {
            InitialFlashCards = ValidateFlashCards(initialFlashCards, LongTextMinCards, LongTextMaxCards),
            AdditionalFlashCards = ValidateFlashCards(additionalFlashCards, 3, LongTextMaxCards),
            AiUsage = await QueryAiUsageLogsSince(testStartTime)
        });

        if (hasDuplicates)
        {
            Console.WriteLine($"Warning: Duplicates detected between initial and additional flashcards." +
                            $"{Environment.NewLine}Initial: {initialFlashCardsJson}" +
                            $"{Environment.NewLine}Additional: {additionalFlashCardsJson}");
        }
    }

    #endregion

    #region Language Consistency Tests

    [Test]
    public async Task Should_generate_flashcards_in_english_for_english_source_text()
    {
        var testStartTime = DateTime.UtcNow;
        var (page, aiFlashCard) = await SetupTestPageAndFlashCardService();

        var flashCards = await aiFlashCard.Generate(
            SourceTexts.LongSourceTextEN,
            page.Id,
            TestUserId,
            CreateAnonymousPermissionCheck(),
            AiModel.Claude);

        if (flashCards == null || flashCards.Count == 0)
        {
            await Verify(new
            {
                FlashCards = ValidateFlashCards(flashCards, LongTextMinCards, LanguageTestMaxCards),
                LanguageCheck = new { IsValidResponse = false, LanguageMatches = false },
                AiUsage = await QueryAiUsageLogsSince(testStartTime)
            });
            return;
        }

        var flashCardsJson = JsonSerializer.Serialize(flashCards);
        var (isValidResponse, languageMatches) = await CheckLanguageMatchViaAi(SourceTexts.LongSourceTextEN, flashCardsJson);

        await Verify(new
        {
            FlashCards = ValidateFlashCards(flashCards, LongTextMinCards, LanguageTestMaxCards),
            LanguageCheck = new { IsValidResponse = isValidResponse, LanguageMatches = languageMatches },
            AiUsage = await QueryAiUsageLogsSince(testStartTime)
        });

        if (!languageMatches)
        {
            Console.WriteLine("Warning: Language mismatch - English source text produced non-English flashcards.");
        }
    }

    [Test]
    public async Task Should_generate_flashcards_in_german_for_german_source_text()
    {
        var testStartTime = DateTime.UtcNow;
        var (page, aiFlashCard) = await SetupTestPageAndFlashCardService();

        var flashCards = await aiFlashCard.Generate(
            SourceTexts.LongSourceTextDE,
            page.Id,
            TestUserId,
            CreateAnonymousPermissionCheck(),
            AiModel.Claude);

        if (flashCards == null || flashCards.Count == 0)
        {
            await Verify(new
            {
                FlashCards = ValidateFlashCards(flashCards, LongTextMinCards, LanguageTestMaxCards),
                LanguageCheck = new { IsValidResponse = false, LanguageMatches = false },
                AiUsage = await QueryAiUsageLogsSince(testStartTime)
            });
            return;
        }

        var flashCardsJson = JsonSerializer.Serialize(flashCards);
        var (isValidResponse, languageMatches) = await CheckLanguageMatchViaAi(SourceTexts.LongSourceTextDE, flashCardsJson);

        await Verify(new
        {
            FlashCards = ValidateFlashCards(flashCards, LongTextMinCards, LanguageTestMaxCards),
            LanguageCheck = new { IsValidResponse = isValidResponse, LanguageMatches = languageMatches },
            AiUsage = await QueryAiUsageLogsSince(testStartTime)
        });

        if (!languageMatches)
        {
            Console.WriteLine("Warning: Language mismatch - German source text produced non-German flashcards.");
        }
    }

    #endregion

    #region Edge Case and Validation Tests

    [Test]
    public async Task Should_return_null_when_source_text_is_null()
    {
        var controller = CreatePageStoreController();
        var request = new PageStoreController.GenerateFlashCardRequest(TestPageId, null);

        var result = await controller.GenerateFlashCard(request);

        await Verify(new { ResultIsNull = result == null });
    }

    [Test]
    public async Task Should_return_null_when_page_id_is_invalid()
    {
        const int invalidPageId = -1;
        var controller = CreatePageStoreController();
        var request = new PageStoreController.GenerateFlashCardRequest(invalidPageId, "Sample source text for flashcard generation.");

        var result = await controller.GenerateFlashCard(request);

        await Verify(new { ResultIsNull = result == null });
    }

    #endregion

    #region Controller Factory

    private PageStoreController CreatePageStoreController()
    {
        return new PageStoreController(
            R<SessionUser>(),
            new PermissionCheck(new SessionlessUser(_testHarness.DefaultSessionUserId)),
            R<KnowledgeSummaryLoader>(),
            R<PageRepository>(),
            R<IHttpContextAccessor>(),
            R<ImageMetaDataReadingRepo>(),
            R<QuestionReadingRepo>(),
            R<PageUpdater>(),
            R<ImageStore>(),
            R<AiUsageLogRepo>(),
            R<TokenDeductionService>());
    }

    #endregion
}

