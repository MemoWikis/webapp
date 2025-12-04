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

    [Test]
    public void GetWikiWithSubpagesPrompt_should_contain_required_instructions()
    {
        // Act
        var prompt = AiPageGenerator.GetWikiWithSubpagesPrompt(
            "Create a comprehensive wiki about World War II",
            AiPageGenerator.DifficultyLevel.Intermediate);

        // Assert
        Assert.That(prompt, Does.Contain("Wiki"), "Prompt should mention Wiki");
        Assert.That(prompt, Does.Contain("Unterseiten").Or.Contain("Subpages"), "Prompt should mention subpages");
        Assert.That(prompt, Does.Contain("3-7"), "Prompt should specify subpage count range");
        Assert.That(prompt, Does.Contain("JSON"), "Prompt should mention JSON format");
        Assert.That(prompt, Does.Contain("Title"), "Prompt should mention Title property");
        Assert.That(prompt, Does.Contain("HtmlContent"), "Prompt should mention HtmlContent property");
        Assert.That(prompt, Does.Contain("Subpages"), "Prompt should mention Subpages array");
    }

    [Test]
    public void GetWikiWithSubpagesFromUrlPrompt_should_contain_source_url()
    {
        // Arrange
        var sourceUrl = "https://example.com/article";
        
        // Act
        var prompt = AiPageGenerator.GetWikiWithSubpagesFromUrlPrompt(
            "Test Title",
            "Test content about the topic",
            sourceUrl,
            AiPageGenerator.DifficultyLevel.Intermediate);

        // Assert
        Assert.That(prompt, Does.Contain(sourceUrl), "Prompt should contain the source URL");
        Assert.That(prompt, Does.Contain("Wiki"), "Prompt should mention Wiki");
        Assert.That(prompt, Does.Contain("Unterseiten").Or.Contain("Subpages"), "Prompt should mention subpages");
    }

    [Test]
    public void GeneratedWiki_record_should_have_correct_structure()
    {
        // Arrange & Act
        var subpages = new List<AiPageGenerator.GeneratedSubpage>
        {
            new("Subpage 1", "<p>Content 1</p>"),
            new("Subpage 2", "<p>Content 2</p>")
        };
        var wiki = new AiPageGenerator.GeneratedWiki("Main Wiki", "<p>Overview</p>", subpages);

        // Assert
        Assert.That(wiki.Title, Is.EqualTo("Main Wiki"));
        Assert.That(wiki.HtmlContent, Is.EqualTo("<p>Overview</p>"));
        Assert.That(wiki.Subpages, Has.Count.EqualTo(2));
        Assert.That(wiki.Subpages[0].Title, Is.EqualTo("Subpage 1"));
        Assert.That(wiki.Subpages[1].Title, Is.EqualTo("Subpage 2"));
    }

    [Test]
    public void GeneratedWiki_should_be_deserializable_from_json()
    {
        // Arrange
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

        // Act
        var wiki = JsonSerializer.Deserialize<AiPageGenerator.GeneratedWiki>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Assert
        Assert.That(wiki.Title, Is.EqualTo("Test Wiki"));
        Assert.That(wiki.HtmlContent, Does.Contain("Overview"));
        Assert.That(wiki.Subpages, Has.Count.EqualTo(2));
        Assert.That(wiki.Subpages[0].Title, Is.EqualTo("First Subpage"));
        Assert.That(wiki.Subpages[1].Title, Is.EqualTo("Second Subpage"));
    }

    [Test]
    public void Prompt_should_adjust_for_difficulty_level()
    {
        // Act
        var eli5Prompt = AiPageGenerator.GetWikiWithSubpagesPrompt("Topic", AiPageGenerator.DifficultyLevel.ELI5);
        var academicPrompt = AiPageGenerator.GetWikiWithSubpagesPrompt("Topic", AiPageGenerator.DifficultyLevel.Academic);

        // Assert
        Assert.That(eli5Prompt, Does.Contain("5-jähriges Kind").Or.Contain("ELI5"), "ELI5 prompt should reference simple explanation");
        Assert.That(academicPrompt, Does.Contain("akademisch").Or.Contain("academic"), "Academic prompt should reference academic level");
    }

    [Test]
    public async Task CreateWiki_should_create_wiki_and_subpages_in_database()
    {
        // Arrange
        var context = NewPageContext();
        var sessionUser = R<SessionUser>();
        var pageViewRepo = R<PageViewRepo>();
        var pageCreator = R<PageCreator>();
        var pageRepository = R<PageRepository>();

        var sessionUserDb = _testHarness.GetDefaultSessionUserFromDb();

        var parentName = "ParentWiki";
        var parent = context
            .Add(parentName, creator: sessionUserDb, isWiki: true)
            .Persist().All
            .Single(page => page.Name.Equals(parentName));

        sessionUser.Login(sessionUserDb, pageViewRepo);

        var wikiTitle = "Test AI Wiki";
        var wikiContent = "<h2>Overview</h2><p>This is the main wiki content.</p>";
        var subpages = new List<(string Title, string HtmlContent)>
        {
            ("Subpage One", "<h2>First Topic</h2><p>Content for first subpage.</p>"),
            ("Subpage Two", "<h2>Second Topic</h2><p>Content for second subpage.</p>"),
            ("Subpage Three", "<h2>Third Topic</h2><p>Content for third subpage.</p>")
        };

        // Act - Create wiki
        var wikiResult = pageCreator.Create(wikiTitle, parent.Id, sessionUser);
        Assert.That(wikiResult.Success, Is.True, "Wiki creation should succeed");

        var wikiId = wikiResult.Data.Id;
        var wikiCacheItem = EntityCache.GetPage(wikiId);
        var wikiPage = pageRepository.GetByIdEager(wikiId);

        wikiCacheItem!.Content = wikiContent;
        wikiCacheItem.IsWiki = true;
        wikiPage!.Content = wikiContent;
        wikiPage.IsWiki = true;

        EntityCache.AddOrUpdate(wikiCacheItem);
        pageRepository.Update(wikiPage, sessionUser.UserId, type: PageChangeType.Text);

        // Create subpages
        var subpageIds = new List<int>();
        foreach (var (subpageTitle, subpageContent) in subpages)
        {
            var subpageResult = pageCreator.Create(subpageTitle, wikiId, sessionUser);
            Assert.That(subpageResult.Success, Is.True, $"Subpage '{subpageTitle}' creation should succeed");

            var subpageId = subpageResult.Data.Id;
            var subpageCacheItem = EntityCache.GetPage(subpageId);
            var subpagePage = pageRepository.GetByIdEager(subpageId);

            subpageCacheItem!.Content = subpageContent;
            subpagePage!.Content = subpageContent;

            EntityCache.AddOrUpdate(subpageCacheItem);
            pageRepository.Update(subpagePage, sessionUser.UserId, type: PageChangeType.Text);

            subpageIds.Add(subpageId);
        }

        // Assert
        await ReloadCaches();

        // Verify wiki
        var wikiFromDb = pageRepository.GetByIdEager(wikiId);
        Assert.That(wikiFromDb, Is.Not.Null);
        Assert.That(wikiFromDb!.Name, Is.EqualTo(wikiTitle));
        Assert.That(wikiFromDb.IsWiki, Is.True);
        Assert.That(wikiFromDb.Content, Does.Contain("Overview"));

        // Verify subpages exist in database
        Assert.That(subpageIds, Has.Count.EqualTo(3));
        foreach (var subpageId in subpageIds)
        {
            var subpageFromDb = pageRepository.GetByIdEager(subpageId);
            Assert.That(subpageFromDb, Is.Not.Null, $"Subpage {subpageId} should exist in database");
            Assert.That(subpageFromDb!.Content, Does.Contain("<h2>"), $"Subpage {subpageId} should have HTML content");
        }
    }

    [Test]
    public async Task CreateWiki_should_create_parent_child_relationships()
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
        var wikiResult = pageCreator.Create(wikiTitle, rootWiki.Id, sessionUser);
        Assert.That(wikiResult.Success, Is.True);

        var wikiId = wikiResult.Data.Id;
        var wikiCacheItem = EntityCache.GetPage(wikiId);
        wikiCacheItem!.IsWiki = true;
        EntityCache.AddOrUpdate(wikiCacheItem);

        // Create subpages as children of the wiki
        var subpageResult1 = pageCreator.Create("Subpage Alpha", wikiId, sessionUser);
        var subpageResult2 = pageCreator.Create("Subpage Beta", wikiId, sessionUser);

        Assert.That(subpageResult1.Success, Is.True);
        Assert.That(subpageResult2.Success, Is.True);

        // Assert
        await ReloadCaches();

        // Verify parent-child relationships via EntityCache
        var wikiFromCache = EntityCache.GetPage(wikiId);
        Assert.That(wikiFromCache, Is.Not.Null);
        Assert.That(wikiFromCache!.IsWiki, Is.True);
        Assert.That(wikiFromCache.ChildRelations, Has.Count.EqualTo(2), "Wiki should have 2 child pages");

        // Verify subpages have the wiki as parent
        var subpage1FromCache = EntityCache.GetPage(subpageResult1.Data.Id);
        var subpage2FromCache = EntityCache.GetPage(subpageResult2.Data.Id);

        Assert.That(subpage1FromCache!.ParentRelations, Has.Count.EqualTo(1));
        Assert.That(subpage2FromCache!.ParentRelations, Has.Count.EqualTo(1));

        // Verify ancestors
        var ancestors1 = GraphService.VisibleAscendants(subpage1FromCache.Id, permissionCheck);
        var ancestors2 = GraphService.VisibleAscendants(subpage2FromCache.Id, permissionCheck);

        Assert.That(ancestors1.Any(a => a.Id == wikiId), Is.True, "Subpage should have wiki as ancestor");
        Assert.That(ancestors2.Any(a => a.Id == wikiId), Is.True, "Subpage should have wiki as ancestor");
    }

    [Test]
    public async Task CreateWiki_subpages_should_inherit_wiki_visibility()
    {
        // Arrange
        var context = NewPageContext();
        var sessionUser = R<SessionUser>();
        var pageViewRepo = R<PageViewRepo>();
        var pageCreator = R<PageCreator>();
        var pageRepository = R<PageRepository>();

        var sessionUserDb = _testHarness.GetDefaultSessionUserFromDb();

        var rootWikiName = "PublicRoot";
        var rootWiki = context
            .Add(rootWikiName, creator: sessionUserDb, isWiki: true)
            .Persist().All
            .Single(page => page.Name.Equals(rootWikiName));

        sessionUser.Login(sessionUserDb, pageViewRepo);

        // Act
        var wikiResult = pageCreator.Create("Generated Wiki", rootWiki.Id, sessionUser);
        Assert.That(wikiResult.Success, Is.True);

        var wikiId = wikiResult.Data.Id;

        // Create subpages
        var subpageResult = pageCreator.Create("Generated Subpage", wikiId, sessionUser);
        Assert.That(subpageResult.Success, Is.True);

        // Assert
        await ReloadCaches();

        var wikiFromCache = EntityCache.GetPage(wikiId);
        var subpageFromCache = EntityCache.GetPage(subpageResult.Data.Id);

        Assert.That(wikiFromCache, Is.Not.Null);
        Assert.That(subpageFromCache, Is.Not.Null);

        // Both wiki and subpage should be visible (not private by default)
        Assert.That(wikiFromCache!.Visibility, Is.EqualTo(PageVisibility.Public));
        Assert.That(subpageFromCache!.Visibility, Is.EqualTo(PageVisibility.Public));
    }
}
