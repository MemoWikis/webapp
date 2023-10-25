using System.Collections.Generic;
using System.Linq;
using HelperClassesControllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrueOrFalse.Environment;

namespace VueApp
{

    public class PublishTopicStoreController : Controller
    {
        private readonly SessionUser _sessionUser;
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
            SessionUserCache sessionUserCache)
        {
            _sessionUser = sessionUser;
            _permissionCheck = permissionCheck;
            _categoryRepository = categoryRepository;
            _questionReadingRepo = questionReadingRepo;
            _questionWritingRepo = questionWritingRepo;
            _sessionUserCache = sessionUserCache;
        }

        [HttpPost]
        [AccessOnlyAsLoggedIn]
        public JsonResult PublishTopic([FromBody] TopicIdHelper topicIdHelper)
        {
            var topicCacheItem = EntityCache.GetCategory(topicIdHelper.TopicId);

            if (topicCacheItem.HasPublicParent() || topicCacheItem.Creator.StartTopicId == topicIdHelper.TopicId)
            {
                if (topicCacheItem.ParentCategories(true).Any(c => c.Id == 1) && !_sessionUser.IsInstallationAdmin)
                    return Json(new
                    {
                        success = false,
                        key = "parentIsRoot"
                    });


                topicCacheItem.Visibility = CategoryVisibility.All;
                var topic = _categoryRepository.GetById(topicIdHelper.TopicId);
                topic.Visibility = CategoryVisibility.All;
                _categoryRepository.Update(topic, _sessionUser.UserId, type: CategoryChangeType.Published);

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
            foreach (var questionId in questionIds)
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
        public JsonResult Get([FromQuery] TopicIdHelper topicIdHelper)
        {
            var topicCacheItem = EntityCache.GetCategory(topicIdHelper.TopicId);
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
                questionCount = filteredAggregatedQuestions.Count()
            });
        }
    }
}

namespace HelperClassesControllers
{
    public class TopicIdHelper
    {
        public int TopicId { get; set; }
    }
}