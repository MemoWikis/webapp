public class MissionControlController(
    SessionUser _sessionUser,
    KnowledgeSummaryLoader _knowledgeSummaryLoader,
    IHttpContextAccessor _httpContextAccessor,
    PopularityCalculator _popularityCalculator,
    ActivityCalendarLoader _activityCalendarLoader)
    : ApiBaseController
{
    public readonly record struct GetAllResponse(IList<PageItem> Wikis, IList<PageItem> Favorites, KnowledgeSummaryResponse KnowledgeStatus, ActivityCalendarLoader.ActivityCalendar ActivityCalendar);

    [HttpGet]
    public GetAllResponse GetAll()
    {
        if (_sessionUser == null || !_sessionUser.IsLoggedIn)
        {
            return new GetAllResponse(
                new List<PageItem>(),
                new List<PageItem>(),
                new KnowledgeSummaryResponse(),
                new ActivityCalendarLoader.ActivityCalendar(new List<ActivityCalendarLoader.Activity>())
            );
        }

        var knowledgeSummary = new KnowledgeSummaryResponse(_knowledgeSummaryLoader.Run(_sessionUser.UserId));

        return new GetAllResponse(
            GetWikis(),
            GetFavorites(),
            knowledgeSummary,
            _activityCalendarLoader.GetForUser(_sessionUser.UserId));
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
    public ActivityCalendarLoader.ActivityCalendar GetActivityCalendar()
    {
        if (_sessionUser == null || !_sessionUser.IsLoggedIn)
        {
            return new ActivityCalendarLoader.ActivityCalendar(new List<ActivityCalendarLoader.Activity>());
        }

        return _activityCalendarLoader.GetForUser(_sessionUser.UserId);
    }

    [HttpGet]
    public ActivityCalendarLoader.ActivityCalendar GetPageActivityCalendar([FromRoute] int id)
    {
        if (_sessionUser == null || !_sessionUser.IsLoggedIn)
        {
            return new ActivityCalendarLoader.ActivityCalendar(new List<ActivityCalendarLoader.Activity>());
        }

        return _activityCalendarLoader.GetForUser(_sessionUser.UserId, id);
    }
}
