[TestFixture]
internal class EntityCacheInitializerMmap_tests : BaseTestHarness
{
    public record struct TestViews(DateTime DateTimeNow, int Count);

    [Test]
    public async Task PageViewMmapCache_ComprehensiveTest()
    {
        // Arrange
        await ClearData();

        var context = NewPageContext();
        var page = context.AddAndGet("Test Page");
        context.Persist();

        var yesterday = DateTime.UtcNow.Date.AddDays(-1);
        var twoDaysAgo = DateTime.UtcNow.Date.AddDays(-2);

        // Step 1: Test loading from database when no mmap cache exists
        var mmapCacheRefreshService = R<MmapCacheRefreshService>();
        mmapCacheRefreshService.DeleteAllCacheFiles();

        AddPageViewsToDatabase(page.Id,
            new[] { new TestViews(yesterday, 10), new TestViews(twoDaysAgo, 5) });

        var pageViewMapCache = R<PageViewMmapCache>();
        var cachedViewsStep1 = pageViewMapCache.LoadPageViews();
        var mmapCacheFileExistedBeforeInit = cachedViewsStep1.Count > 0;
        // Act - Initialize without existing mmap cache
        SimulateEntityCacheFirstStart("_Test_NoMmapCache");

        var cachedViewsStep2 = pageViewMapCache.LoadPageViews();
        var mmapCacheCreated = cachedViewsStep2.Count > 0;
        // Collect Step 1 results - capture TotalViews at this point
        var pageCacheItemStep1 = EntityCache.GetPage(page.Id);
        var step1TotalViews = pageCacheItemStep1?.TotalViews;


        // Step 2: Test loading from existing mmap cache
        // Add more views to database AFTER mmap cache creation
        var today = DateTime.Now.Date;
        AddPageViewsToDatabase(page.Id, new[] { new TestViews(today, 3) });

        // Clear entity cache to simulate restart
        EntityCache.Clear();
        EntityCache.IsFirstStart = true; // Simulate first start

        // Act - Initialize again (should load from mmap cache, missing today's views)
        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init("_Test_SecondInit");

        // Collect Step 2 results
        var pageCacheItemStep2 = EntityCache.GetPage(page.Id);
        var step2TotalViews = pageCacheItemStep2?.TotalViews;
        var allDbViewsStep2 = R<PageViewRepo>().GetAllEager();
        var totalDbViewsStep2 = allDbViewsStep2.Where(v => v.PageId == page.Id).Sum(v => v.Count);

        // Step 3: Test refresh functionality to update EntityCache with missing views
        // Add additional views to database (simulating more activity after last mmap refresh)
        AddPageViewsToDatabase(page.Id, new[]
        {
            new TestViews(yesterday, 2) // Additional views on existing date
        });

        // Step 4: Run the scheduled refresh (this should update EntityCache)
        mmapCacheRefreshService.Refresh();

        // Force the first refresh behavior to update EntityCache
        mmapCacheRefreshService.UpdateEntityCacheViewsFromMmap();

        // Collect final results
        var pageAfterRefresh = EntityCache.GetPage(page.Id);
        var finalTotalViews = pageAfterRefresh?.TotalViews;

        // Single comprehensive verification
        await Verify(new
        {
            Step1_LoadFromDatabase = new
            {
                TotalViews = step1TotalViews,
                MmapCacheFileExistedBeforeInit = mmapCacheFileExistedBeforeInit,
                MmmapcacheFileCreated = mmapCacheCreated
            },
            Step2_LoadFromMmapCache = new
            {
                TotalViews = step2TotalViews,
                TotalDbViews = totalDbViewsStep2
            },
            Step3_AfterRefresh = new
            {
                TotalViews = finalTotalViews
            }
        });
    }

    [Test]
    public async Task LoadQuestionViewsFromMmapOrDatabase_WithoutExistingMmapCache_ShouldLoadFromDatabase()
    {
        // Arrange
        await ClearData();

        var context = NewQuestionContext();
        var question = context.AddQuestion("Test Question", "Test Solution", 1, true, null, new List<Page>(), 50, true)
            .Persist()
            .All[0];

        var yesterday = DateTime.Now.Date.AddDays(-1);
        var twoDaysAgo = DateTime.Now.Date.AddDays(-2);

        AddQuestionViewsToDatabase(question.Id,
            new[] { new TestViews(yesterday, 8), new TestViews(twoDaysAgo, 4) });

        // Act
        var mmapCacheRefreshService = R<MmapCacheRefreshService>();
        mmapCacheRefreshService.DeleteAllCacheFiles();
        SimulateEntityCacheFirstStart("_Test_NoMmapCache_Questions");

        // Collect results
        var questionCacheItem = EntityCache.GetQuestion(question.Id);
        var cachedViews = R<QuestionViewMmapCache>().LoadQuestionViews();

        // Single verification
        await Verify(new
        {
            TotalViews = questionCacheItem?.TotalViews,
            MmapCacheCreated = cachedViews.Count > 0
        });
    }

