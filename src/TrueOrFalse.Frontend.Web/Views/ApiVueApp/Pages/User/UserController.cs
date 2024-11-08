using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TrueOrFalse.Domain.Question.QuestionValuation;
using TrueOrFalse.Web;

namespace VueApp;

public class UserController(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    ReputationCalc _rpReputationCalc,
    IHttpContextAccessor _httpContextAccessor,
    ExtendedUserCache _extendedUserCache) : Controller
{
    public readonly record struct GetResult(User User, Overview Overview, bool IsCurrentUser, string? MessageKey, NuxtErrorPageType ErrorCode);

    public readonly record struct User(
        int Id,
        string Name,
        string WikiUrl,
        string ImageUrl,
        int ReputationPoints,
        int Rank,
        bool ShowWuwi);

    public readonly record struct Overview(
        ActivityPoints ActivityPoints,
        int PublicQuestionsCount,
        int PrivateQuestionsCount,
        int PublicTopicsCount,
        int PrivateTopicsCount,
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

        var userWiki = EntityCache.GetPage(user.StartTopicId);
        var reputation = _rpReputationCalc.RunWithQuestionCacheItems(user);
        var isCurrentUser = _sessionUser.UserId == user.Id;
        var allQuestionsCreatedByUser = EntityCache.GetAllQuestions()
            .Where(q => q.Creator != null && q.CreatorId == user.Id);
        var allTopicsCreatedByUser = EntityCache.GetAllCategoriesList()
            .Where(c => c.Creator != null && c.CreatorId == user.Id);
        var result = new GetResult
        {
            User = new User
            {
                Id = user.Id,
                Name = user.Name,
                WikiUrl = _permissionCheck.CanView(userWiki)
                    ? "/" + UriSanitizer.Run(userWiki.Name) + "/" + user.StartTopicId
                    : null,
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
                        q.Visibility == QuestionVisibility.All),
                PrivateQuestionsCount =
                    allQuestionsCreatedByUser.Count(q =>
                        q.Visibility != QuestionVisibility.All),
                PublicTopicsCount =
                    allTopicsCreatedByUser.Count(c => c.Visibility == PageVisibility.All),
                PrivateTopicsCount =
                    allTopicsCreatedByUser.Count(c => c.Visibility != PageVisibility.All),
                WuwiCount = user.WishCountQuestions
            },
            IsCurrentUser = isCurrentUser
        };
        return result;
    }

    public readonly record struct WuwiResult(WuwiQuestion[] Questions, WuwiTopic[] Topics);

    public readonly record struct WuwiQuestion(
        string Title,
        string PrimaryTopicName,
        int? PrimaryTopicId,
        int Id);

    public readonly record struct WuwiTopic(
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
                                   && question.CategoriesVisibleToCurrentUser(_permissionCheck)
                                       .Any());

            return new WuwiResult
            {
                Questions = wishQuestions.Select(q => new WuwiQuestion
                {
                    Title = q.GetShortTitle(200),
                    PrimaryTopicName = q.CategoriesVisibleToCurrentUser(_permissionCheck)
                        .LastOrDefault()?.Name,
                    PrimaryTopicId = q.CategoriesVisibleToCurrentUser(_permissionCheck)
                        .LastOrDefault()?.Id,
                    Id = q.Id
                }).ToArray(),
                Topics = wishQuestions.QuestionsInCategories()
                    .Where(t => _permissionCheck.CanView(t.PageCacheItem)).Select(t =>
                        new WuwiTopic
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