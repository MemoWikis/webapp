public class MissionControlController(
    SessionUser _sessionUser,
    KnowledgeSummaryLoader _knowledgeSummaryLoader,
    IHttpContextAccessor _httpContextAccessor)
    : ApiBaseController
{
    public readonly record struct WikiItem(int Id, string Name, string ImgUrl, int? QuestionCount, KnowledgeSummaryResponse KnowledgebarData);

    public readonly record struct Activity(DateTime Day, int Count);

    public readonly record struct ActivityCalendar(IList<Activity> Activity);

    public readonly record struct GetAllResponse(IList<WikiItem> Wikis, KnowledgeSummaryResponse KnowledgeStatus, ActivityCalendar ActivityCalendar);

    public readonly record struct KnowledgeSummaryResponse(
        int NotLearned,
        int NotLearnedPercentage,
        int NeedsLearning,
        int NeedsLearningPercentage,
        int NeedsConsolidation,
        int NeedsConsolidationPercentage,
        int Solid,
        int SolidPercentage,
        int NotInWishknowledge,
        int NotInWishknowledgePercentage,
        int Total);

    [HttpGet]
    public GetAllResponse GetAll()
    {
        if (_sessionUser == null || !_sessionUser.IsLoggedIn)
        {
            return new GetAllResponse(new List<WikiItem>(), new KnowledgeSummaryResponse(), new ActivityCalendar(new List<Activity>()));
        }

        var knowledgeSummary = FillKnowledgeSummaryResponse(_knowledgeSummaryLoader.Run(_sessionUser.UserId));

        return new GetAllResponse(
            GetWikis(),
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
            knowledgeSummary.Total);
    }

    // wip needs to be moved to a service
    private IList<WikiItem> GetWikis()
    {
        var userCacheItem = EntityCache.GetUserById(_sessionUser.UserId);
        userCacheItem.CleanupWikiIdsAndFavoriteIds();

        var wikis = userCacheItem.Wikis
            .Where(page => page != null && page.IsWiki)
            .Select(wiki => new WikiItem(
                wiki.Id,
                wiki.Name,
                new PageImageSettings(wiki.Id, _httpContextAccessor).GetUrl_128px(true).Url,
                wiki.GetCountQuestionsAggregated(_sessionUser.UserId),
                FillKnowledgeSummaryResponse(_knowledgeSummaryLoader.Run(_sessionUser.UserId, wiki.Id))))
            .ToList();

        var userStartPage = EntityCache.GetPage(userCacheItem.StartPageId);
        var userStartPageAsWikiItem = new WikiItem(
            userStartPage.Id,
            userStartPage.Name,
            new PageImageSettings(userStartPage.Id, _httpContextAccessor).GetUrl_128px(true).Url,
            userStartPage.GetCountQuestionsAggregated(_sessionUser.UserId),
            FillKnowledgeSummaryResponse(_knowledgeSummaryLoader.Run(_sessionUser.UserId, userStartPage.Id))
        );

        wikis.Insert(0, userStartPageAsWikiItem);

        return wikis;
    }

    // wip mockup data
    private ActivityCalendar GetActivityCalendar()
    {
        var currentYear = DateTime.Now.Year;
        var startDate = new DateTime(currentYear, 1, 1);
        var endDate = new DateTime(currentYear, 12, 31);
        var random = new Random();
        var activity = new List<Activity>();

        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            int count;

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                count = random.Next(0, 5);
            }
            else
            {
                count = random.Next(1, 15);
            }

            if (random.NextDouble() < 0.05)
            {
                count = random.Next(15, 30);
            }

            activity.Add(new Activity(date, count));
        }

        return new ActivityCalendar(activity);
    }
}
