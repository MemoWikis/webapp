using JetBrains.Annotations;

public readonly record struct UserPageItem(
    int Id,
    string Name,
    string ImgUrl,
    int? QuestionCount,
    KnowledgeSummaryResponse KnowledgebarData,
    int Popularity,
    [CanBeNull] string CreatorName = "",
    [CanBeNull] bool IsPublic = false);

public class UserPageItemMapper(
    SessionUser sessionUser,
    IHttpContextAccessor httpContextAccessor,
    KnowledgeSummaryLoader knowledgeSummaryLoader,
    PopularityCalculator popularityCalculator,
    PermissionCheck permissionCheck,
    ExtendedUserCache extendedUserCache) : IRegisterAsInstancePerLifetime
{
    public IList<UserPageItem> MapWikis(IEnumerable<PageCacheItem> wikis)
    {
        return wikis.Select(CreateUserPageItemFromWiki).ToList();
    }

    private UserPageItem CreateUserPageItemFromWiki(PageCacheItem wiki)
    {
        return new UserPageItem(
            wiki.Id,
            wiki.Name,
            new PageImageSettings(wiki.Id, httpContextAccessor).GetUrl_128px(true).Url,
            wiki.GetCountQuestionsAggregated(sessionUser.UserId),
            new KnowledgeSummaryResponse(knowledgeSummaryLoader.Run(sessionUser.UserId, wiki.Id, onlyInWishKnowledge: true)),
            popularityCalculator.CalculatePagePopularity(wiki));
    }

    public IList<UserPageItem> MapPages(
        int userId,
        IEnumerable<PageCacheItem> userWikis)
    {
        var user = EntityCache.GetUserById(userId);
        var userWikiIds = userWikis.Select(w => w.Id).ToHashSet();
        
        var userWikiDescendantIds = GetUserWikiDescendantIds(userWikis);
        var publicPagesCreatedByUser = GetPublicPagesCreatedByUser(userId, userWikiIds, userWikiDescendantIds);
        var filteredPages = FilterOutChildPagesWithParentsInList(publicPagesCreatedByUser, userId);

        return filteredPages.Select(CreateUserPageItemFromPage).ToList();
    }

    private HashSet<int> GetUserWikiDescendantIds(IEnumerable<PageCacheItem> userWikis)
    {
        var userWikiDescendantIds = new HashSet<int>();
        foreach (var wiki in userWikis)
        {
            var descendants = GraphService.Descendants(wiki.Id);
            foreach (var descendant in descendants)
            {
                userWikiDescendantIds.Add(descendant.Id);
            }
        }
        return userWikiDescendantIds;
    }

    private List<PageCacheItem> GetPublicPagesCreatedByUser(int userId, HashSet<int> userWikiIds, HashSet<int> userWikiDescendantIds)
    {
        return EntityCache.GetAllPagesList()
            .Where(page => page.Creator != null 
                       && page.CreatorId == userId 
                       && page.Visibility == PageVisibility.Public
                       && !userWikiIds.Contains(page.Id)
                       && !userWikiDescendantIds.Contains(page.Id))
            .Where(permissionCheck.CanView)
            .ToList();
    }

    private List<PageCacheItem> FilterOutChildPagesWithParentsInList(List<PageCacheItem> pages, int userId)
    {
        var pageIds = pages.Select(p => p.Id).ToHashSet();
        return pages
            .Where(page => 
            {
                var ascendants = GraphService.Ascendants(page.Id);
                return !ascendants.Any(ascendant => 
                    ascendant.CreatorId == userId && pageIds.Contains(ascendant.Id));
            })
            .ToList();
    }

    private UserPageItem CreateUserPageItemFromPage(PageCacheItem page)
    {
        return new UserPageItem(
            page.Id,
            page.Name,
            new PageImageSettings(page.Id, httpContextAccessor).GetUrl_128px(true).Url,
            page.GetCountQuestionsAggregated(sessionUser.UserId),
            new KnowledgeSummaryResponse(knowledgeSummaryLoader.Run(sessionUser.UserId, page.Id, onlyInWishKnowledge: true)),
            popularityCalculator.CalculatePagePopularity(page),
            page.Creator.Name,
            IsPublic: page.IsPublic);
    }

    public IList<UserPageItem> MapSkills(int userId)
    {
        var extendedUserCacheItem = extendedUserCache.GetUser(userId);
        
        return extendedUserCacheItem.GetAllSkills()
            .Select(skill => (skill, page: EntityCache.GetPage(skill.PageId)))
            .Where(item => permissionCheck.CanView(item.page))
            .Select(item => CreateUserPageItemFromSkill(item.skill, item.page))
            .ToList();
    }

    private UserPageItem CreateUserPageItemFromSkill(KnowledgeEvaluationCacheItem skill, PageCacheItem page)
    {
        return new UserPageItem(
            skill.PageId,
            page.Name,
            new PageImageSettings(skill.PageId, httpContextAccessor).GetUrl_128px(true).Url,
            page.GetAggregatedPublicQuestions().Count,
            new KnowledgeSummaryResponse(skill.KnowledgeSummary),
            popularityCalculator.CalculatePagePopularity(page),
            page.Creator.Name,
            IsPublic: page.IsPublic);
    }
}