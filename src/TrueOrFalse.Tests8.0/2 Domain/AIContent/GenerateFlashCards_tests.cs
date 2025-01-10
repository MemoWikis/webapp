using System.Text.Json;
class GenerateFlashCards_tests : BaseTest
{
    private const int PageId = 1;
    private const string ClaudeSonnetModel = "claude-3-5-sonnet-20241022";
    private const int DefaultUserId = 1;

    [Test]
    public async Task Should_generate_flashcards_for_shortSourceText()
    {
        // Arrange
        var context = ContextPage.New();

        // Assuming the test page needs to exist
        context.Add("TestPage").Persist();
        var page = context.All.ByName("TestPage");
        context.AddToEntityCache(page);

        var permissionCheck = new PermissionCheck(-1); // Default user ID for tests

        // Optionally, initialize entity cache or other dependencies
        RecycleContainerAndEntityCache();

        // Act
        var flashCards = await AiFlashCard.Generate(SourceTexts.ShortSourceTextEN, page.Id, DefaultUserId, permissionCheck, AiModel.Claude);

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
        var context = ContextPage.New();

        // Assuming the test page needs to exist
        context.Add("TestPage").Persist();
        var page = context.All.ByName("TestPage");
        context.AddToEntityCache(page);

        var permissionCheck = new PermissionCheck(-1); // Default user ID for tests

        // Optionally, initialize entity cache or other dependencies
        RecycleContainerAndEntityCache();

        // Act
        var flashCards = await AiFlashCard.Generate(SourceTexts.ShortSourceTextEN, page.Id, DefaultUserId, permissionCheck, AiModel.Claude);

        // Assert
        Assert.That(flashCards, Is.Not.Null, "Flashcards should not be null.");
        Assert.That(flashCards.Count, Is.GreaterThanOrEqualTo(2), "Flashcard count should pass min threshold.");
        Assert.That(flashCards.Count, Is.LessThan(9), "Flashcard count should not pass sensible threshold.");
    }

    [Test]
    public async Task Should_not_generate_duplicates_for_shortSourceText()
    {
        // Arrange
        var context = ContextPage.New();

        // Assuming the test page needs to exist
        context.Add("TestPage").Persist();
        var page = context.All.ByName("TestPage");
        context.AddToEntityCache(page);

        var permissionCheck = new PermissionCheck(-1); // Default user ID for tests

        // Optionally, initialize entity cache or other dependencies
        RecycleContainerAndEntityCache();

        // Act
        var flashCardsBase = await AiFlashCard.Generate(SourceTexts.ShortSourceTextEN, page.Id, DefaultUserId, permissionCheck, AiModel.Claude);
        var flashCardsBaseJson = JsonSerializer.Serialize(flashCardsBase);

        var newPrompt = AiFlashCard.GetPromptOpus(SourceTexts.ShortSourceTextEN, flashCardsBaseJson);
        var newFlashCards = await AiFlashCard.Generate(newPrompt, AiModel.Claude, DefaultUserId, PageId);
        var newFlashCardsJson = JsonSerializer.Serialize(newFlashCards);

        //Assert
        var assertPrompt = $@"Hier sind zwei Listen die Karteikarten mit den Eigenschaften \""Front\"" und \""Back\"". 
            Liste A: {flashCardsBaseJson} und Liste B: {newFlashCardsJson}.
            Vergleiche beide Listen ob es inhaltlich/fachliche Duplikate gibt.
            Teilweisende/Partiell überlappende Inhalte sind okay und sollten nicht als Duplikate gewertet werden.
            Falls es Duplikate gibt antworte NUR mit true.
            Falls es keine Duplikate gibt antworte NUR mit false.";

        var requestJson = ClaudeService.GetRequestJson(assertPrompt, ClaudeSonnetModel);
        var claudeResponse = await ClaudeService.GetClaudeResponse(requestJson);

        Assert.NotNull(claudeResponse);
        Assert.That(claudeResponse!.Content[0].Text, Is.EqualTo("false"));
    }

    [Test]
    public async Task Should_generate_flashcards_for_longSourceText()
    {
        // Arrange
        var context = ContextPage.New();

        // Assuming the test page needs to exist
        context.Add("TestPage").Persist();
        var page = context.All.ByName("TestPage");
        context.AddToEntityCache(page);

        var permissionCheck = new PermissionCheck(-1); // Default user ID for tests

        // Optionally, initialize entity cache or other dependencies
        RecycleContainerAndEntityCache();

        // Act
        var flashCards = await AiFlashCard.Generate(SourceTexts.LongSourceTextEN, page.Id, DefaultUserId, permissionCheck, AiModel.Claude);
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
        var context = ContextPage.New();

        // Assuming the test page needs to exist
        context.Add("TestPage").Persist();
        var page = context.All.ByName("TestPage");
        context.AddToEntityCache(page);

        var permissionCheck = new PermissionCheck(-1); // Default user ID for tests

        // Optionally, initialize entity cache or other dependencies
        RecycleContainerAndEntityCache();

        // Act
        var flashCards = await AiFlashCard.Generate(SourceTexts.LongSourceTextEN, page.Id, DefaultUserId, permissionCheck, AiModel.Claude);

        // Assert
        Assert.That(flashCards, Is.Not.Null, "Flashcards should not be null.");
        Assert.That(flashCards.Count, Is.GreaterThanOrEqualTo(5), "Flashcard count should pass min threshold.");
        Assert.That(flashCards.Count, Is.LessThan(20), "Flashcard count should not pass sensible threshold.");
    }


