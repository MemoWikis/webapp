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

public static class UserPageItemMapper
{
    public static IList<UserPageItem> MapWikis(
        IEnumerable<PageCacheItem> wikis,
        SessionUser sessionUser,
        IHttpContextAccessor httpContextAccessor,
        KnowledgeSummaryLoader knowledgeSummaryLoader,
        PopularityCalculator popularityCalculator)
    {
        return wikis.Select(wiki =>
                new UserPageItem(
                    wiki.Id,
                    wiki.Name,
                    new PageImageSettings(wiki.Id, httpContextAccessor).GetUrl_128px(true).Url,
                    wiki.GetCountQuestionsAggregated(sessionUser.UserId),
                    new KnowledgeSummaryResponse(knowledgeSummaryLoader.Run(sessionUser.UserId, wiki.Id, onlyInWishknowledge: true)),
                    popularityCalculator.CalculatePagePopularity(wiki))
            )
            .ToList();
    }

    public static IList<UserPageItem> MapPages(
        int userId,
        IEnumerable<PageCacheItem> userWikis,
        SessionUser sessionUser,
        IHttpContextAccessor httpContextAccessor,
        KnowledgeSummaryLoader knowledgeSummaryLoader,
        PopularityCalculator popularityCalculator,
        PermissionCheck permissionCheck)
    {
        var user = EntityCache.GetUserById(userId);
        var userWikiIds = userWikis.Select(w => w.Id).ToHashSet();
        
        var publicPagesCreatedByUser = EntityCache.GetAllPagesList()
            .Where(page => page.Creator != null 
                       && page.CreatorId == userId 
                       && page.Visibility == PageVisibility.Public
                       && !userWikiIds.Contains(page.Id)  // Exclude pages that are user's own wikis
                       && !page.Parents().Any(parent => userWikiIds.Contains(parent.Id))) // Exclude pages under user's own wikis
            .Where(permissionCheck.CanView)
            .ToList();

        // Filter out child pages when their parent is also created by the same user and in the list
        var pageIds = publicPagesCreatedByUser.Select(p => p.Id).ToHashSet();
        var filteredPages = publicPagesCreatedByUser
            .Where(page => !page.Parents().Any(parent => 
                parent.CreatorId == userId && pageIds.Contains(parent.Id)))
            .ToList();

        return filteredPages.Select(page =>
                new UserPageItem(
                    page.Id,
                    page.Name,
                    new PageImageSettings(page.Id, httpContextAccessor).GetUrl_128px(true).Url,
                    page.GetCountQuestionsAggregated(sessionUser.UserId),
                    new KnowledgeSummaryResponse(knowledgeSummaryLoader.Run(sessionUser.UserId, page.Id, onlyInWishknowledge: true)),
                    popularityCalculator.CalculatePagePopularity(page),
                    page.Creator.Name,
                    IsPublic: page.IsPublic)
            )
            .ToList();
    }

    public static IList<UserPageItem> MapSkills(
        int userId,
        ExtendedUserCache extendedUserCache,
        IHttpContextAccessor httpContextAccessor,
        PopularityCalculator popularityCalculator,
        PermissionCheck permissionCheck)
    {
        var extendedUserCacheItem = extendedUserCache.GetUser(userId);
        var skillsWithPages = new List<UserPageItem>();

        foreach (var skill in extendedUserCacheItem.GetAllSkills())
        {
            var page = EntityCache.GetPage(skill.PageId);
            if (permissionCheck.CanView(page))
            {
                skillsWithPages.Add(
                    new UserPageItem(
                        skill.PageId,
                        page.Name,
                        new PageImageSettings(skill.PageId, httpContextAccessor).GetUrl_128px(true).Url,
                        page.GetAggregatedPublicQuestions().Count,
                        new KnowledgeSummaryResponse(skill.KnowledgeSummary),
                        popularityCalculator.CalculatePagePopularity(page),
                        page.Creator.Name,
                        IsPublic: page.IsPublic
                    )
                );
            }
        }

        return skillsWithPages;
    }
}