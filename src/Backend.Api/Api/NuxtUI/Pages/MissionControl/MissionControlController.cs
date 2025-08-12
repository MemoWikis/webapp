public class MissionControlController(
    SessionUser _sessionUser,
    KnowledgeSummaryLoader _knowledgeSummaryLoader,
    IHttpContextAccessor _httpContextAccessor)
    : ApiBaseController
{
    public readonly record struct PageItem(int Id, string Name, string ImgUrl, int? QuestionCount, KnowledgeSummaryResponse KnowledgebarData);

    public readonly record struct Activity(DateTime Day, int Count);

    public readonly record struct ActivityCalendar(IList<Activity> Activity);

    public readonly record struct GetAllResponse(IList<PageItem> Wikis, IList<PageItem> Favorites, KnowledgeSummaryResponse KnowledgeStatus, ActivityCalendar ActivityCalendar);

    public readonly record struct KnowledgeSummaryResponse(
        int NotLearned = 0,
        int NotLearnedPercentage = 0,
        int NeedsLearning = 0,
        int NeedsLearningPercentage = 0,
        int NeedsConsolidation = 0,
        int NeedsConsolidationPercentage = 0,
        int Solid = 0,
        int SolidPercentage = 0,
        int NotInWishknowledge = 0,
        int NotInWishknowledgePercentage = 0,
        int Total = 0,
        double KnowledgeStatusPoints = 0.0,
        double KnowledgeStatusPointsTotal = 0.0);

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

        var knowledgeSummary = FillKnowledgeSummaryResponse(_knowledgeSummaryLoader.Run(_sessionUser.UserId));

        return new GetAllResponse(
            GetWikis(),
            GetFavorites(),
            knowledgeSummary,
            GetActivityCalendar());
    }

    // wip because KnowledgeSummary is incomplete as response
    private KnowledgeSummaryResponse FillKnowledgeSummaryResponse(KnowledgeSummary knowledgeSummary)
    {
        return new KnowledgeSummaryResponse(
            knowledgeSummary.NotLearned,
            knowledgeSummary.NotLearnedPercentage,
            knowledgeSummary.NeedsLearning,
            knowledgeSummary.NeedsLearningPercentage,
            knowledgeSummary.NeedsConsolidation,
            knowledgeSummary.NeedsConsolidationPercentage,
            knowledgeSummary.Solid,
            knowledgeSummary.SolidPercentage,
            knowledgeSummary.NotInWishknowledge,
            knowledgeSummary.NotInWishknowledgePercentage,
            knowledgeSummary.Total,
            knowledgeSummary.KnowledgeStatusPoints,
            knowledgeSummary.KnowledgeStatusPointsTotal);
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
                FillKnowledgeSummaryResponse(_knowledgeSummaryLoader.Run(_sessionUser.UserId, wiki.Id))))
            .ToList();

        return wikis;
    }

    private IList<PageItem> GetFavorites()
    {
        var userCacheItem = EntityCache.GetUserById(_sessionUser.UserId);

        var favorites = userCacheItem.GetFavorites()
            .Select(wiki => new PageItem(
                wiki.Id,
                wiki.Name,
                new PageImageSettings(wiki.Id, _httpContextAccessor).GetUrl_128px(true).Url,
                wiki.GetCountQuestionsAggregated(_sessionUser.UserId),
                FillKnowledgeSummaryResponse(_knowledgeSummaryLoader.Run(_sessionUser.UserId, wiki.Id))))
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
