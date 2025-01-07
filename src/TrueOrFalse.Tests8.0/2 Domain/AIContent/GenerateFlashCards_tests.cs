
//using FakeItEasy;
//using OpenAI.Chat;
//using System.Text.Json;
//using TrueOrFalse;
//class GenerateFlashCards_tests : BaseTest
//{

//    private const int PageId = 1;
//    private const string SourceText = "Sample source text for generating flashcards.";
//    private const string LongSourceText = ""

//    [Test]
//    public async Task Generate




//    [Test]
//    public async Task Generate_Returns_CorrectFlashCards_When_AiResponseIsValid()
//    {
//        // Arrange
//        // Add existing flashcards to EntityCache
//        var existingQuestions = new List<Question>
//            {
//                new Question
//                {
//                    TextHtml = "Existing Front 1",
//                    Solution = JsonSerializer.Serialize(new AiFlashCard.BackJson("Existing Back 1")),
//                    SolutionType = SolutionType.FlashCard
//                },
//                new Question
//                {
//                    TextHtml = "Existing Front 2",
//                    Solution = JsonSerializer.Serialize(new AiFlashCard.BackJson("Existing Back 2")),
//                    SolutionType = SolutionType.FlashCard
//                }
//            };
//        EntityCache.AddQuestionsForPage(PageId, existingQuestions);

//        // Mock ChatClient behavior
//        var fakeChatCompletion = new ChatCompletion
//        {
//            Content = new List<ChatContent>
//                {
//                    new ChatContent { Text = JsonSerializer.Serialize(new List<AiFlashCard.FlashCard>
//                    {
//                        new AiFlashCard.FlashCard("New Front 1", "New Back 1"),
//                        new AiFlashCard.FlashCard("New Front 2", "New Back 2")
//                    })}
//                }
//        };

//        // Fake the ChatClient's CompleteChatAsync method
//        A.CallTo(() => ChatClientWrapper.CompleteChatAsync(A<string>.Ignored))
//            .Returns(Task.FromResult(fakeChatCompletion));

//        // Act
//        var result = await _aiFlashCard.Generate(SourceText, PageId, _permissionCheck);

//        // Assert
//        Assert.NotNull(result);
//        Assert.AreEqual(2, result.Count);
//        Assert.IsTrue(result.Exists(fc => fc.Front == "New Front 1" && fc.Back == "New Back 1"));
//        Assert.IsTrue(result.Exists(fc => fc.Front == "New Front 2" && fc.Back == "New Back 2"));
//    }

//    [Test]
//    public async Task Generate_DoesNotInclude_DuplicateFlashCards()
//    {
//        // Arrange
//        // Add existing flashcards to EntityCache
//        var existingQuestions = new List<Question>
//            {
//                new Question
//                {
//                    TextHtml = "Existing Front",
//                    Solution = JsonSerializer.Serialize(new AiFlashCard.BackJson("Existing Back")),
//                    SolutionType = SolutionType.FlashCard
//                }
//            };
//        EntityCache.AddQuestionsForPage(PageId, existingQuestions);

//        // Mock ChatClient behavior with a duplicate flashcard
//        var fakeChatCompletion = new ChatCompletion
//        {
//            Content = new List<ChatContent>
//                {
//                    new ChatContent { Text = JsonSerializer.Serialize(new List<AiFlashCard.FlashCard>
//                    {
//                        new AiFlashCard.FlashCard("Existing Front", "Existing Back"), // Duplicate
//                        new AiFlashCard.FlashCard("Unique Front", "Unique Back")
//                    })}
//                }
//        };

//        // Fake the ChatClient's CompleteChatAsync method
//        A.CallTo(() => ChatClientWrapper.CompleteChatAsync(A<string>.Ignored))
//            .Returns(Task.FromResult(fakeChatCompletion));

//        // Act
//        var result = await _aiFlashCard.Generate(SourceText, PageId, _permissionCheck);

//        // Assert
//        Assert.NotNull(result);
//        Assert.AreEqual(1, result.Count);
//        Assert.IsTrue(result.Exists(fc => fc.Front == "Unique Front" && fc.Back == "Unique Back"));
//        Assert.IsFalse(result.Exists(fc => fc.Front == "Existing Front" && fc.Back == "Existing Back"));
//    }

//    [Test]
//    public async Task Generate_Returns_EmptyList_When_AiReturns_NoFlashCards()
//    {
//        // Arrange
//        // No existing flashcards
//        EntityCache.AddQuestionsForPage(PageId, new List<Question>());

//        // Mock ChatClient behavior with empty flashcards
//        var fakeChatCompletion = new ChatCompletion
//        {
//            Content = new List<ChatContent>
//                {
//                    new ChatContent { Text = JsonSerializer.Serialize(new List<AiFlashCard.FlashCard>()) }
//                }
//        };

//        // Fake the ChatClient's CompleteChatAsync method
//        A.CallTo(() => ChatClientWrapper.CompleteChatAsync(A<string>.Ignored))
//            .Returns(Task.FromResult(fakeChatCompletion));

//        // Act
//        var result = await _aiFlashCard.Generate(SourceText, PageId, _permissionCheck);

//        // Assert
//        Assert.NotNull(result);
//        Assert.IsEmpty(result);
//    }

//    [Test]
//    public async Task Generate_Returns_EmptyList_When_AiReturns_InvalidJson()
//    {
//        // Arrange
//        // No existing flashcards
//        EntityCache.AddQuestionsForPage(PageId, new List<Question>());

//        // Mock ChatClient behavior with invalid JSON
//        var invalidJson = "This is not a valid JSON string.";

//        var fakeChatCompletion = new ChatCompletion
//        {
//            Content = new List<ChatContent>
//                {
//                    new ChatContent { Text = invalidJson }
//                }
//        };

//        // Fake the ChatClient's CompleteChatAsync method
//        A.CallTo(() => ChatClientWrapper.CompleteChatAsync(A<string>.Ignored))
//            .Returns(Task.FromResult(fakeChatCompletion));

//        // Act
//        var result = await _aiFlashCard.Generate(SourceText, PageId, _permissionCheck);

//        // Assert
//        Assert.NotNull(result);
//        Assert.IsEmpty(result);
//    }

//    [Test]
//    public async Task Generate_Returns_EmptyList_When_AiResponseIsEmpty()
//    {
//        // Arrange
//        // No existing flashcards
//        EntityCache.AddQuestionsForPage(PageId, new List<Question>());

//        // Mock ChatClient behavior with empty content
//        var fakeChatCompletion = new ChatCompletion
//        {
//            Content = new List<ChatContent>() // No content returned
//        };

//        // Fake the ChatClient's CompleteChatAsync method
//        A.CallTo(() => ChatClientWrapper.CompleteChatAsync(A<string>.Ignored))
//            .Returns(Task.FromResult(fakeChatCompletion));

//        // Act
//        var result = await _aiFlashCard.Generate(SourceText, PageId, _permissionCheck);

//        // Assert
//        Assert.NotNull(result);
//        Assert.IsEmpty(result);
//    }

//    // Additional tests can be added here to cover more scenarios
//}

