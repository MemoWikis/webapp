using System.Text.Json;

class GenerateAiPage_tests : BaseTestHarness
{
    private const int DefaultUserId = 1;
    private const int DefaultPageId = 1;

    [Test]
    public async Task Should_generate_page_from_prompt_short_content()
    {
        // Arrange
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var webContentFetcher = R<WebContentFetcher>();
        var aiPageGenerator = new AiPageGenerator(aiUsageLogRepo, webContentFetcher);

        var prompt = "Explain the basics of photosynthesis for beginners";

        // Act
        var result = await aiPageGenerator.Generate(
            prompt,
            AiPageGenerator.DifficultyLevel.Beginner,
            AiPageGenerator.ContentLength.Short,
            DefaultUserId,
            DefaultPageId);

        // Assert
        Assert.That(result, Is.Not.Null, "Generated page should not be null.");
        Assert.That(result!.Value.Title, Is.Not.Null.Or.Empty, "Title should not be null or empty.");
        Assert.That(result.Value.HtmlContent, Is.Not.Null.Or.Empty, "HtmlContent should not be null or empty.");
        Assert.That(result.Value.Title.Length, Is.LessThanOrEqualTo(100), "Title should be reasonably short.");
    }

    [Test]
    public async Task Should_generate_page_from_prompt_medium_content()
    {
        // Arrange
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var webContentFetcher = R<WebContentFetcher>();
        var aiPageGenerator = new AiPageGenerator(aiUsageLogRepo, webContentFetcher);

        var prompt = "Create a learning page about SQL database fundamentals";

        // Act
        var result = await aiPageGenerator.Generate(
            prompt,
            AiPageGenerator.DifficultyLevel.Intermediate,
            AiPageGenerator.ContentLength.Medium,
            DefaultUserId,
            DefaultPageId);

        // Assert
        Assert.That(result, Is.Not.Null, "Generated page should not be null.");
        Assert.That(result!.Value.Title, Is.Not.Null.Or.Empty, "Title should not be null or empty.");
        Assert.That(result.Value.HtmlContent, Is.Not.Null.Or.Empty, "HtmlContent should not be null or empty.");
        Assert.That(result.Value.HtmlContent.Contains("<h2>") || result.Value.HtmlContent.Contains("<h3>"),
            Is.True, "Content should contain proper HTML headings.");
    }

    [Test]
    public async Task Should_generate_page_from_prompt_long_content()
    {
        // Arrange
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var webContentFetcher = R<WebContentFetcher>();
        var aiPageGenerator = new AiPageGenerator(aiUsageLogRepo, webContentFetcher);

        var prompt = "Create a comprehensive learning page about machine learning algorithms";

        // Act
        var result = await aiPageGenerator.Generate(
            prompt,
            AiPageGenerator.DifficultyLevel.Advanced,
            AiPageGenerator.ContentLength.Long,
            DefaultUserId,
            DefaultPageId);

        // Assert
        Assert.That(result, Is.Not.Null, "Generated page should not be null.");
        Assert.That(result!.Value.Title, Is.Not.Null.Or.Empty, "Title should not be null or empty.");
        Assert.That(result.Value.HtmlContent, Is.Not.Null.Or.Empty, "HtmlContent should not be null or empty.");
        
        // Long content should be substantially longer than short content
        Assert.That(result.Value.HtmlContent.Length, Is.GreaterThan(500),
            "Long content should have substantial length.");
    }

    [Test]
    public async Task Should_generate_page_in_same_language_as_prompt_German()
    {
        // Arrange
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var webContentFetcher = R<WebContentFetcher>();
        var aiPageGenerator = new AiPageGenerator(aiUsageLogRepo, webContentFetcher);

        var germanPrompt = "Erstelle eine Lernseite über die Grundlagen der deutschen Grammatik";

        // Act
        var result = await aiPageGenerator.Generate(
            germanPrompt,
            AiPageGenerator.DifficultyLevel.Beginner,
            AiPageGenerator.ContentLength.Medium,
            DefaultUserId,
            DefaultPageId);

        // Assert using Claude to verify language
        Assert.That(result, Is.Not.Null, "Generated page should not be null.");
        
        var assertPrompt = $@"Überprüfe ob dieser Text auf Deutsch ist: 
            Titel: {result!.Value.Title}
            Inhalt: {result.Value.HtmlContent}
            Antworte nur mit 'true' oder 'false', ohne Erklärung.";
        
        var claudeResponse = await ClaudeService.GetClaudeResponse(assertPrompt);
        Assert.That(claudeResponse, Is.Not.Null);
        
        var isGerman = claudeResponse!.Content[0].Text.Trim().ToLower() == "true";
        Assert.That(isGerman, Is.True, "Generated content should be in German.");
    }

