using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace VueApp;

public class SetTopicToPrivateController : BaseController
{
    [HttpGet]
    [AccessOnlyAsLoggedIn]
    public JsonResult Get(int id)
    {
        var topicCacheItem = EntityCache.GetCategory(id);
        var userCacheItem = EntityCache.GetUserById(SessionUser.UserId);

        if (!PermissionCheck.CanEdit(topicCacheItem))
            return Json(new
            {
                success = false,
                key = "missingRights"
            }, JsonRequestBehavior.AllowGet);

        var aggregatedTopics = topicCacheItem.AggregatedCategories()
            .Where(c => c.Value.Visibility == CategoryVisibility.All);
        var publicAggregatedQuestions = topicCacheItem.GetAggregatedQuestionsFromMemoryCache(true).Where(q => q.Visibility == QuestionVisibility.All).ToList();
        var pinCount = topicCacheItem.TotalRelevancePersonalEntries;
        if (!IsInstallationAdmin)
        {
            if (id == RootCategory.RootCategoryId)
                return Json(new
                {
                    success = false,
                    key = "rootCategoryMustBePublic"
                }, JsonRequestBehavior.AllowGet);

            foreach (var c in aggregatedTopics)
            {
                bool childHasPublicParent = c.Value.ParentCategories().Any(p => p.Visibility == CategoryVisibility.All && p.Id != id);
                if (!childHasPublicParent)
                    return Json(new
                    {
                        success = false,
                        key = "publicChildCategories"
                    }, JsonRequestBehavior.AllowGet);
            }

            var pinnedQuestionIds = new List<int>();
            foreach (var q in publicAggregatedQuestions)
            {
                bool questionIsPinned = q.TotalRelevanceForAllEntries > 0;
                if (questionIsPinned)
                    pinnedQuestionIds.Add(q.Id);
            }

            if (pinnedQuestionIds.Count > 0)
                return Json(new
                {
                    success = false,
                    key = "questionIsPinned",
                    pinnedQuestionIds
                }, JsonRequestBehavior.AllowGet);

            if (pinCount >= 10)
            {
                return Json(new
                {
                    success = true,
                    key = "tooPopular"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        var filteredAggregatedQuestions = publicAggregatedQuestions.Where(q => q.Creator != null && q.Creator.Id == userCacheItem.Id)
            .Select(q => q.Id).ToList();

        return Json(new
        {
            categoryName = topicCacheItem.Name,
            personalQuestionIds = filteredAggregatedQuestions,
            personalQuestionCount = filteredAggregatedQuestions.Count(),
            allQuestionIds = publicAggregatedQuestions.Select(q => q.Id).ToList(),
            allQuestionCount = publicAggregatedQuestions.Count()
        }, JsonRequestBehavior.AllowGet);
    }
}