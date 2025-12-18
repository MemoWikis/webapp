using System.Text.Json;

/// <summary>
/// Tests for the wiki with subpages feature.
/// 
/// Methods in AiPageGenerator:
/// - GenerateWikiWithSubpages(prompt, difficultyLevel, userId, pageId) -> GeneratedWiki
/// - GenerateWikiWithSubpagesFromUrl(url, difficultyLevel, userId, pageId) -> GeneratedWiki
/// 
/// Records:
/// - GeneratedWiki { Title, HtmlContent, List&lt;GeneratedSubpage&gt; Subpages }
/// - GeneratedSubpage { Title, HtmlContent }
/// </summary>
class GenerateAiWikiWithSubpages_tests : BaseTestHarness
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
    public async Task GeneratedWiki_record_structure_and_json_deserialization()
    {
        // Test record structure
        var subpages = new List<AiPageGenerator.GeneratedSubpage>
        {
            new("Subpage 1", "<p>Content 1</p>"),
            new("Subpage 2", "<p>Content 2</p>")
        };
        var wikiFromRecord = new AiPageGenerator.GeneratedWiki("Main Wiki", "<p>Overview</p>", subpages);

        // Test JSON deserialization
        var json = """
        {
            "Title": "Test Wiki",
            "HtmlContent": "<h2>Overview</h2><p>Introduction</p>",
            "Subpages": [
                {
                    "Title": "First Subpage",
                    "HtmlContent": "<h2>First</h2><p>Content</p>"
                },
                {
                    "Title": "Second Subpage",
                    "HtmlContent": "<h2>Second</h2><p>More content</p>"
                }
            ]
        }
        """;

        var wikiFromJson = JsonSerializer.Deserialize<AiPageGenerator.GeneratedWiki>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        await Verify(new
        {
            RecordStructure = new
            {
                wikiFromRecord.Title,
                wikiFromRecord.HtmlContent,
                SubpageCount = wikiFromRecord.Subpages.Count,
                FirstSubpageTitle = wikiFromRecord.Subpages[0].Title,
                SecondSubpageTitle = wikiFromRecord.Subpages[1].Title
            },
            JsonDeserialization = new
            {
                wikiFromJson!.Title,
                ContentContainsOverview = wikiFromJson.HtmlContent.Contains("Overview"),
                SubpageCount = wikiFromJson.Subpages.Count,
                FirstSubpageTitle = wikiFromJson.Subpages[0].Title,
                SecondSubpageTitle = wikiFromJson.Subpages[1].Title
            }
        });
    }

    [Test]
    public async Task GenerateWikiWithSubpages_creates_wiki_with_relevant_subpages()
    {
        // Arrange
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var webContentFetcher = R<WebContentFetcher>();
        var aiPageGenerator = new AiPageGenerator(aiUsageLogRepo, webContentFetcher);

        var prompt = "Create a comprehensive wiki about the Solar System";

        // Act
        var result = await aiPageGenerator.GenerateWikiWithSubpages(
            prompt,
            AiPageGenerator.DifficultyLevel.Intermediate,
            DefaultUserId,
            DefaultPageId);

        // Use AI to validate the wiki structure and content
        var hasRelevantTitle = await AskClaude($"Is this title related to the Solar System or space/astronomy? Title: {result?.Title}");

        var mainContentIsOverview = await AskClaude($@"Is this content an overview/introduction to the Solar System (not detailed about one specific planet)?
            Content: {result?.HtmlContent}");

        var subpageTitles = string.Join(", ", result?.Subpages.Select(s => s.Title) ?? []);
        var subpagesAreRelevant = await AskClaude($@"Are these subpage titles all related to the Solar System topic (planets, moons, sun, asteroids, etc.)?
            Subpage titles: {subpageTitles}");

        var subpagesAreDifferentTopics = await AskClaude($@"Are these subpage titles about DIFFERENT aspects/topics (not duplicates or overlapping)?
            Subpage titles: {subpageTitles}");

        await Verify(new
        {
            WikiGenerated = result != null,
            HasTitle = !string.IsNullOrEmpty(result?.Title),
            HasMainContent = !string.IsNullOrEmpty(result?.HtmlContent),
            HasSubpages = result?.Subpages.Count > 0,
            AllSubpagesHaveTitles = result?.Subpages.All(s => !string.IsNullOrEmpty(s.Title)) ?? false,
            AllSubpagesHaveContent = result?.Subpages.All(s => !string.IsNullOrEmpty(s.HtmlContent)) ?? false,
            AiValidation = new
            {
                TitleIsRelevant = hasRelevantTitle,
                MainContentIsOverview = mainContentIsOverview,
                SubpagesAreRelevant = subpagesAreRelevant,
                SubpagesAreDifferentTopics = subpagesAreDifferentTopics
            }
        });
    }

    [Test]
    public async Task GenerateWikiWithSubpages_respects_language()
    {
        // Arrange
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var webContentFetcher = R<WebContentFetcher>();
        var aiPageGenerator = new AiPageGenerator(aiUsageLogRepo, webContentFetcher);

        var germanPrompt = "Erstelle ein umfassendes Wiki über die Geschichte Deutschlands";
        var frenchPrompt = "Créer un wiki complet sur l'histoire de France";

        // Act
        var germanResult = await aiPageGenerator.GenerateWikiWithSubpages(
            germanPrompt, AiPageGenerator.DifficultyLevel.Intermediate,
            DefaultUserId, DefaultPageId);

        var frenchResult = await aiPageGenerator.GenerateWikiWithSubpages(
            frenchPrompt, AiPageGenerator.DifficultyLevel.Intermediate,
            DefaultUserId, DefaultPageId);

        // Validate languages with AI
        var germanTitleIsGerman = await AskClaude($"Is this title written in German? Title: {germanResult?.Title}");
        var germanContentIsGerman = await AskClaude($"Is this content written in German? Content: {germanResult?.HtmlContent}");
        var germanSubpagesAreGerman = await AskClaude($"Are all these subpage titles written in German? Titles: {string.Join(", ", germanResult?.Subpages.Select(s => s.Title) ?? [])}");

        var frenchTitleIsFrench = await AskClaude($"Is this title written in French? Title: {frenchResult?.Title}");
        var frenchContentIsFrench = await AskClaude($"Is this content written in French? Content: {frenchResult?.HtmlContent}");
        var frenchSubpagesAreFrench = await AskClaude($"Are all these subpage titles written in French? Titles: {string.Join(", ", frenchResult?.Subpages.Select(s => s.Title) ?? [])}");

        await Verify(new
        {
            German = new
            {
                Generated = germanResult != null,
                HasSubpages = germanResult?.Subpages.Count > 0,
                AiConfirmsTitleIsGerman = germanTitleIsGerman,
                AiConfirmsContentIsGerman = germanContentIsGerman,
                AiConfirmsSubpagesAreGerman = germanSubpagesAreGerman
            },
            French = new
            {
                Generated = frenchResult != null,
                HasSubpages = frenchResult?.Subpages.Count > 0,
                AiConfirmsTitleIsFrench = frenchTitleIsFrench,
                AiConfirmsContentIsFrench = frenchContentIsFrench,
                AiConfirmsSubpagesAreFrench = frenchSubpagesAreFrench
            }
        });
    }

    [Test]
    public async Task GenerateWikiWithSubpagesFromUrl_creates_wiki_from_wikipedia()
    {
        // Arrange
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var webContentFetcher = R<WebContentFetcher>();
        var aiPageGenerator = new AiPageGenerator(aiUsageLogRepo, webContentFetcher);

        var url = "https://en.wikipedia.org/wiki/World_War_II";

        // Act
        var result = await aiPageGenerator.GenerateWikiWithSubpagesFromUrl(
            url,
            AiPageGenerator.DifficultyLevel.Intermediate,
            DefaultUserId,
            DefaultPageId);

        // Use AI to validate content relevance
        var titleIsRelevant = await AskClaude($"Is this title related to World War II or WWII? Title: {result?.Title}");
        var contentIsAboutWWII = await AskClaude($"Is this content about World War II (1939-1945, Hitler, Allies, Axis, etc.)? Content: {result?.HtmlContent}");

        var subpageTitles = string.Join(", ", result?.Subpages.Select(s => s.Title) ?? []);
        var subpagesAreWWIITopics = await AskClaude($@"Are these subpage titles all related to World War II topics (battles, countries, leaders, events, etc.)?
            Subpage titles: {subpageTitles}");

        await Verify(new
        {
            WikiGenerated = result != null,
            HasTitle = !string.IsNullOrEmpty(result?.Title),
            HasMainContent = !string.IsNullOrEmpty(result?.HtmlContent),
            HasSubpages = result?.Subpages.Count > 0,
            AiValidation = new
            {
                TitleIsRelevant = titleIsRelevant,
                ContentIsAboutWWII = contentIsAboutWWII,
                SubpagesAreWWIITopics = subpagesAreWWIITopics
            }
        });
    }

    [Test]
    public async Task CreateWiki_should_create_wiki_with_subpages_and_parent_child_relationships()
    {
        // Arrange
        var context = NewPageContext();
        var sessionUser = R<SessionUser>();
        var pageViewRepo = R<PageViewRepo>();
        var pageCreator = R<PageCreator>();
        var pageRepository = R<PageRepository>();
        var permissionCheck = R<PermissionCheck>();

        var sessionUserDb = _testHarness.GetDefaultSessionUserFromDb();

        var rootWikiName = "RootWiki";
        var rootWiki = context
            .Add(rootWikiName, creator: sessionUserDb, isWiki: true)
            .Persist().All
            .Single(page => page.Name.Equals(rootWikiName));

        sessionUser.Login(sessionUserDb, pageViewRepo);

        // Act - Create wiki as child of rootWiki
        var wikiTitle = "AI Generated Wiki";
        var wikiContent = "<h2>Overview</h2><p>This is the main wiki content.</p>";
        var wikiResult = pageCreator.Create(wikiTitle, rootWiki.Id, sessionUser);

        var wikiId = wikiResult.Data.Id;
        var wikiCacheItem = EntityCache.GetPage(wikiId);
        var wikiPage = pageRepository.GetByIdEager(wikiId);

        wikiCacheItem!.Content = wikiContent;
        wikiCacheItem.IsWiki = true;
        wikiPage!.Content = wikiContent;
        wikiPage.IsWiki = true;

        EntityCache.AddOrUpdate(wikiCacheItem);
        pageRepository.Update(wikiPage, sessionUser.UserId, type: PageChangeType.Text);

        // Create subpages with content
        var subpagesData = new List<(string Title, string HtmlContent)>
        {
            ("Subpage Alpha", "<h2>Alpha Topic</h2><p>Content for alpha.</p>"),
            ("Subpage Beta", "<h2>Beta Topic</h2><p>Content for beta.</p>"),
            ("Subpage Gamma", "<h2>Gamma Topic</h2><p>Content for gamma.</p>")
        };

        var subpageIds = new List<int>();
        foreach (var (subpageTitle, subpageContent) in subpagesData)
        {
            var subpageResult = pageCreator.Create(subpageTitle, wikiId, sessionUser);
            var subpageId = subpageResult.Data.Id;

            var subpageCacheItem = EntityCache.GetPage(subpageId);
            var subpagePage = pageRepository.GetByIdEager(subpageId);

            subpageCacheItem!.Content = subpageContent;
            subpagePage!.Content = subpageContent;

            EntityCache.AddOrUpdate(subpageCacheItem);
            pageRepository.Update(subpagePage, sessionUser.UserId, type: PageChangeType.Text);

            subpageIds.Add(subpageId);
        }

        // Reload caches to ensure consistency
        await ReloadCaches();

        // Gather verification data
        var wikiFromDb = pageRepository.GetByIdEager(wikiId);
        var wikiFromCache = EntityCache.GetPage(wikiId);

        var subpagesFromDb = subpageIds.Select(id => pageRepository.GetByIdEager(id)).ToList();
        var subpagesFromCache = subpageIds.Select(id => EntityCache.GetPage(id)).ToList();

        var firstSubpageAncestors = GraphService.VisibleAscendants(subpageIds[0], permissionCheck);

        await Verify(new
        {
            WikiCreation = new
            {
                Success = wikiResult.Success,
                HasValidId = wikiId > 0,
                Name = wikiFromDb!.Name,
                IsWiki = wikiFromDb.IsWiki,
                ContentContainsOverview = wikiFromDb.Content?.Contains("Overview") ?? false,
                Visibility = wikiFromCache!.Visibility.ToString()
            },
            SubpagesCreation = new
            {
                Count = subpageIds.Count,
                AllExistInDb = subpagesFromDb.All(s => s != null),
                AllHaveHtmlContent = subpagesFromDb.All(s => s?.Content?.Contains("<h2>") ?? false)
            },
            ParentChildRelationships = new
            {
                WikiChildCount = wikiFromCache.ChildRelations.Count,
                AllSubpagesHaveOneParent = subpagesFromCache.All(s => s?.ParentRelations.Count == 1),
                FirstSubpageHasWikiAsAncestor = firstSubpageAncestors.Any(a => a.Id == wikiId)
            }
        });
    }
}
