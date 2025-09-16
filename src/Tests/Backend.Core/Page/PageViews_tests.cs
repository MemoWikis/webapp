
internal class PageViews_tests : BaseTestHarness
{
    [Test]
    public async Task Should_Correctly_Count_Page_Views()
    {
        //Arrange
        await ClearData();

        var context = NewPageContext();
        var page = context.Add("Test Page").Persist().All.First();
        var pageCacheItem = PageCacheItem.ToCachePage(page);

        // Create view data with specific counts
        var yesterday = DateTime.Now.Date.AddDays(-1);
        var twoDaysAgo = DateTime.Now.Date.AddDays(-2);

        var views = new List<PageViewSummaryWithId>
        {
            new() { PageId = page.Id, DateOnly = yesterday, Count = 10 },
            new() { PageId = page.Id, DateOnly = twoDaysAgo, Count = 5 }
        };

        //Act
        PageCacheItem.SetPageViews(pageCacheItem, views);

        //Assert
        await Verify(new
        {
            TotalViews = pageCacheItem.TotalViews,
            ExpectedSum = 15,
            ViewsCount = views.Count,
            DailyViewsCount = pageCacheItem.ViewsOfPast90Days?.Count,
            ViewsOfPast90Days = pageCacheItem.ViewsOfPast90Days?.Sum(views => views.Count),
            ExpectedViewsOfPast90Days = 15,
            HasYesterday = pageCacheItem.ViewsOfPast90Days?.Any(v => v.Date == yesterday),
            YesterdayCount = pageCacheItem.ViewsOfPast90Days?.FirstOrDefault(v => v.Date == yesterday)?.Count,
            HasTwoDaysAgo = pageCacheItem.ViewsOfPast90Days?.Any(v => v.Date == twoDaysAgo),
            TwoDaysAgoCount = pageCacheItem.ViewsOfPast90Days?.FirstOrDefault(v => v.Date == twoDaysAgo)?.Count
        });
    }

    [Test]
    public async Task Should_Correctly_Count_Page_Views_Including_Old_Views()
    {
        // Arrange
        await ClearData();

        var context = NewPageContext();
        var page = context.Add("Test Page").Persist().All.First();
        var pageCacheItem = PageCacheItem.ToCachePage(page);

        // Create view data with specific counts - including views older than 90 days
        var yesterday = DateTime.Now.Date.AddDays(-1);
        var twoDaysAgo = DateTime.Now.Date.AddDays(-2);
        var outsideRange = DateTime.Now.Date.AddDays(-95); // This is outside the 90-day window

        var views = new List<PageViewSummaryWithId>
        {
            new() { PageId = page.Id, DateOnly = yesterday, Count = 10 },
            new() { PageId = page.Id, DateOnly = twoDaysAgo, Count = 5 },
            new() { PageId = page.Id, DateOnly = outsideRange, Count = 13 } // Old views
        };

        // Act
        PageCacheItem.SetPageViews(pageCacheItem, views);

        // Assert
        await Verify(new
        {
            TotalViews = pageCacheItem.TotalViews,
            ExpectedTotalViews = 28, // 10 + 5 + 13
            ViewsCount = views.Count,
            DailyViewsCount = pageCacheItem.ViewsOfPast90Days?.Count,
            ViewsOfPast90Days = pageCacheItem.ViewsOfPast90Days?.Sum(view => view.Count),
            ExpectedViewsOfPast90Days = 15, // Only 10 + 5 (within 90 days)
            HasOutsideRangeViews = pageCacheItem.ViewsOfPast90Days?.Any(v => v.Date == outsideRange),
            HasYesterday = pageCacheItem.ViewsOfPast90Days?.Any(v => v.Date == yesterday),
            YesterdayCount = pageCacheItem.ViewsOfPast90Days?.FirstOrDefault(v => v.Date == yesterday)?.Count,
            HasTwoDaysAgo = pageCacheItem.ViewsOfPast90Days?.Any(v => v.Date == twoDaysAgo),
            TwoDaysAgoCount = pageCacheItem.ViewsOfPast90Days?.FirstOrDefault(v => v.Date == twoDaysAgo)?.Count,
            DoesNotHave95DaysAgo = pageCacheItem.ViewsOfPast90Days?.All(v => v.Date != outsideRange)
        });
    }
}