    [Test]
    public async Task Should_not_generate_duplicates_for_longSourceText()
    {
        // Arrange
        var context = ContextPage.New();

        // Assuming the test page needs to exist
        context.Add("TestPage").Persist();
        var page = context.All.ByName("TestPage");
        context.AddToEntityCache(page);

        var permissionCheck = new PermissionCheck(-1); // Default user ID for tests

        // Optionally, initialize entity cache or other dependencies
        RecycleContainerAndEntityCache();

        // Act
        var flashCardsBase = await AiFlashCard.Generate(SourceTexts.LongSourceTextEN, page.Id, DefaultUserId, permissionCheck, AiModel.Claude);
        var flashCardsBaseJson = JsonSerializer.Serialize(flashCardsBase);

        var newPrompt = AiFlashCard.GetPromptOpus(SourceTexts.ShortSourceTextEN, flashCardsBaseJson);
        var newFlashCards = await AiFlashCard.Generate(newPrompt, AiModel.Claude, DefaultUserId, PageId);
        var newFlashCardsJson = JsonSerializer.Serialize(newFlashCards);

        //Assert
        var assertPrompt = $@"Hier sind zwei Listen die Karteikarten mit den Eigenschaften \""Front\"" und \""Back\"". 
            Liste A: {flashCardsBaseJson} und Liste B: {newFlashCardsJson}.
            Vergleiche beide Listen ob es inhaltlich/fachliche Duplikate gibt.
            Falls es Duplikate gibt antworte NUR mit true oder fall es keine Duplikate gibt antworte NUR mit false. Keine Erklärung.";

        var requestJson = ClaudeService.GetRequestJson(assertPrompt, ClaudeSonnetModel);
        var claudeResponse = await ClaudeService.GetClaudeResponse(requestJson);

        Assert.NotNull(claudeResponse);
        Assert.That(claudeResponse!.Content[0].Text, Is.EqualTo("false"));
    }

    [Test]
    public async Task Should_generate_flashcards_in_same_language_Long_EN()
    {
        // Arrange
        var context = ContextPage.New();

        // Assuming the test page needs to exist
        context.Add("TestPage").Persist();
        var page = context.All.ByName("TestPage");
        context.AddToEntityCache(page);

        var permissionCheck = new PermissionCheck(-1); // Default user ID for tests

        // Optionally, initialize entity cache or other dependencies
        RecycleContainerAndEntityCache();

        // Act
        var flashCards = await AiFlashCard.Generate(SourceTexts.LongSourceTextEN, page.Id, DefaultUserId, permissionCheck, AiModel.ChatGPT);
        var flashCardsJson = JsonSerializer.Serialize(flashCards);

        //Assert
        var assertPrompt = $@"Überprüfe ob die Sprache dieses Textes: {SourceTexts.LongSourceTextEN} und diese Karteikarten: {flashCardsJson} übereinstimmen. Antworte nur mit true oder false, füge keine Erklärung hinzu.";

        var requestJson = ClaudeService.GetRequestJson(assertPrompt, ClaudeSonnetModel);
        var claudeResponse = await ClaudeService.GetClaudeResponse(requestJson);

        Assert.NotNull(claudeResponse);
        Assert.That(claudeResponse!.Content[0].Text, Is.EqualTo("true"));
    }

    [Test]
    public async Task Should_generate_flashcards_in_same_language_Long_DE()
    {
        // Arrange
        var context = ContextPage.New();

        // Assuming the test page needs to exist
        context.Add("TestPage").Persist();
        var page = context.All.ByName("TestPage");
        context.AddToEntityCache(page);

        var permissionCheck = new PermissionCheck(-1); // Default user ID for tests

        // Optionally, initialize entity cache or other dependencies
        RecycleContainerAndEntityCache();

        // Act
        var flashCards = await AiFlashCard.Generate(SourceTexts.LongSourceTextDE, page.Id, DefaultUserId, permissionCheck, AiModel.ChatGPT);
        var flashCardsJson = JsonSerializer.Serialize(flashCards);

        //Assert
        var assertPrompt = $@"Überprüfe ob die Sprache dieses Textes: {SourceTexts.LongSourceTextDE} und diese Karteikarten: {flashCardsJson} übereinstimmen. Antworte nur mit true oder false, füge keine Erklärung hinzu.";

        var requestJson = ClaudeService.GetRequestJson(assertPrompt, ClaudeSonnetModel);
        var claudeResponse = await ClaudeService.GetClaudeResponse(requestJson);

        Assert.NotNull(claudeResponse);
        Assert.That(claudeResponse!.Content[0].Text, Is.EqualTo("true"));
    }
}