    [Test]
    public async Task RefreshMmapCache_ShouldUpdateQuestionCacheWithMissingViews()
    {
        // Arrange
        await ClearData();

        var context = NewQuestionContext();
        var question = context.AddQuestion("Test Question", "Test Solution", 2, true, null, new List<Page>(), 50, true)
            .Persist()
            .All[0];

        var yesterday = DateTime.Now.Date.AddDays(-1);
        var twoDaysAgo = DateTime.Now.Date.AddDays(-2);

        // Initial setup
        AddQuestionViewsToDatabase(question.Id,
            new[] { new TestViews(yesterday, 8), new TestViews(twoDaysAgo, 4) });

        //Act - Create initial mmap cache
        var mmapCacheRefreshService = R<MmapCacheRefreshService>();
        mmapCacheRefreshService.DeleteAllCacheFiles();
        SimulateEntityCacheFirstStart("_Test_InitialSetup_Questions");

        // Collect initial state
        var initialQuestionCacheItem = EntityCache.GetQuestion(question.Id);
        var initialTotalViews = initialQuestionCacheItem?.TotalViews;

        // Add new views after mmap cache creation
        var today = DateTime.Now.Date;
        AddQuestionViewsToDatabase(question.Id,
            new[] { new TestViews(today, 6), new TestViews(yesterday, 2) });

        // Simulate server restart
        SimulateEntityCacheFirstStart("_Test_AfterRestart_Questions");

        // Collect state after restart
        var questionAfterRestart = EntityCache.GetQuestion(question.Id);
        var restartTotalViews = questionAfterRestart?.TotalViews;

        // Run refresh
        R<MmapCacheRefreshService>().Refresh();
        R<MmapCacheRefreshService>().UpdateEntityCacheViewsFromMmap();

        // Collect final state
        var questionAfterRefresh = EntityCache.GetQuestion(question.Id);
        var finalTotalViews = questionAfterRefresh?.TotalViews;

        // Single comprehensive verification
        await Verify(new
        {
            InitialSetup = new
            {
                TotalViews = initialTotalViews
            },
            AfterRestart = new
            {
                TotalViews = restartTotalViews
            },
            AfterRefresh = new
            {
                TotalViews = finalTotalViews
            }
        });
    }

    private void AddPageViewsToDatabase(int pageId, IEnumerable<TestViews> viewData)
    {
        var pageViewRepo = R<PageViewRepo>();

        foreach (var data in viewData)
        {
            for (int i = 0; i < data.Count; i++)
            {
                // Use direct SQL to bypass the ReadOnly mapping restriction for DateOnly
                var sql = @"
                    INSERT INTO pageview (Page_id, User_id, UserAgent, DateCreated, DateOnly) 
                    VALUES (:pageId, :userId, :userAgent, :dateCreated, :dateOnly)";

                pageViewRepo.Session.CreateSQLQuery(sql)
                    .SetParameter("pageId", pageId)
                    .SetParameter("userId", 1)
                    .SetParameter("userAgent", "pageView test")
                    .SetParameter("dateCreated", data.DateTimeNow)
                    .SetParameter("dateOnly", data.DateTimeNow.Date)
                    .ExecuteUpdate();
            }
        }

        pageViewRepo.Flush();
    }

    private void AddQuestionViewsToDatabase(int questionId, IEnumerable<TestViews> viewData)
    {
        var questionViewRepo = R<QuestionViewRepository>();

        foreach (var data in viewData)
        {
            for (int i = 0; i < data.Count; i++)
            {
                // Use direct SQL to bypass the ReadOnly mapping restriction for DateOnly
                var sql = @"
                    INSERT INTO questionview (QuestionId, UserId, DateCreated, DateOnly) 
                    VALUES (:questionId, :userId, :dateCreated, :dateOnly)";

                questionViewRepo.Session.CreateSQLQuery(sql)
                    .SetParameter("questionId", questionId)
                    .SetParameter("userId", 1)
                    .SetParameter("dateCreated", data.DateTimeNow)
                    .SetParameter("dateOnly", data.DateTimeNow.Date)
                    .ExecuteUpdate();
            }
        }

        questionViewRepo.Flush();
    }

    public void SimulateEntityCacheFirstStart(string initMsg = "")
    {
        EntityCache.Clear();
        EntityCache.IsFirstStart = true;
        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init(initMsg);
    }
}