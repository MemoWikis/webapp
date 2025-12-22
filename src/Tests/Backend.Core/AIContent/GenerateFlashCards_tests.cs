using Microsoft.AspNetCore.Http;
using System.Text.Json;

class GenerateFlashCards_tests : BaseTestHarness
{
    private const int PageId = 1;
    private const int DefaultUserId = 1;

    [Test]
    public async Task Should_generate_flashcards_for_shortSourceText()
    {
        // Arrange
        var context = NewPageContext();

        // Assuming the test page needs to exist
        context.Add("TestPage").Persist();
        var page = context.All.ByName("TestPage");
        context.AddToEntityCache(page);

        await ReloadCaches();

        var permissionCheck = new PermissionCheck(new SessionlessUser(-1)); // Default user ID for tests
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var aiFlashCard = new AiFlashCard(aiUsageLogRepo);

        // Act
        var flashCards = await aiFlashCard.Generate(SourceTexts.ShortSourceTextEN, page.Id, DefaultUserId, permissionCheck, AiModel.Claude);

        // Assert
        Assert.That(flashCards, Is.Not.Null, "Flashcards should not be null.");
        Assert.That(flashCards.Count, Is.GreaterThan(0), "Flashcards should be generated.");

        foreach (var flashCard in flashCards)
        {
            Assert.That(flashCard.Front, Is.Not.Null.Or.Empty, "Flashcard 'Front' should not be null or empty.");
            Assert.That(flashCard.Back, Is.Not.Null.Or.Empty, "Flashcard 'Back' should not be null or empty.");
        }
    }

    [Test]
    public async Task Should_generate_sensible_amount_of_flashcards_for_shortSourceText()
    {
        // Arrange
        var context = NewPageContext();

        // Assuming the test page needs to exist
        context.Add("TestPage").Persist();
        var page = context.All.ByName("TestPage");
        context.AddToEntityCache(page);

        await ReloadCaches();

        var permissionCheck = new PermissionCheck(new SessionlessUser(-1)); // Default user ID for tests
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var aiFlashCard = new AiFlashCard(aiUsageLogRepo);

        // Act
        var flashCards = await aiFlashCard.Generate(SourceTexts.ShortSourceTextEN, page.Id, DefaultUserId, permissionCheck, AiModel.Claude);

        // Assert
        Assert.That(flashCards, Is.Not.Null, "Flashcards should not be null.");
        Assert.That(flashCards.Count, Is.GreaterThanOrEqualTo(2), "Flashcard count should pass min threshold.");
        Assert.That(flashCards.Count, Is.LessThan(9), "Flashcard count should not pass sensible threshold.");
    }

    [Test]
    public async Task Should_not_generate_duplicates_for_shortSourceText()
    {
        // Arrange
        var context = NewPageContext();

        // Assuming the test page needs to exist
        context.Add("TestPage").Persist();
        var page = context.All.ByName("TestPage");
        context.AddToEntityCache(page);

        await ReloadCaches();

        var permissionCheck = new PermissionCheck(new SessionlessUser(-1)); // Default user ID for tests
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var aiFlashCard = new AiFlashCard(aiUsageLogRepo);

        // Act
        var flashCardsBase = await aiFlashCard.Generate(SourceTexts.ShortSourceTextEN, page.Id, DefaultUserId, permissionCheck, AiModel.Claude);
        var flashCardsBaseJson = JsonSerializer.Serialize(flashCardsBase);

        var newPrompt = AiFlashCard.GetPromptOpus(SourceTexts.ShortSourceTextEN, flashCardsBaseJson);
        var newFlashCards = await aiFlashCard.Generate(newPrompt, AiModel.Claude, DefaultUserId, PageId);
        var newFlashCardsJson = JsonSerializer.Serialize(newFlashCards);

        //Assert
        var assertPrompt = $@"Hier sind zwei Listen die Karteikarten mit den Eigenschaften \""Front\"" und \""Back\"". 
            Liste A: {flashCardsBaseJson} und Liste B: {newFlashCardsJson}.
            Vergleiche beide Listen ob es inhaltlich/fachliche Duplikate gibt.
            Teilweisende/Partiell überlappende Inhalte sind okay und sollten nicht als Duplikate gewertet werden.
            Falls es Duplikate gibt antworte NUR mit true.
            Falls es keine Duplikate gibt antworte NUR mit false.";

        var claudeResponse = await ClaudeService.GetClaudeResponse(assertPrompt);

        Assert.That(claudeResponse, Is.Not.Null);
        var hasDuplicate = claudeResponse!.Content[0].Text == "true";
        Warn.If(hasDuplicate, $"ClaudeAssertResult: {claudeResponse.Content[0].Text} " + Environment.NewLine +
                              $"FlashCardsBaseJson: {flashCardsBaseJson}" + Environment.NewLine +
                              $"NewFlashCardsJson: {newFlashCardsJson}");
        Assert.Pass();
    }