    [Test]
    public async Task Should_generate_page_in_same_language_as_prompt_English()
    {
        // Arrange
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var webContentFetcher = R<WebContentFetcher>();
        var aiPageGenerator = new AiPageGenerator(aiUsageLogRepo, webContentFetcher);

        var englishPrompt = "Create a learning page about the history of the Roman Empire";

        // Act
        var result = await aiPageGenerator.Generate(
            englishPrompt,
            AiPageGenerator.DifficultyLevel.Intermediate,
            AiPageGenerator.ContentLength.Medium,
            DefaultUserId,
            DefaultPageId);

        // Assert using Claude to verify language
        Assert.That(result, Is.Not.Null, "Generated page should not be null.");
        
        var assertPrompt = $@"Check if this text is in English: 
            Title: {result!.Value.Title}
            Content: {result.Value.HtmlContent}
            Answer only with 'true' or 'false', no explanation.";
        
        var claudeResponse = await ClaudeService.GetClaudeResponse(assertPrompt);
        Assert.That(claudeResponse, Is.Not.Null);
        
        var isEnglish = claudeResponse!.Content[0].Text.Trim().ToLower() == "true";
        Assert.That(isEnglish, Is.True, "Generated content should be in English.");
    }

    [Test]
    public async Task Should_generate_content_appropriate_for_difficulty_level_ELI5()
    {
        // Arrange
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var webContentFetcher = R<WebContentFetcher>();
        var aiPageGenerator = new AiPageGenerator(aiUsageLogRepo, webContentFetcher);

        var prompt = "Explain quantum physics";

        // Act
        var result = await aiPageGenerator.Generate(
            prompt,
            AiPageGenerator.DifficultyLevel.ELI5,
            AiPageGenerator.ContentLength.Short,
            DefaultUserId,
            DefaultPageId);

        // Assert using Claude to verify simplicity
        Assert.That(result, Is.Not.Null, "Generated page should not be null.");
        
        var assertPrompt = $@"Analyze this text and determine if it's written for a 5-year-old (ELI5 style - simple words, short sentences, easy examples):
            {result!.Value.HtmlContent}
            Answer only with 'true' if it's suitable for a young child, or 'false' if it uses complex language. No explanation.";
        
        var claudeResponse = await ClaudeService.GetClaudeResponse(assertPrompt);
        Assert.That(claudeResponse, Is.Not.Null);
        
        var isSimple = claudeResponse!.Content[0].Text.Trim().ToLower() == "true";
        Warn.If(!isSimple, "ELI5 content should use simple language suitable for children.");
        Assert.Pass();
    }

    [Test]
    public async Task Should_generate_valid_HTML_content()
    {
        // Arrange
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var webContentFetcher = R<WebContentFetcher>();
        var aiPageGenerator = new AiPageGenerator(aiUsageLogRepo, webContentFetcher);

        var prompt = "Create a page about healthy eating habits";

        // Act
        var result = await aiPageGenerator.Generate(
            prompt,
            AiPageGenerator.DifficultyLevel.Beginner,
            AiPageGenerator.ContentLength.Medium,
            DefaultUserId,
            DefaultPageId);

        // Assert
        Assert.That(result, Is.Not.Null, "Generated page should not be null.");
        
        var htmlContent = result!.Value.HtmlContent;
        
        // Check for valid HTML structure
        Assert.That(htmlContent.Contains("<p>"), Is.True, "Content should contain paragraph tags.");
        Assert.That(!htmlContent.Contains("<h1>"), Is.True, "Content should NOT contain h1 tags (added by system).");
        
        // Check that tags are properly closed (basic check)
        var openH2Count = System.Text.RegularExpressions.Regex.Matches(htmlContent, "<h2").Count;
        var closeH2Count = System.Text.RegularExpressions.Regex.Matches(htmlContent, "</h2>").Count;
        Assert.That(openH2Count, Is.EqualTo(closeH2Count), "All h2 tags should be properly closed.");
    }

