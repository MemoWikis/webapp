using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrueOrFalse.Domain.Question.QuestionValuation;
using TrueOrFalse.Web;

namespace VueApp;

public class VueUserController(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    ReputationCalc _rpReputationCalc,
    IHttpContextAccessor _httpContextAccessor,
    SessionUserCache _sessionUserCache) : Controller
{
    public readonly record struct GetResult(User User, Overview Overview, bool IsCurrentUser);

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
        var user = EntityCache.GetUserById(id);

        if (user != null)
        {
            var userWiki = EntityCache.GetCategory(user.StartTopicId);
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
                        allTopicsCreatedByUser.Count(c => c.Visibility == CategoryVisibility.All),
                    PrivateTopicsCount =
                        allTopicsCreatedByUser.Count(c => c.Visibility != CategoryVisibility.All),
                    WuwiCount = user.WishCountQuestions
                },
                IsCurrentUser = isCurrentUser
            };
            return result;
        }

        return null;
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
            var valuations = new QuestionValuationCache(_sessionUserCache)
                .GetByUserFromCache(user.Id)
                .QuestionIds().ToList();
            var wishQuestions = EntityCache.GetQuestionsByIds(valuations)
                .Where(question => _permissionCheck.CanView(question)
                                   && question.IsInWishknowledge(id, _sessionUserCache)
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
                    .Where(t => _permissionCheck.CanView(t.CategoryCacheItem)).Select(t =>
                        new WuwiTopic
                        {
                            Name = t.CategoryCacheItem.Name,
                            Id = t.CategoryCacheItem.Id,
                            QuestionCount = t.CategoryCacheItem.CountQuestions
                        }).ToArray()
            };
        }

        return null;
    }
}