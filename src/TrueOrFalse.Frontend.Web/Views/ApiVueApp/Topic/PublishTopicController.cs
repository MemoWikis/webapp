using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace VueApp;

public class PublishTopicController
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
            if (questionCacheItem.Creator == SessionUser.User)
            {
                questionCacheItem.Visibility = QuestionVisibility.All;
                EntityCache.AddOrUpdate(questionCacheItem);
                var question = questionRepo.GetById(questionId);
                question.Visibility = QuestionVisibility.All;
                questionRepo.Update(question);
            }
        }
    }
}