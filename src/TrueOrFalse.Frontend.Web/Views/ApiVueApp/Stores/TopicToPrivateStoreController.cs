using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace VueApp;

public class TopicToPrivateStoreController
    : BaseController
{
    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public JsonResult Get(int categoryId)
    {
        var categoryCacheItem = EntityCache.GetCategory(categoryId);
        var userCacheItem = SessionUserCache.GetItem(User_().Id);


        if (!PermissionCheck.CanEdit(categoryCacheItem))
            return Json(new
            {
                success = false,
                key = "missingRights"
            });

        var aggregatedCategories = categoryCacheItem.AggregatedCategories()
            .Where(c => c.Value.Visibility == CategoryVisibility.All);
        var publicAggregatedQuestions = categoryCacheItem.GetAggregatedQuestionsFromMemoryCache(true).Where(q => q.Visibility == QuestionVisibility.All).ToList();
        var pinCount = categoryCacheItem.TotalRelevancePersonalEntries;
        if (!IsInstallationAdmin)
        {
            if (categoryId == RootCategory.RootCategoryId)
                return Json(new
                {
                    success = false,
                    key = "rootCategoryMustBePublic"
                });

            foreach (var c in aggregatedCategories)
            {
                bool childHasPublicParent = c.Value.ParentCategories().Any(p => p.Visibility == CategoryVisibility.All && p.Id != categoryId);
                if (!childHasPublicParent)
                    return Json(new
                    {
                        success = false,
                        key = "publicChildCategories"
                    });
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
                });

            if (pinCount >= 10)
            {
                return Json(new
                {
                    success = true,
                    key = "tooPopular"
                });
            }
        }

        var filteredAggregatedQuestions = publicAggregatedQuestions.Where(q => q.Creator != null && q.Creator.Id == userCacheItem.Id)
            .Select(q => q.Id).ToList();

        return Json(new
        {
            categoryName = categoryCacheItem.Name,
            personalQuestionIds = filteredAggregatedQuestions,
            personalQuestionCount = filteredAggregatedQuestions.Count(),
            allQuestionIds = publicAggregatedQuestions.Select(q => q.Id).ToList(),
            allQuestionCount = publicAggregatedQuestions.Count()
        });
    }
}