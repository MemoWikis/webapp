using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace VueApp
{
    public class PublishTopicStoreController(
        SessionUser _sessionUser,
        PermissionCheck _permissionCheck,
        PageRepository pageRepository,
        QuestionReadingRepo _questionReadingRepo,
        QuestionWritingRepo _questionWritingRepo,
        ExtendedUserCache _extendedUserCache) : Controller
    {
        public readonly record struct PublishTopicJson(int id);

        public readonly record struct PublishTopicResult(
            bool Success,
            string MessageKey,
            List<int> Data);

        [HttpPost]
        [AccessOnlyAsLoggedIn]
        public PublishTopicResult PublishTopic([FromBody] PublishTopicJson json)
        {
            var topicCacheItem = EntityCache.GetPage(json.id);

            if (topicCacheItem != null)
            {
                if (topicCacheItem.HasPublicParent() ||
                    topicCacheItem.Creator.StartTopicId == json.id)
                {
                    if (topicCacheItem.Parents().Any(c => c.Id == 1) &&
                        !_sessionUser.IsInstallationAdmin)
                        return new PublishTopicResult
                        {
                            Success = false,
                            MessageKey = FrontendMessageKeys.Error.Page.ParentIsRoot
                        };

                    topicCacheItem.Visibility = PageVisibility.All;
                    var topic = pageRepository.GetById(json.id);
                    topic.Visibility = PageVisibility.All;
                    pageRepository.Update(topic, _sessionUser.UserId,
                        type: PageChangeType.Published);
                    return new PublishTopicResult
                    {
                        Success = true,
                    };
                }

                return new PublishTopicResult
                {
                    Success = false,
                    MessageKey = FrontendMessageKeys.Error.Page.ParentIsPrivate,
                    Data = topicCacheItem.Parents().Select(c => c.Id).ToList()
                };
            }

            return new PublishTopicResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Default
            };
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

        public readonly record struct TinyTopic(
            bool Success,
            string Name,
            List<int> QuestionIds,
            int QuestionCount);

        [HttpGet]
        [AccessOnlyAsLoggedIn]
        public TinyTopic Get([FromRoute] int id)
        {
            var topicCacheItem = EntityCache.GetPage(id);
            var userCacheItem = _extendedUserCache.GetItem(_sessionUser.UserId);

            if (topicCacheItem.Creator == null || topicCacheItem.Creator.Id != userCacheItem.Id)
                return new TinyTopic
                {
                    Success = false,
                };

            var filteredAggregatedQuestions = topicCacheItem
                .GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId)
                .Where(q =>
                    q.Creator != null &&
                    q.Creator.Id == userCacheItem.Id &&
                    q.IsPrivate() &&
                    _permissionCheck.CanEdit(q))
                .Select(q => q.Id).ToList();

            return new TinyTopic
            {
                Success = true,
                Name = topicCacheItem.Name,
                QuestionIds = filteredAggregatedQuestions,
                QuestionCount = filteredAggregatedQuestions.Count
            };
        }
    }
}