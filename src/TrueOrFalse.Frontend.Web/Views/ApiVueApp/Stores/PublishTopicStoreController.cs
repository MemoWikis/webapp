﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace VueApp
{

    public class PublishTopicStoreController : BaseController
    {
        private readonly PermissionCheck _permissionCheck;
        private readonly CategoryRepository _categoryRepository;
        private readonly QuestionReadingRepo _questionReadingRepo;
        private readonly QuestionWritingRepo _questionWritingRepo;
        private readonly SessionUserCache _sessionUserCache;
      

        public PublishTopicStoreController(SessionUser sessionUser,
            PermissionCheck permissionCheck,
            CategoryRepository categoryRepository,
            QuestionReadingRepo questionReadingRepo,
            QuestionWritingRepo questionWritingRepo,
            SessionUserCache sessionUserCache) : base(sessionUser)
        {
            _permissionCheck = permissionCheck;
            _categoryRepository = categoryRepository;
            _questionReadingRepo = questionReadingRepo;
            _questionWritingRepo = questionWritingRepo;
            _sessionUserCache = sessionUserCache;
        }

        public readonly record struct PublishTopicJson(int id);
        [HttpPost] 
        [AccessOnlyAsLoggedIn]
        public JsonResult PublishTopic([FromBody] PublishTopicJson json)
        {
            var topicCacheItem = EntityCache.GetCategory(json.id);

            if (topicCacheItem != null)
            {

                if (topicCacheItem.HasPublicParent() || topicCacheItem.Creator.StartTopicId == json.id)
                {
                    if (topicCacheItem.Parents().Any(c => c.Id == 1) && !_sessionUser.IsInstallationAdmin)
                        return Json(new RequestResult
                        {
                            success = false,
                            messageKey = FrontendMessageKeys.Error.Category.ParentIsRoot
                        });

                    topicCacheItem.Visibility = CategoryVisibility.All;
                    var topic = _categoryRepository.GetById(json.id);
                    topic.Visibility = CategoryVisibility.All;
                    _categoryRepository.Update(topic, _sessionUser.UserId, type: CategoryChangeType.Published);

                    return Json(new RequestResult
                    {
                        success = true,
                    });
                } 

                return Json(new RequestResult
                {
                    success = false,
                    messageKey = FrontendMessageKeys.Error.Category.ParentIsPrivate,
                    data = topicCacheItem.Parents().Select(c => c.Id).ToList()
                });

            }

            return Json(new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Default
            });
        }
        public readonly record struct PublishQuestionsJson(List<int> questionIds);

        [HttpPost]
        [AccessOnlyAsLoggedIn]
        public void PublishQuestions([FromBody] PublishQuestionsJson json)
        { 
            foreach (var questionId in json.questionIds) 
            {
                var questionCacheItem =
                    EntityCache.GetQuestionById(questionId);
                if (questionCacheItem.Creator.Id == _sessionUser.User.Id)
                {
                    questionCacheItem.Visibility = QuestionVisibility.All;
                    EntityCache.AddOrUpdate(questionCacheItem);
                    var question = _questionReadingRepo.GetById(questionId);
                    question.Visibility = QuestionVisibility.All;
                    _questionWritingRepo.UpdateOrMerge(question, false);
                }
            }
        }

        [HttpGet]
        [AccessOnlyAsLoggedIn]
        public JsonResult Get([FromRoute] int id)
        {
            var topicCacheItem = EntityCache.GetCategory(id);
            var userCacheItem = _sessionUserCache.GetItem(_sessionUser.UserId);

            if (topicCacheItem.Creator == null || topicCacheItem.Creator.Id != userCacheItem.Id)
                return Json(new
                {
                    success = false,
                });

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
                questionCount = filteredAggregatedQuestions.Count
            });
        }
    }
}