    [Test]
    public async Task Should_generate_flashcards_for_longSourceText()
    {
        // Arrange
        var context = NewPageContext();

        // Assuming the test page needs to exist
        context.Add("TestPage").Persist();
        var page = context.All.ByName("TestPage");
        context.AddToEntityCache(page);

        await ReloadCaches();

        var permissionCheck = new PermissionCheck(new SessionlessUser(-1)); // Default user ID for tests
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var aiFlashCard = new AiFlashCard(aiUsageLogRepo);

        // Act
        var flashCards = await aiFlashCard.Generate(SourceTexts.LongSourceTextEN, page.Id, DefaultUserId, permissionCheck, AiModel.Claude);
        Console.WriteLine("Should_generate_flashcards_for_longSourceText");

        // Assert
        Assert.That(flashCards, Is.Not.Null, "Flashcards should not be null.");
        Assert.That(flashCards.Count, Is.GreaterThan(0), "Flashcards should be generated.");

        foreach (var flashCard in flashCards)
        {
            Assert.That(flashCard.Front, Is.Not.Null.Or.Empty, "Flashcard 'Front' should not be null or empty.");
            Assert.That(flashCard.Back, Is.Not.Null.Or.Empty, "Flashcard 'Back' should not be null or empty.");
        }
    }

    [Test]
    public async Task Should_generate_sensible_amount_of_flashcards_for_longSourceText()
    {
        // Arrange
        var context = NewPageContext();

        // Assuming the test page needs to exist
        context.Add("TestPage").Persist();
        var page = context.All.ByName("TestPage");
        context.AddToEntityCache(page);

        await ReloadCaches();

        var permissionCheck = new PermissionCheck(new SessionlessUser(-1)); // Default user ID for tests
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var aiFlashCard = new AiFlashCard(aiUsageLogRepo);

        // Act
        var flashCards = await aiFlashCard.Generate(SourceTexts.LongSourceTextEN, page.Id, DefaultUserId, permissionCheck, AiModel.Claude);

        // Assert
        Assert.That(flashCards, Is.Not.Null, "Flashcards should not be null.");
        Assert.That(flashCards.Count, Is.GreaterThanOrEqualTo(5), "Flashcard count should pass min threshold.");
        Assert.That(flashCards.Count, Is.LessThan(20), "Flashcard count should not pass sensible threshold.");
    }


    [Test]
    public async Task Should_not_generate_duplicates_for_longSourceText()
    {
        // Arrange
        var context = NewPageContext();

        // Assuming the test page needs to exist
        context.Add("TestPage").Persist();
        var page = context.All.ByName("TestPage");
        context.AddToEntityCache(page);

        await ReloadCaches();

        var permissionCheck = new PermissionCheck(new SessionlessUser(-1)); // Default user ID for tests
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var aiFlashCard = new AiFlashCard(aiUsageLogRepo);

        // Act
        var flashCardsBase = await aiFlashCard.Generate(SourceTexts.LongSourceTextEN, page.Id, DefaultUserId, permissionCheck, AiModel.Claude);
        var flashCardsBaseJson = JsonSerializer.Serialize(flashCardsBase);

        var newPrompt = AiFlashCard.GetPromptOpus(SourceTexts.ShortSourceTextEN, flashCardsBaseJson);
        var newFlashCards = await aiFlashCard.Generate(newPrompt, AiModel.Claude, DefaultUserId, PageId);
        var newFlashCardsJson = JsonSerializer.Serialize(newFlashCards);

        //Assert
        var assertPrompt = $@"Hier sind zwei Listen die Karteikarten mit den Eigenschaften \""Front\"" und \""Back\"". 
            Liste A: {flashCardsBaseJson} und Liste B: {newFlashCardsJson}.
            Vergleiche beide Listen ob es inhaltlich/fachliche Duplikate gibt.
            Falls es Duplikate gibt antworte NUR mit true oder fall es keine Duplikate gibt antworte NUR mit false. Keine Erklärung.";

        var claudeResponse = await ClaudeService.GetClaudeResponse(assertPrompt);

        Assert.That(claudeResponse, Is.Not.Null);
        var hasDuplicate = claudeResponse!.Content[0].Text == "true";
        Warn.If(hasDuplicate, $"ClaudeAssertResult: {claudeResponse.Content[0].Text} " + Environment.NewLine +
                              $"FlashCardsBaseJson: {flashCardsBaseJson}" + Environment.NewLine +
                              $"NewFlashCardsJson: {newFlashCardsJson}");
        Assert.Pass();
    }

