using System.Text.Json;
class GenerateFlashCards_tests : BaseTest
{
    private const int PageId = 1;
    private const string ShortSourceText = SourceTexts.ShortSourceText;
    //private const string MediumSourceText = SourceTexts.MediumSourceText;
    private const string LongSourceText = SourceTexts.LongSourceText;

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
        var flashCards = await AiFlashCard.Generate(ShortSourceText, page.Id, permissionCheck, AiModel.Claude);
        Console.WriteLine("Should_generate_flashcards_for_shortSourceText");

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
        var flashCards = await AiFlashCard.Generate(ShortSourceText, page.Id, permissionCheck, AiModel.Claude);

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
        var flashCardsBase = await AiFlashCard.Generate(ShortSourceText, page.Id, permissionCheck, AiModel.Claude);
        var flashCardsBaseJson = JsonSerializer.Serialize(flashCardsBase);

        var newFlashCards = await AiFlashCard.Generate(ShortSourceText, page.Id, permissionCheck, AiModel.Claude);
        var newFlashCardsJson = JsonSerializer.Serialize(newFlashCards);

        //Assert
        var assertPrompt = $@"Hier sind zwei Listen die Karteikarten mit den Eigenschaften \""Front\"" und \""Back\"". 
            Liste A: {flashCardsBaseJson} und Liste B: {newFlashCardsJson}.
            Vergleiche beide Listen ob es inhaltlich/fachliche Duplikate gibt.
            Falls es Duplikate gibt antworte mit true.
            Falls es keine Duplikate gibt antworte mit false.";

        var response = ClaudeService.GetJsonData(assertPrompt);
        var claudeResponse = await ClaudeService.GetClaudeResponse(response);

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
        var flashCards = await AiFlashCard.Generate(LongSourceText, page.Id, permissionCheck, AiModel.Claude);
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
        var flashCards = await AiFlashCard.Generate(LongSourceText, page.Id, permissionCheck, AiModel.Claude);

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
        var flashCardsBase = await AiFlashCard.Generate(LongSourceText, page.Id, permissionCheck, AiModel.Claude);
        var flashCardsBaseJson = JsonSerializer.Serialize(flashCardsBase);

        var newFlashCards = await AiFlashCard.Generate(LongSourceText, page.Id, permissionCheck, AiModel.Claude);
        var newFlashCardsJson = JsonSerializer.Serialize(newFlashCards);

        //Assert
        var assertPrompt = $@"Hier sind zwei Listen die Karteikarten mit den Eigenschaften \""Front\"" und \""Back\"". 
            Liste A: {flashCardsBaseJson} und Liste B: {newFlashCardsJson}.
            Vergleiche beide Listen ob es inhaltlich/fachliche Duplikate gibt.
            Falls es Duplikate gibt antworte mit true.
            Falls es keine Duplikate gibt antworte mit false.";

        var response = ClaudeService.GetJsonData(assertPrompt);
        var claudeResponse = await ClaudeService.GetClaudeResponse(response);

        Assert.NotNull(claudeResponse);
        Assert.That(claudeResponse!.Content[0].Text, Is.EqualTo("false"));
    }
}

