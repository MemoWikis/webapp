using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace VueApp;

public class PublishTopicStoreController
    : BaseController
{

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public JsonResult PublishCategory(int categoryId)
    {
        var topicCacheItem = EntityCache.GetCategory(categoryId);

        if (topicCacheItem.HasPublicParent() || topicCacheItem.Creator.StartTopicId == categoryId)
        {
            if (topicCacheItem.ParentCategories(true).Any(c => c.Id == 1) && !IsInstallationAdmin)
                return Json(new
                {
                    success = false,
                    key = "parentIsRoot"
                });

            var topicRepo = Sl.CategoryRepo;
            topicCacheItem.Visibility = CategoryVisibility.All;
            var topic = topicRepo.GetById(categoryId);
            topic.Visibility = CategoryVisibility.All;
            topicRepo.Update(topic, SessionUser.User, type: CategoryChangeType.Published);

            return Json(new
            {
                success = true,
            });
        }

        return Json(new
        {
            success = false,
            key = "parentIsPrivate",
            parentList = topicCacheItem.ParentCategories().Select(c => c.Id).ToList()
        });
    }

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public void PublishQuestions(List<int> questionIds)
    {
        var questionRepo = Sl.QuestionRepo;
        foreach (var questionId in questionIds)
        {
            var questionCacheItem = EntityCache.GetQuestionById(questionId);
            if (questionCacheItem.Creator.Id == SessionUser.User.Id)
            {
                questionCacheItem.Visibility = QuestionVisibility.All;
                EntityCache.AddOrUpdate(questionCacheItem);
                var question = questionRepo.GetById(questionId);
                question.Visibility = QuestionVisibility.All;
                questionRepo.Update(question);
            }
        }
    }

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