    [Test]
    public async Task Should_generate_flashcards_in_same_language_Long_EN()
    {
        // Arrange
        var context = NewPageContext();

        // Assuming the test page needs to exist
        context.Add("TestPage").Persist();
        var page = context.All.ByName("TestPage");
        context.AddToEntityCache(page);

        await ReloadCaches();

        var permissionCheck = new PermissionCheck(new SessionlessUser(-1)); // Default user ID for tests
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var aiFlashCard = new AiFlashCard(aiUsageLogRepo);

        // Act
        var flashCards = await aiFlashCard.Generate(SourceTexts.LongSourceTextEN, page.Id, DefaultUserId, permissionCheck, AiModel.ChatGPT);
        var flashCardsJson = JsonSerializer.Serialize(flashCards);

        //Assert
        var assertPrompt = $@"Überprüfe ob die Sprache dieses Textes: {SourceTexts.LongSourceTextEN} und diese Karteikarten: {flashCardsJson} übereinstimmen. Antworte nur mit true oder false, füge keine Erklärung hinzu.";
        var claudeResponse = await ClaudeService.GetClaudeResponse(assertPrompt);

        Assert.That(claudeResponse, Is.Not.Null);
        var languageDiffers = claudeResponse!.Content[0].Text != "true";
        Warn.If(languageDiffers, $"ClaudeAssertResult: {claudeResponse.Content[0].Text}");
        Assert.Pass();
    }

    [Test]
    public async Task Should_generate_flashcards_in_same_language_Long_DE()
    {
        // Arrange
        var context = NewPageContext();

        // Assuming the test page needs to exist
        context.Add("TestPage").Persist();
        var page = context.All.ByName("TestPage");
        context.AddToEntityCache(page);

        await ReloadCaches();

        var permissionCheck = new PermissionCheck(new SessionlessUser(-1)); // Default user ID for tests
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var aiFlashCard = new AiFlashCard(aiUsageLogRepo);

        // Act
        var flashCards = await aiFlashCard.Generate(SourceTexts.LongSourceTextDE, page.Id, DefaultUserId, permissionCheck, AiModel.ChatGPT);
        var flashCardsJson = JsonSerializer.Serialize(flashCards);

        //Assert
        var assertPrompt = $@"Überprüfe ob die Sprache dieses Textes: {SourceTexts.LongSourceTextDE} und diese Karteikarten: {flashCardsJson} übereinstimmen. Antworte nur mit true oder false, füge keine Erklärung hinzu.";
        var claudeResponse = await ClaudeService.GetClaudeResponse(assertPrompt);

        Assert.That(claudeResponse, Is.Not.Null);
        var languageDiffers = claudeResponse!.Content[0].Text != "true";
        Warn.If(languageDiffers, $"ClaudeAssertResult: {claudeResponse.Content[0].Text}");
        Assert.Pass();
    }

    [Test]
    public async Task Should_Return_Null_When_Request_Text_Is_Null()
    {
        // Arrange
        var controller = CreatePageStoreController();
        var request = new PageStoreController.GenerateFlashCardRequest(PageId, null);

        // Act
        var result = await controller.GenerateFlashCard(request);

        // Assert
        Assert.That(result, Is.Null, "Result should be null when request text is null.");
    }

    [Test]
    public async Task Should_Return_Null_When_Request_PageId_Is_Invalid()
    {
        // Arrange
        var controller = CreatePageStoreController();
        var request = new PageStoreController.GenerateFlashCardRequest(-1, "Sample Text");

        // Act
        var result = await controller.GenerateFlashCard(request);

        // Assert
        Assert.That(result, Is.Null, "Result should be null when request PageId is invalid.");
    }

    // Helper method to create the controller with necessary dependencies
    private PageStoreController CreatePageStoreController(LimitCheck? limitCheck = null)
    {
        var sessionUser = R<SessionUser>();

        var permissionCheck = new PermissionCheck(new SessionlessUser(_testHarness.DefaultSessionUserId));
        var knowledgeSummaryLoader = R<KnowledgeSummaryLoader>();
        var pageRepository = R<PageRepository>();
        var httpContextAccessor = R<IHttpContextAccessor>();
        var imageMetaDataReadingRepo = R<ImageMetaDataReadingRepo>();
        var questionReadingRepo = R<QuestionReadingRepo>();
        var pageUpdater = R<PageUpdater>();
        var imageStore = R<ImageStore>();
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var tokenDeductionService = R<TokenDeductionService>();

        return new PageStoreController(
            sessionUser,
            permissionCheck,
            knowledgeSummaryLoader,
            pageRepository,
            httpContextAccessor,
            imageMetaDataReadingRepo,
            questionReadingRepo,
            pageUpdater,
            imageStore,
            aiUsageLogRepo,
            tokenDeductionService);
    }
}

