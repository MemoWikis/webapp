using JetBrains.Annotations;

public class UserController(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    ReputationCalc _reputationCalc,
    IHttpContextAccessor _httpContextAccessor,
    ExtendedUserCache _extendedUserCache,
    PopularityCalculator _popularityCalculator,
    UserPageItemMapper _userPageItemMapper) : ApiBaseController
{
    public readonly record struct GetResult(
        User User,
        Overview Overview,
        bool IsCurrentUser,
        string MessageKey,
        NuxtErrorPageType ErrorCode,
        [CanBeNull] IList<UserPageItem> Wikis,
        [CanBeNull] IList<UserPageItem> Pages,
        [CanBeNull] IList<UserPageItem> Skills,
        [CanBeNull] IList<QuestionItem> Questions
    );

    public new readonly record struct User(
        int Id,
        string Name,
        string WikiName,
        int? WikiId,
        string ImageUrl,
        int ReputationPoints,
        int Rank,
        bool ShowWuwi,
        [CanBeNull] string AboutMeText);

    public readonly record struct Overview(
        ActivityPoints ActivityPoints,
        int PublicQuestionsCount,
        int PrivateQuestionsCount,
        int PublicPagesCount,
        int PrivatePagesCount,
        int WuwiCount,
        int PublicWikisCount,
        int Reputation,
        int Rank);

    public readonly record struct ActivityPoints(
        int Total,
        int QuestionsInOtherWishknowledges,
        int QuestionsCreated,
        int PublicWishknowledges);

    public readonly record struct QuestionItem(
        int Id,
        string Title,
        int Popularity,
        string CreationDate,
        int? PageId,
        [CanBeNull] string PageName);


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
                Pages = null,
                Skills = null,
                Questions = null
            };
        }

        var userWiki = EntityCache.GetPage(user.FirstWikiId);
        var canViewUserWiki = _permissionCheck.CanView(userWiki);
        var reputation = _reputationCalc.RunWithQuestionCacheItems(user);
        var isCurrentUser = _sessionUser.UserId == user.Id;
        var allQuestionsCreatedByUser = EntityCache.GetAllQuestions()
            .Where(q => q.Creator != null && q.CreatorId == user.Id);
        var allPagesCreatedByUser = EntityCache.GetAllPagesList()
            .Where(c => c.Creator != null && c.CreatorId == user.Id);

        var publicWikis = user.GetPublicWikis();

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
                ShowWuwi = user.ShowWishKnowledge,
                AboutMeText = user.AboutMeText
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
                WuwiCount = user.WishCountQuestions,
                PublicWikisCount = publicWikis.Count(),
                Reputation = reputation.TotalReputation,
                Rank = user.Rank
            },
            IsCurrentUser = isCurrentUser,
            Wikis = _userPageItemMapper.MapWikis(publicWikis),
            Pages = _userPageItemMapper.MapPages(user.Id, publicWikis),
            Skills = _userPageItemMapper.MapSkills(user.Id),
            Questions = GetQuestions(user.Id)
        };
        return result;
    }



    private IList<QuestionItem> GetQuestions(int userId)
    {
        var questionsCreatedByUser = EntityCache.GetAllQuestions()
            .Where(q => q.Creator != null && q.CreatorId == userId && q.Visibility == QuestionVisibility.Public)
            .Take(10) // Limit to 10 questions for now
            .ToList();

        var questionItems = new List<QuestionItem>();

        foreach (var question in questionsCreatedByUser)
        {
            if (question.IsPublic)
            {
                var primaryPage = question.PublicPages().LastOrDefault();

                if (primaryPage != null)
                    questionItems.Add(new QuestionItem(
                        question.Id,
                        question.GetShortTitle(200),
                        _popularityCalculator.CalculateQuestionPopularity(question),
                        question.DateCreated.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        primaryPage.Id,
                        primaryPage.Name
                    ));
            }
        }

        return questionItems;
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