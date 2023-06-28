﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace VueApp;

public class PublishTopicStoreController : BaseController
{
    private readonly PermissionCheck _permissionCheck;
    private readonly CategoryValuationRepo _categoryValuationRepo;

    public PublishTopicStoreController(SessionUser sessionUser,
        PermissionCheck permissionCheck,
        CategoryValuationRepo categoryValuationRepo): base(sessionUser)
    {
        _permissionCheck = permissionCheck;
        _categoryValuationRepo = categoryValuationRepo;
    }

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public JsonResult PublishTopic(int topicId)
    {
        var topicCacheItem = EntityCache.GetCategory(topicId);

        if (topicCacheItem.HasPublicParent() || topicCacheItem.Creator.StartTopicId == topicId)
        {
            if (topicCacheItem.ParentCategories(true).Any(c => c.Id == 1) && !IsInstallationAdmin)
                return Json(new
                {
                    success = false,
                    key = "parentIsRoot"
                });

            var topicRepo = Sl.CategoryRepo;
            topicCacheItem.Visibility = CategoryVisibility.All;
            var topic = topicRepo.GetById(topicId);
            topic.Visibility = CategoryVisibility.All;
            topicRepo.Update(topic, _sessionUser.User, type: CategoryChangeType.Published);

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
            if (questionCacheItem.Creator.Id == _sessionUser.User.Id)
            {
                questionCacheItem.Visibility = QuestionVisibility.All;
                EntityCache.AddOrUpdate(questionCacheItem);
                var question = questionRepo.GetById(questionId);
                question.Visibility = QuestionVisibility.All;
                questionRepo.Update(question);
            }
        }
    }

    [HttpGet]
    [AccessOnlyAsLoggedIn]
    public JsonResult Get(int topicId)
    {
        var topicCacheItem = EntityCache.GetCategory(topicId);
        var userCacheItem = SessionUserCache.GetItem(User_().Id, _categoryValuationRepo);

        if (topicCacheItem.Creator == null || topicCacheItem.Creator.Id != userCacheItem.Id)
            return Json(new
            {
                success = false,
            }, JsonRequestBehavior.AllowGet);

        var filteredAggregatedQuestions = topicCacheItem
            .GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId)
            .Where(q =>
                q.Creator != null &&
                q.Creator.Id == userCacheItem.Id &&
                q.IsPrivate() &&
                _permissionCheck.CanEdit(q))
            .Select(q => q.Id).ToList();

        return Json(new
        {
            success = true,
            name = topicCacheItem.Name,
            questionIds = filteredAggregatedQuestions,
            questionCount = filteredAggregatedQuestions.Count()
        }, JsonRequestBehavior.AllowGet);
    }
}