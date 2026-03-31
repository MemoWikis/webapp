using Microsoft.AspNetCore.Http;

public class WikiListService(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    IHttpContextAccessor _httpContextAccessor,
    PopularityCalculator _popularityCalculator) : IRegisterAsInstancePerLifetime
{
    public readonly record struct WikiListItem(
        int Id,
        string Name,
        string ImgUrl,
        int QuestionCount,
        int ChildPageCount,
        int Popularity,
        string CreatorName,
        int CreatorId);

    public readonly record struct WikisResult(
        IList<WikiListItem> Wikis,
        int TotalCount);

    public WikisResult GetPublicWikis(int page = 1, int pageSize = 20)
    {
        var allPublicWikis = PublicWikiCache.GetAllPublicWikis()
            .Where(p => _permissionCheck.CanView(p))
            .ToList();

        var totalCount = allPublicWikis.Count;

        var wikis = allPublicWikis
            .OrderByDescending(w => _popularityCalculator.CalculatePagePopularity(w))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(w => new WikiListItem(
                w.Id,
                w.Name,
                new PageImageSettings(w.Id, _httpContextAccessor).GetUrl_128px(true).Url,
                w.GetCountQuestionsAggregated(_sessionUser.UserId),
                w.VisibleChildrenCount(_permissionCheck, _sessionUser.UserId),
                _popularityCalculator.CalculatePagePopularity(w),
                w.Creator?.Name ?? "",
                w.CreatorId))
            .ToList();

        return new WikisResult(wikis, totalCount);
    }
}
