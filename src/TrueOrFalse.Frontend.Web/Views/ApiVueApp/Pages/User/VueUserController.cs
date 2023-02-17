using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Web;

namespace VueApp;

public class VueUserController : BaseController
{
    [HttpGet]
    public JsonResult Get(int id)
    {
        var user = EntityCache.GetUserById(id);
        if (user != null)
        {
            var userWiki = EntityCache.GetCategory(user.StartTopicId);
            var reputation = Resolve<ReputationCalc>().Run(user);

            var valuations = Sl.QuestionValuationRepo
                .GetByUserFromCache(user.Id)
                .QuestionIds().ToList();
            var wishQuestions = Resolve<QuestionRepo>().GetByIds(valuations).Where(PermissionCheck.CanView);

            return Json(new
            {
                user = new
                {
                    id = user.Id,
                    name = user.Name,
                    wikiUrl = PermissionCheck.CanView(userWiki) ? "/" + UriSanitizer.Run(userWiki.Name) + "/" + user.StartTopicId : null,
                    imageUrl = new UserImageSettings(user.Id).GetUrl_250px(user).Url, 
                    reputationPoints = reputation.TotalReputation,
                    rank = user.ReputationPos
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
                    publicQuestionsCount = Resolve<UserSummary>().AmountCreatedQuestions(user.Id, false),
                    publicTopicsCount = Resolve<UserSummary>().AmountCreatedCategories(user.Id),
                    wuwiCount = Resolve<GetWishQuestionCount>().Run(user.Id)
                },
                wuwi = new
                {
                    questions = wishQuestions.Select(q => new
                    {
                        title = q.GetShortTitle(200),
                        encodedName = UriSanitizer.Run(q.GetShortTitle(50)),
                        id = q.Id

                    }).ToArray(),
                    topics = wishQuestions.QuestionsInCategories().Select(t => new
                    {
                        name = t.CategoryCacheItem.Name,
                        encodedName = UriSanitizer.Run(t.CategoryCacheItem.Name),
                        id = t.CategoryCacheItem.Id
                    }).ToArray()
                },
                isCurrentUser = SessionUser.UserId == user.Id
            }, JsonRequestBehavior.AllowGet);
        }

        return Json(null, JsonRequestBehavior.AllowGet);
    }
}