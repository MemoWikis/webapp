using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace VueApp;

public class TopicToPrivateStoreController : BaseController
{
    public TopicToPrivateStoreController(SessionUser sessionUser) :base(sessionUser)
    {
        
    }

    [HttpGet]
    [AccessOnlyAsLoggedIn]
    public JsonResult Get(int topicId)
    {
        var topicCacheItem = EntityCache.GetCategory(topicId);
        var userCacheItem = SessionUserCache.GetItem(User_().Id);

        if (!PermissionCheck.CanEdit(topicCacheItem))
            return Json(new
            {
                success = false,
                key = "missingRights"
            }, JsonRequestBehavior.AllowGet);

        var aggregatedTopics = topicCacheItem.AggregatedCategories()
            .Where(c => c.Value.Visibility == CategoryVisibility.All);
        var publicAggregatedQuestions = topicCacheItem.GetAggregatedQuestionsFromMemoryCache(true)
            .Where(q => q.Visibility == QuestionVisibility.All).ToList();
        var pinCount = topicCacheItem.TotalRelevancePersonalEntries;
        if (!IsInstallationAdmin)
        {
            if (topicId == RootCategory.RootCategoryId)
                return Json(new
                {
                    success = false,
                    key = "rootCategoryMustBePublic"
                }, JsonRequestBehavior.AllowGet);

            foreach (var c in aggregatedTopics)
            {
                bool childHasPublicParent = c.Value.ParentCategories()
                    .Any(p => p.Visibility == CategoryVisibility.All && p.Id != topicId);
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
                    success = false,
                    key = "tooPopular"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        var filteredAggregatedQuestions = publicAggregatedQuestions
            .Where(q => q.Creator != null && q.Creator.Id == userCacheItem.Id)
            .Select(q => q.Id).ToList();

        return Json(new
        {
            success = true,
            name = topicCacheItem.Name,
            personalQuestionIds = filteredAggregatedQuestions,
            personalQuestionCount = filteredAggregatedQuestions.Count(),
            allQuestionIds = publicAggregatedQuestions.Select(q => q.Id).ToList(),
            allQuestionCount = publicAggregatedQuestions.Count()
        }, JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public JsonResult Set(int topicId)
    {
        var topicCacheItem = EntityCache.GetCategory(topicId);
        if (!PermissionCheck.CanEdit(topicCacheItem))
            return Json(new
            {
                success = false,
                key = "missingRights"
            });

        var aggregatedTopics = topicCacheItem.AggregatedCategories(false)
            .Where(c => c.Value.Visibility == CategoryVisibility.All);
        var topic = Sl.CategoryRepo.GetById(topicId);
        var pinCount = topic.TotalRelevancePersonalEntries;
        if (!IsInstallationAdmin)
        {
            if (topicId == RootCategory.RootCategoryId)
                return Json(new
                {
                    success = false,
                    key = "rootCategoryMustBePublic"
                });

            foreach (var c in aggregatedTopics)
            {
                bool childHasPublicParent = c.Value.ParentCategories()
                    .Any(p => p.Visibility == CategoryVisibility.All && p.Id != topicId);
                if (!childHasPublicParent)
                    return Json(new
                    {
                        success = false,
                        key = "publicChildCategories"
                    });
            }

            if (pinCount >= 10)
            {
                return Json(new
                {
                    success = true,
                    key = "tooPopular"
                });
            }
        }

        topicCacheItem.Visibility = CategoryVisibility.Owner;
        topic.Visibility = CategoryVisibility.Owner;
        Sl.CategoryRepo.Update(topic, _sessionUser.User, type: CategoryChangeType.Privatized);

        return Json(new
        {
            success = true,
            key = "setToPrivate"
        });
    }

    [HttpGet]
    [AccessOnlyAsLoggedIn]
    public void SetQuestionsToPrivate(List<int> questionIds)
    {
        foreach (var questionId in questionIds)
        {
            var questionCacheItem = EntityCache.GetQuestionById(questionId);
            var otherUsersHaveQuestionInWuwi =
                questionCacheItem.TotalRelevancePersonalEntries > (questionCacheItem.IsInWishknowledge() ? 1 : 0);
            if ((questionCacheItem.Creator.Id == _sessionUser.UserId && !otherUsersHaveQuestionInWuwi) ||
                IsInstallationAdmin)
            {
                questionCacheItem.Visibility = QuestionVisibility.Owner;
                EntityCache.AddOrUpdate(questionCacheItem);
                var question = Sl.QuestionRepo.GetById(questionId);
                question.Visibility = QuestionVisibility.Owner;
                Sl.QuestionRepo.Update(question);
            }
        }
    }
}