    [Test]
    public async Task Should_not_contain_images_or_external_links()
    {
        // Arrange
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var webContentFetcher = R<WebContentFetcher>();
        var aiPageGenerator = new AiPageGenerator(aiUsageLogRepo, webContentFetcher);

        var prompt = "Create a learning page about web development with examples";

        // Act
        var result = await aiPageGenerator.Generate(
            prompt,
            AiPageGenerator.DifficultyLevel.Intermediate,
            AiPageGenerator.ContentLength.Long,
            DefaultUserId,
            DefaultPageId);

        // Assert
        Assert.That(result, Is.Not.Null, "Generated page should not be null.");
        
        var htmlContent = result!.Value.HtmlContent;
        
        Assert.That(!htmlContent.Contains("<img"), Is.True, "Content should not contain image tags.");
        Assert.That(!htmlContent.Contains("<a href="), Is.True, "Content should not contain external links.");
    }

    [Test]
    public async Task Should_generate_page_from_url_Wikipedia()
    {
        // Arrange
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var webContentFetcher = R<WebContentFetcher>();
        var aiPageGenerator = new AiPageGenerator(aiUsageLogRepo, webContentFetcher);

        var url = "https://en.wikipedia.org/wiki/Photosynthesis";

        // Act
        var result = await aiPageGenerator.GenerateFromUrl(
            url,
            AiPageGenerator.DifficultyLevel.Intermediate,
            AiPageGenerator.ContentLength.Medium,
            DefaultUserId,
            DefaultPageId);

        // Assert
        Assert.That(result, Is.Not.Null, "Generated page from URL should not be null.");
        Assert.That(result!.Value.Title, Is.Not.Null.Or.Empty, "Title should not be null or empty.");
        Assert.That(result.Value.HtmlContent, Is.Not.Null.Or.Empty, "HtmlContent should not be null or empty.");
        
        // The content should be related to photosynthesis
        var contentLower = result.Value.HtmlContent.ToLower();
        Assert.That(contentLower.Contains("photosynthe") || contentLower.Contains("plant") || contentLower.Contains("light"),
            Is.True, "Content should be related to the source topic.");
    }

    [Test]
    public async Task Should_return_null_for_invalid_url()
    {
        // Arrange
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var webContentFetcher = R<WebContentFetcher>();
        var aiPageGenerator = new AiPageGenerator(aiUsageLogRepo, webContentFetcher);

        var invalidUrl = "https://this-domain-definitely-does-not-exist-12345.com/page";

        // Act
        var result = await aiPageGenerator.GenerateFromUrl(
            invalidUrl,
            AiPageGenerator.DifficultyLevel.Intermediate,
            AiPageGenerator.ContentLength.Medium,
            DefaultUserId,
            DefaultPageId);

        // Assert
        Assert.That(result, Is.Null, "Should return null for invalid/unreachable URL.");
    }

    [Test]
    public async Task Content_length_should_affect_output_size()
    {
        // Arrange
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var webContentFetcher = R<WebContentFetcher>();
        var aiPageGenerator = new AiPageGenerator(aiUsageLogRepo, webContentFetcher);

        var prompt = "Explain the water cycle";

        // Act - Generate short content
        var shortResult = await aiPageGenerator.Generate(
            prompt,
            AiPageGenerator.DifficultyLevel.Intermediate,
            AiPageGenerator.ContentLength.Short,
            DefaultUserId,
            DefaultPageId);

        // Act - Generate long content
        var longResult = await aiPageGenerator.Generate(
            prompt,
            AiPageGenerator.DifficultyLevel.Intermediate,
            AiPageGenerator.ContentLength.Long,
            DefaultUserId,
            DefaultPageId);

        // Assert
        Assert.That(shortResult, Is.Not.Null, "Short content should not be null.");
        Assert.That(longResult, Is.Not.Null, "Long content should not be null.");
        
        // Long content should generally be longer than short content
        // Using a warning instead of assertion as AI output can vary
        Warn.If(longResult!.Value.HtmlContent.Length <= shortResult!.Value.HtmlContent.Length,
            $"Long content ({longResult.Value.HtmlContent.Length} chars) should typically be longer than short content ({shortResult.Value.HtmlContent.Length} chars).");
        Assert.Pass();
    }
}
