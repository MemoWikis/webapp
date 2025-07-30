using JetBrains.Annotations;
using static MissionControlController;

public class UserController(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    ReputationCalc _rpReputationCalc,
    IHttpContextAccessor _httpContextAccessor,
    ExtendedUserCache _extendedUserCache,
    KnowledgeSummaryLoader _knowledgeSummaryLoader) : ApiBaseController
{
    public readonly record struct GetResult(
        User User,
        Overview Overview,
        bool IsCurrentUser,
        string MessageKey,
        NuxtErrorPageType ErrorCode,
        [CanBeNull] IList<PageItem> Wikis, //temp for ui building
        [CanBeNull] IList<PageItem> Skills
    );

    public new readonly record struct User(
        int Id,
        string Name,
        string WikiName,
        int? WikiId,
        string ImageUrl,
        int ReputationPoints,
        int Rank,
        bool ShowWuwi);

    public readonly record struct Overview(
        ActivityPoints ActivityPoints,
        int PublicQuestionsCount,
        int PrivateQuestionsCount,
        int PublicPagesCount,
        int PrivatePagesCount,
        int WuwiCount);

    public readonly record struct ActivityPoints(
        int Total,
        int QuestionsInOtherWishknowledges,
        int QuestionsCreated,
        int PublicWishknowledges);

    public readonly record struct PageItem(
        int Id,
        string Name,
        string ImgUrl,
        int? QuestionCount,
        KnowledgeSummaryResponse KnowledgebarData,
        [CanBeNull] string CreatorName = "");


    [HttpGet]
    public GetResult? Get([FromRoute] int id)
    {
        var user = EntityCache.GetUserByIdNullable(id);

        if (user == null)
        {
            return new GetResult
            {
                ErrorCode = NuxtErrorPageType.NotFound,
                MessageKey = FrontendMessageKeys.Error.User.NotFound,
                Skills = null
            };
        }

        var userWiki = EntityCache.GetPage(user.FirstWikiId);
        var canViewUserWiki = _permissionCheck.CanView(userWiki);
        var reputation = _rpReputationCalc.RunWithQuestionCacheItems(user);
        var isCurrentUser = _sessionUser.UserId == user.Id;
        var allQuestionsCreatedByUser = EntityCache.GetAllQuestions()
            .Where(q => q.Creator != null && q.CreatorId == user.Id);
        var allPagesCreatedByUser = EntityCache.GetAllPagesList()
            .Where(c => c.Creator != null && c.CreatorId == user.Id);

        var result = new GetResult
        {
            User = new User
            {
                Id = user.Id,
                Name = user.Name,
                WikiName = canViewUserWiki ? userWiki.Name : "",
                WikiId = canViewUserWiki ? user.FirstWikiId : null,
                ImageUrl = new UserImageSettings(user.Id, _httpContextAccessor)
                    .GetUrl_256px_square(user)
                    .Url,
                ReputationPoints = reputation.TotalReputation,
                Rank = user.ReputationPos,
                ShowWuwi = user.ShowWishKnowledge
            },
            Overview = new Overview
            {
                ActivityPoints = new ActivityPoints
                {
                    Total = reputation.TotalReputation,
                    QuestionsInOtherWishknowledges =
                        reputation.ForQuestionsInOtherWishknowledge,
                    QuestionsCreated = reputation.ForQuestionsCreated,
                    PublicWishknowledges = reputation.ForPublicWishknowledge
                },
                PublicQuestionsCount =
                    allQuestionsCreatedByUser.Count(q =>
                        q.Visibility == QuestionVisibility.Public),
                PrivateQuestionsCount =
                    allQuestionsCreatedByUser.Count(q =>
                        q.Visibility != QuestionVisibility.Public),
                PublicPagesCount =
                    allPagesCreatedByUser.Count(c => c.Visibility == PageVisibility.Public),
                PrivatePagesCount =
                    allPagesCreatedByUser.Count(c => c.Visibility != PageVisibility.Public),
                WuwiCount = user.WishCountQuestions
            },
            IsCurrentUser = isCurrentUser,
            Wikis = GetWikis(user.Id),
            Skills = GetSkills(user.Id)
        };
        return result;
    }

    /// temp start
    private IList<PageItem> GetWikis(int userId)
    {
        var userCacheItem = EntityCache.GetUserById(userId);

        var wikis = userCacheItem.GetWikis()
            .Where(_permissionCheck.CanView)
            .Select(wiki => new PageItem(
                wiki.Id,
                wiki.Name,
                new PageImageSettings(wiki.Id, _httpContextAccessor).GetUrl_128px(true).Url,
                wiki.GetCountQuestionsAggregated(_sessionUser.UserId),
                FillKnowledgeSummaryResponse(_knowledgeSummaryLoader.RunTest(_sessionUser.UserId, userId, wiki.Id))))
            .ToList();

        return wikis;
    }

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

    private IList<PageItem> GetSkills(int userId)
    {
        var extendedUserCacheItem = _extendedUserCache.GetUser(userId);
        var skillsWithPages = new List<PageItem>();

        foreach (var skill in extendedUserCacheItem.GetAllSkills())
        {
            var page = EntityCache.GetPage(skill.PageId);
            if (_permissionCheck.CanView(page))
            {
                skillsWithPages.Add(new PageItem(
                    skill.PageId,
                    page.Name,
                    new PageImageSettings(skill.PageId, _httpContextAccessor).GetUrl_128px(true).Url,
                    page.CountQuestions,
                    FillKnowledgeSummaryResponse(skill.KnowledgeSummary),
                    page.Creator.Name));
            }
        }

        return skillsWithPages;
    }

    public readonly record struct WuwiResult(WuwiQuestion[] Questions, WuwiPage[] Pages);

    public readonly record struct WuwiQuestion(
        string Title,
        string PrimaryPageName,
        int? PrimaryPageId,
        int Id);

    public readonly record struct WuwiPage(
        string Name,
        int Id,
        int QuestionCount);

    [HttpGet]
    public WuwiResult? GetWuwi([FromRoute] int id)
    {
        var user = EntityCache.GetUserById(id);

        if (user.Id > 0 && (user.ShowWishKnowledge || user.Id == _sessionUser.UserId))
        {
            var valuations = new QuestionValuationCache(_extendedUserCache)
                .GetByUserFromCache(user.Id)
                .QuestionIds().ToList();
            var wishQuestions = EntityCache.GetQuestionsByIds(valuations)
                .Where(question => _permissionCheck.CanView(question)
                                   && question.IsInWishknowledge(id, _extendedUserCache)
                                   && question.PagesVisibleToCurrentUser(_permissionCheck)
                                       .Any());

            return new WuwiResult
            {
                Questions = wishQuestions.Select(q => new WuwiQuestion
                {
                    Title = q.GetShortTitle(200),
                    PrimaryPageName = q.PagesVisibleToCurrentUser(_permissionCheck)
                        .LastOrDefault()?.Name,
                    PrimaryPageId = q.PagesVisibleToCurrentUser(_permissionCheck)
                        .LastOrDefault()?.Id,
                    Id = q.Id
                }).ToArray(),
                Pages = wishQuestions.QuestionsInPages()
                    .Where(t => _permissionCheck.CanView(t.PageCacheItem)).Select(t =>
                        new WuwiPage
                        {
                            Name = t.PageCacheItem.Name,
                            Id = t.PageCacheItem.Id,
                            QuestionCount = t.PageCacheItem.CountQuestions
                        }).ToArray()
            };
        }

        return null;
    }
}