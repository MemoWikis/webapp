using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace VueApp
{
    public class PublishPageStoreController(
        SessionUser _sessionUser,
        PermissionCheck _permissionCheck,
        PageRepository pageRepository,
        QuestionReadingRepo _questionReadingRepo,
        QuestionWritingRepo _questionWritingRepo,
        ExtendedUserCache _extendedUserCache) : Controller
    {
        public readonly record struct PublishPageJson(int id);

        public readonly record struct PublishPageResult(
            bool Success,
            string MessageKey,
            List<int> Data);

        [HttpPost]
        [AccessOnlyAsLoggedIn]
        public PublishPageResult PublishPage([FromBody] PublishPageJson json)
        {
            var topicCacheItem = EntityCache.GetPage(json.id);

            if (topicCacheItem != null)
            {
                if (topicCacheItem.HasPublicParent() ||
                    topicCacheItem.Creator.StartPageId == json.id)
                {
                    if (topicCacheItem.Parents().Any(c => c.Id == 1) &&
                        !_sessionUser.IsInstallationAdmin)
                        return new PublishPageResult
                        {
                            Success = false,
                            MessageKey = FrontendMessageKeys.Error.Page.ParentIsRoot
                        };

                    topicCacheItem.Visibility = PageVisibility.All;
                    var topic = pageRepository.GetById(json.id);
                    topic.Visibility = PageVisibility.All;
                    pageRepository.Update(topic, _sessionUser.UserId,
                        type: PageChangeType.Published);
                    return new PublishPageResult
                    {
                        Success = true,
                    };
                }

                return new PublishPageResult
                {
                    Success = false,
                    MessageKey = FrontendMessageKeys.Error.Page.ParentIsPrivate,
                    Data = topicCacheItem.Parents().Select(c => c.Id).ToList()
                };
            }

            return new PublishPageResult
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

        public readonly record struct TinyPage(
            bool Success,
            string Name,
            List<int> QuestionIds,
            int QuestionCount);

        [HttpGet]
        [AccessOnlyAsLoggedIn]
        public TinyPage Get([FromRoute] int id)
        {
            var topicCacheItem = EntityCache.GetPage(id);
            var userCacheItem = _extendedUserCache.GetItem(_sessionUser.UserId);

            if (topicCacheItem.Creator == null || topicCacheItem.Creator.Id != userCacheItem.Id)
                return new TinyPage
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

            return new TinyPage
            {
                Success = true,
                Name = topicCacheItem.Name,
                QuestionIds = filteredAggregatedQuestions,
                QuestionCount = filteredAggregatedQuestions.Count
            };
        }
    }
}