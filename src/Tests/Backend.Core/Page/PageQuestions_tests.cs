class PageQuestions_tests : BaseTestHarness
{
    [Test]
    public async Task Should_Get_Direct_Questions()
    {
        // Arrange
        await ClearData();

        var pageContext = NewPageContext();
        var questionContext = NewQuestionContext();

        // Create a page hierarchy
        var rootPage = pageContext.Add("Root Page").Persist().All.First();
        pageContext.Add("Child Page 1").Persist();
        var childPage1 = pageContext.GetPageByName("Child Page 1");
        pageContext.Add("Child Page 2").Persist();
        var childPage2 = pageContext.GetPageByName("Child Page 2");

        pageContext.AddChild(rootPage, childPage1);
        pageContext.AddChild(rootPage, childPage2);

        // Create questions attached to these pages
        var rootQuestion = questionContext.AddQuestion(
            questionText: "Root Question",
            solutionText: "Root Answer",
            pages: new List<Page> { rootPage },
            questionVisibility: QuestionVisibility.Public
        );

        var child1Question1 = questionContext.AddQuestion(
            questionText: "Child1 Question 1",
            solutionText: "Child1 Answer 1",
            pages: new List<Page> { childPage1 },
            questionVisibility: QuestionVisibility.Public
        );

        var child1Question2 = questionContext.AddQuestion(
            questionText: "Child1 Question 2",
            solutionText: "Child1 Answer 2",
            pages: new List<Page> { childPage1 },
            questionVisibility: QuestionVisibility.Public
        );

        var child2Question = questionContext.AddQuestion(
            questionText: "Child2 Question",
            solutionText: "Child2 Answer",
            pages: new List<Page> { childPage2 },
            questionVisibility: QuestionVisibility.Public
        );

        questionContext.Persist();

        await ReloadCaches();

        // Act - Get only direct questions (no child pages)
        var rootCacheItem = EntityCache.GetPage(rootPage);
        var questions = EntityCache.GetAllQuestions();
        var pages = EntityCache.GetAllPagesList();
        var userId = 1; // Standard test user

        var directQuestionsOnly = rootCacheItem!.GetAggregatedQuestions(
            userId,
            fullList: false,
            pageId: rootPage.Id
        );

        // Get questions including child pages
        var allQuestionsIncludingChildren = rootCacheItem!.GetAggregatedQuestions(
            userId,
            fullList: true
        );

        // Get questions from child1 only
        var child1CacheItem = EntityCache.GetPage(childPage1);
        var child1QuestionsOnly = child1CacheItem!.GetAggregatedQuestions(
            userId,
            fullList: false,
            pageId: childPage1.Id
        );

        // Assert
        await Verify(new
        {
            DirectQuestionsCount = directQuestionsOnly.Count,
            DirectQuestionTexts = directQuestionsOnly.Select(q => q.Text).ToList(),

            AggregatedQuestionsCount = allQuestionsIncludingChildren.Count,
            AggregatedQuestionTexts = allQuestionsIncludingChildren.Select(q => q.Text).ToList(),

            Child1QuestionsCount = child1QuestionsOnly.Count,
            Child1QuestionTexts = child1QuestionsOnly.Select(q => q.Text).ToList(),

            // Verify the page relationships are correct
            RootPageHasChildren = rootCacheItem!.ChildRelations?.Count > 0,
            Child1IsChildOfRoot = rootCacheItem!.ChildRelations?.Any(r => r.ChildId == childPage1.Id) ?? false,
            Child2IsChildOfRoot = rootCacheItem!.ChildRelations?.Any(r => r.ChildId == childPage2.Id) ?? false
        });
    }

    [Test]
    public async Task Should_Filter_Questions_By_Visibility()
    {
        // Arrange
        await ClearData();

        var pageContext = NewPageContext();
        var questionContext = NewQuestionContext();

        // Create page and users
        var page = pageContext.Add("Test Page").Persist().All.First();
        var creator = questionContext.Creator; // Default creator
        var otherUser = new User { Id = 999, Name = "Other User" };

        // Create questions with different visibilities
        var publicQuestion = questionContext.AddQuestion(
            questionText: "Public Question",
            pages: new List<Page> { page },
            questionVisibility: QuestionVisibility.Public
        );

        var privateQuestion = questionContext.AddQuestion(
            questionText: "Private Question",
            pages: new List<Page> { page },
            creator: creator,
            questionVisibility: QuestionVisibility.Private
        );

        questionContext.Persist();

        await ReloadCaches();

        // Act
        var pageCacheItem = EntityCache.GetPage(page);

        // Creator should see both questions
        var creatorQuestions = pageCacheItem!.GetAggregatedQuestions(
            creator.Id,
            onlyVisible: true,
            fullList: false,
            pageId: page.Id
        );

        // Other user should only see public questions
        var otherUserQuestions = pageCacheItem!.GetAggregatedQuestions(
            otherUser.Id,
            onlyVisible: true,
            fullList: false,
            pageId: page.Id
        );

        // Get all questions regardless of visibility
        var allQuestions = pageCacheItem!.GetAggregatedQuestions(
            otherUser.Id,
            onlyVisible: false,
            fullList: false,
            pageId: page.Id
        );

        // Assert
        await Verify(new
        {
            CreatorQuestionsCount = creatorQuestions.Count,
            CreatorQuestionsTexts = creatorQuestions.Select(q => q.Text).ToList(),

            OtherUserQuestionsCount = otherUserQuestions.Count,
            OtherUserQuestionsTexts = otherUserQuestions.Select(q => q.Text).ToList(),

            AllQuestionsCount = allQuestions.Count,
            AllQuestionsTexts = allQuestions.Select(q => q.Text).ToList()
        });
    }
}