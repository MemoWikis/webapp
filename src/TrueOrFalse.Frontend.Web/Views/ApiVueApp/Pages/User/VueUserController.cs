using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Web;

namespace VueApp;

public class VueUserController : BaseController
{
    private readonly PermissionCheck _permissionCheck;

    public VueUserController(SessionUser sessionUser, PermissionCheck permissionCheck) :base(sessionUser)
    {
        _permissionCheck = permissionCheck;
    }
    [HttpGet]
    public JsonResult Get(int id)
    {
        var user = EntityCache.GetUserById(id);

        if (user != null)
        {
            var userWiki = EntityCache.GetCategory(user.StartTopicId);
            var reputation = Resolve<ReputationCalc>().RunWithQuestionCacheItems(user);
            var isCurrentUser = _sessionUser.UserId == user.Id;
            var allQuestionsCreatedByUser = EntityCache.GetAllQuestions().Where(q => q.Creator != null && q.CreatorId == user.Id);
            var allTopicsCreatedByUser = EntityCache.GetAllCategories().Where(c => c.Creator != null && c.CreatorId == user.Id);
            var result = new
            {
                user = new
                {
                    id = user.Id,
                    name = user.Name,
                    wikiUrl = _permissionCheck.CanView(userWiki)
                        ? "/" + UriSanitizer.Run(userWiki.Name) + "/" + user.StartTopicId
                        : null,
                    imageUrl = new UserImageSettings(user.Id).GetUrl_250px(user).Url,
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
            return Json(result, JsonRequestBehavior.AllowGet);


        }

        return Json(null, JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    public JsonResult GetWuwi(int id)
    {
        var user = EntityCache.GetUserById(id);

        if (user.Id > 0 && (user.ShowWishKnowledge || user.Id == _sessionUser.UserId))
        {
            var valuations = Sl.QuestionValuationRepo
                .GetByUserFromCache(user.Id)
                .QuestionIds().ToList();
            var wishQuestions = EntityCache.GetQuestionsByIds(valuations).Where(question => _permissionCheck.CanView(question) && question.IsInWishknowledge(id) && question.CategoriesVisibleToCurrentUser(_permissionCheck).Any());

            return Json(new
            {
                questions = wishQuestions.Select(q => new
                {
                    title = q.GetShortTitle(200),
                    primaryTopicName =q.CategoriesVisibleToCurrentUser(_permissionCheck).LastOrDefault()?.Name,
                    primaryTopicId = q.CategoriesVisibleToCurrentUser(_permissionCheck).LastOrDefault()?.Id,
                    id = q.Id

                }).ToArray(),
                topics = wishQuestions.QuestionsInCategories().Where(t => _permissionCheck.CanView(t.CategoryCacheItem)).Select(t => new
                {
                    name = t.CategoryCacheItem.Name,
                    id = t.CategoryCacheItem.Id,
                    questionCount = t.CategoryCacheItem.CountQuestions
                }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
        return Json(null, JsonRequestBehavior.AllowGet);
    }

}