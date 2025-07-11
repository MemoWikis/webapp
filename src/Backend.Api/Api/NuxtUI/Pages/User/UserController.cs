﻿public class UserController(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    ReputationCalc _rpReputationCalc,
    IHttpContextAccessor _httpContextAccessor,
    ExtendedUserCache _extendedUserCache) : ApiBaseController
{
    public readonly record struct GetResult(
        User User,
        Overview Overview,
        bool IsCurrentUser,
        string MessageKey,
        NuxtErrorPageType ErrorCode);

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

    [HttpGet]
    public GetResult? Get([FromRoute] int id)
    {
        var user = EntityCache.GetUserByIdNullable(id);

        if (user == null)
        {
            return new GetResult
            {
                ErrorCode = NuxtErrorPageType.NotFound,
                MessageKey = FrontendMessageKeys.Error.User.NotFound
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
            IsCurrentUser = isCurrentUser
        };
        return result;
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