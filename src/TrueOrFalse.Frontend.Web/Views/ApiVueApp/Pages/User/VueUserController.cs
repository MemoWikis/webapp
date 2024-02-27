using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrueOrFalse.Domain.Question.QuestionValuation;
using TrueOrFalse.Web;

namespace VueApp;

public class VueUserController : BaseController
{
    private readonly PermissionCheck _permissionCheck;
    private readonly ReputationCalc _rpReputationCalc;
    private readonly QuestionValuationReadingRepo _questionValuationReadingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly SessionUserCache _sessionUserCache;

    public VueUserController(SessionUser sessionUser,
        PermissionCheck permissionCheck,
        ReputationCalc rpReputationCalc,
        QuestionValuationReadingRepo questionValuationReadingRepo,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        SessionUserCache sessionUserCache) : base(sessionUser)
    {
        _permissionCheck = permissionCheck;
        _rpReputationCalc = rpReputationCalc;
        _questionValuationReadingRepo = questionValuationReadingRepo;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _sessionUserCache = sessionUserCache;
    }
    [HttpGet]
    public JsonResult Get([FromRoute] int id)
    {
        var user = EntityCache.GetUserById(id);

        if (user != null)
        {
            var userWiki = EntityCache.GetCategory(user.StartTopicId);
            var reputation = _rpReputationCalc.RunWithQuestionCacheItems(user);
            var isCurrentUser = _sessionUser.UserId == user.Id;
            var allQuestionsCreatedByUser = EntityCache.GetAllQuestions().Where(q => q.Creator != null && q.CreatorId == user.Id);
            var allTopicsCreatedByUser = EntityCache.GetAllCategoriesList().Where(c => c.Creator != null && c.CreatorId == user.Id);
            var result = new
            {
                user = new
                {
                    id = user.Id,
                    name = user.Name,
                    wikiUrl = _permissionCheck.CanView(userWiki)
                        ? "/" + UriSanitizer.Run(userWiki.Name) + "/" + user.StartTopicId
                        : null,
                    imageUrl = new UserImageSettings(user.Id, _httpContextAccessor)
                        .GetUrl_256px_square(user)
                        .Url,
                    reputationPoints = reputation.TotalReputation,
                    rank = user.ReputationPos,
                    showWuwi = user.ShowWishKnowledge
                },
                overview = new
                {
                    activityPoints = new
                    {
                        total = reputation.TotalReputation,
                        questionsInOtherWishknowledges = reputation.ForQuestionsInOtherWishknowledge,
                        questionsCreated = reputation.ForQuestionsCreated,
                        publicWishknowledges = reputation.ForPublicWishknowledge
                    },
                    publicQuestionsCount = allQuestionsCreatedByUser.Count(q => q.Visibility == QuestionVisibility.All),
                    privateQuestionsCount =
                        allQuestionsCreatedByUser.Count(q => q.Visibility != QuestionVisibility.All),
                    publicTopicsCount = allTopicsCreatedByUser.Count(c => c.Visibility == CategoryVisibility.All),
                    privateTopicsCount = allTopicsCreatedByUser.Count(c => c.Visibility != CategoryVisibility.All),
                    wuwiCount = user.WishCountQuestions
                },
                isCurrentUser = isCurrentUser
            };
            return Json(result);


        }

        return Json(null);
    }

    [HttpGet]
    public JsonResult GetWuwi([FromRoute] int id)
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
                    && question.CategoriesVisibleToCurrentUser(_permissionCheck).Any());

            return Json(new
            {
                questions = wishQuestions.Select(q => new
                {
                    title = q.GetShortTitle(200),
                    primaryTopicName = q.CategoriesVisibleToCurrentUser(_permissionCheck).LastOrDefault()?.Name,
                    primaryTopicId = q.CategoriesVisibleToCurrentUser(_permissionCheck).LastOrDefault()?.Id,
                    id = q.Id

                }).ToArray(),
                topics = wishQuestions.QuestionsInCategories().Where(t => _permissionCheck.CanView(t.CategoryCacheItem)).Select(t => new
                {
                    name = t.CategoryCacheItem.Name,
                    id = t.CategoryCacheItem.Id,
                    questionCount = t.CategoryCacheItem.CountQuestions
                }).ToArray()
            });
        }
        return Json(null);
    }

}