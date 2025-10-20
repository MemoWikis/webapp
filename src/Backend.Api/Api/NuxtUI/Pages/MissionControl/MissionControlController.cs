public class MissionControlController(
    SessionUser _sessionUser,
    KnowledgeSummaryLoader _knowledgeSummaryLoader,
    IHttpContextAccessor _httpContextAccessor,
    PopularityCalculator _popularityCalculator)
    : ApiBaseController
{
    public readonly record struct Activity(DateTime Day, int Count);

    public readonly record struct ActivityCalendar(IList<Activity> Activity);

    public readonly record struct GetAllResponse(IList<PageItem> Wikis, IList<PageItem> Favorites, KnowledgeSummaryResponse KnowledgeStatus, ActivityCalendar ActivityCalendar);

    [HttpGet]
    public GetAllResponse GetAll()
    {
        if (_sessionUser == null || !_sessionUser.IsLoggedIn)
        {
            return new GetAllResponse(
                new List<PageItem>(),
                new List<PageItem>(),
                new KnowledgeSummaryResponse(),
                new ActivityCalendar(new List<Activity>())
            );
        }

        var knowledgeSummary = new KnowledgeSummaryResponse(_knowledgeSummaryLoader.Run(_sessionUser.UserId));

        return new GetAllResponse(
            GetWikis(),
            GetFavorites(),
            knowledgeSummary,
            GetActivityCalendar());
    }

    private IList<PageItem> GetWikis()
    {
        var userCacheItem = EntityCache.GetUserById(_sessionUser.UserId);

        var wikis = userCacheItem.GetWikis()
            .Select(wiki => new PageItem(
                wiki.Id,
                wiki.Name,
                new PageImageSettings(wiki.Id, _httpContextAccessor).GetUrl_128px(true).Url,
                wiki.GetCountQuestionsAggregated(_sessionUser.UserId),
                new KnowledgeSummaryResponse(_knowledgeSummaryLoader.Run(_sessionUser.UserId, wiki.Id, onlyInWishKnowledge: true)),
                _popularityCalculator.CalculatePagePopularity(wiki)))
            .ToList();

        return wikis;
    }

    private IList<PageItem> GetFavorites()
    {
        var userCacheItem = EntityCache.GetUserById(_sessionUser.UserId);

        var favorites = userCacheItem.GetFavorites()
            .Select(favorite => new PageItem(
                favorite.Id,
                favorite.Name,
                new PageImageSettings(favorite.Id, _httpContextAccessor).GetUrl_128px(true).Url,
                favorite.GetCountQuestionsAggregated(_sessionUser.UserId),
                new KnowledgeSummaryResponse(_knowledgeSummaryLoader.Run(_sessionUser.UserId, favorite.Id, onlyInWishKnowledge: true)),
                _popularityCalculator.CalculatePagePopularity(favorite)))
            .ToList();

        return favorites;
    }

    [HttpGet]
    public ActivityCalendar GetMockActivityCalendar() => GetActivityCalendar();

    // wip mockup data
    private static ActivityCalendar GetActivityCalendar()
    {
        var endDate = DateTime.Today;
        var startDate = endDate.AddDays(-364); // 365 days including today
        var random = new Random();
        var activity = new List<Activity>();

        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            int count;

            // Less activity on weekends
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                count = random.Next(0, 5);
            }
            // More activity on weekdays
            else
            {
                count = random.Next(1, 15);
            }

            // Occasional spikes of high activity
            if (random.NextDouble() < 0.05)
            {
                count = random.Next(15, 30);
            }

            activity.Add(new Activity(date, count));
        }

        return new ActivityCalendar(activity);
    }
}
