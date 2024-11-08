using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class TopicToPrivateStoreController(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    PageRepository pageRepository,
    QuestionReadingRepo _questionReadingRepo,
    QuestionWritingRepo _questionWritingRepo,
    ExtendedUserCache _extendedUserCache) : Controller
{
    public readonly record struct PersonalTopic(
        string Name,
        List<int> PersonalQuestionIds,
        int PersonalQuestionCount,
        List<int> AllQuestionIds,
        int AllQuestionCount
    );

    public readonly record struct GetResult(bool Success, string MessageKey, object Data);

    [HttpGet]
    [AccessOnlyAsLoggedIn]
    public GetResult Get([FromRoute] int id)
    {
        var topicCacheItem = EntityCache.GetPage(id);
        var userCacheItem = _extendedUserCache.GetItem(_sessionUser.UserId);
        if (topicCacheItem == null)
            return new GetResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Default
            };

        if (!_permissionCheck.CanEdit(topicCacheItem))
            return new GetResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Page.MissingRights
            };

        var aggregatedTopics = topicCacheItem.AggregatedCategories(_permissionCheck)
            .Where(c => c.Value.Visibility == PageVisibility.All);
        var publicAggregatedQuestions = topicCacheItem
            .GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId, true)
            .Where(q => q.Visibility == QuestionVisibility.All).ToList();
        var pinCount = topicCacheItem.TotalRelevancePersonalEntries;
        if (!_sessionUser.IsInstallationAdmin)
        {
            if (id == RootCategory.RootCategoryId)
                return new GetResult
                {
                    Success = false,
                    MessageKey = FrontendMessageKeys.Error.Page.RootCategoryMustBePublic
                };

            foreach (var c in aggregatedTopics)
            {
                var parentCategories = c.Value.Parents();
                bool childHasPublicParent = parentCategories.Any(p =>
                    p.Visibility == PageVisibility.All && p.Id != id);

                if (!childHasPublicParent && parentCategories.Any(p => p.Id != id))
                    return new GetResult
                    {
                        Success = false,
                        MessageKey = FrontendMessageKeys.Error.Page.PublicChildCategories
                    };
            }

            var pinnedQuestionIds = new List<int>();
            foreach (var q in publicAggregatedQuestions)
            {
                bool questionIsPinned = q.TotalRelevanceForAllEntries > 0;
                if (questionIsPinned)
                    pinnedQuestionIds.Add(q.Id);
            }

            if (pinnedQuestionIds.Count > 0)
                return new GetResult
                {
                    Success = false,
                    MessageKey = FrontendMessageKeys.Error.Page.PinnedQuestions,
                    Data = pinnedQuestionIds
                };

            if (pinCount >= 10)
            {
                return new GetResult
                {
                    Success = false,
                    MessageKey = FrontendMessageKeys.Error.Page.TooPopular
                };
            }
        }

        var filteredAggregatedQuestions = publicAggregatedQuestions
            .Where(q => q.Creator != null && q.Creator.Id == userCacheItem.Id)
            .Select(q => q.Id).ToList();

        return new GetResult
        {
            Success = true,
            Data = new PersonalTopic
            {
                Name = topicCacheItem.Name,
                PersonalQuestionIds = filteredAggregatedQuestions,
                PersonalQuestionCount = filteredAggregatedQuestions.Count,
                AllQuestionIds = publicAggregatedQuestions.Select(q => q.Id).ToList(),
                AllQuestionCount = publicAggregatedQuestions.Count
            }
        };
    }

    public readonly record struct SetResult(bool Success, string MessageKey);

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public SetResult Set([FromRoute] int id)
    {
        var topicCacheItem = EntityCache.GetPage(id);
        if (topicCacheItem == null)
            return new SetResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Default
            };

        if (!_permissionCheck.CanEdit(topicCacheItem))
            return new SetResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Page.MissingRights
            };

        var topic = pageRepository.GetById(id);
        var pinCount = topic.TotalRelevancePersonalEntries;
        if (!_sessionUser.IsInstallationAdmin)
        {
            if (id == RootCategory.RootCategoryId)
                return new SetResult
                {
                    Success = false,
                    MessageKey = FrontendMessageKeys.Error.Page.RootCategoryMustBePublic
                };

            var aggregatedTopics = topicCacheItem.AggregatedCategories(_permissionCheck, false)
                .Where(c => c.Value.Visibility == PageVisibility.All);

            foreach (var c in aggregatedTopics)
            {
                var parentCategories = c.Value.Parents();
                bool childHasPublicParent = parentCategories.Any(p =>
                    p.Visibility == PageVisibility.All && p.Id != id);

                if (!childHasPublicParent && parentCategories.Any(p => p.Id != id))
                    return new SetResult
                    {
                        Success = false,
                        MessageKey = FrontendMessageKeys.Error.Page.PublicChildCategories
                    };
            }

            if (pinCount >= 10)
            {
                return new SetResult
                {
                    Success = false,
                    MessageKey = FrontendMessageKeys.Error.Page.TooPopular
                };
            }
        }

        topicCacheItem.Visibility = PageVisibility.Owner;
        topic.Visibility = PageVisibility.Owner;
        pageRepository.Update(topic, _sessionUser.UserId, type: PageChangeType.Privatized);

        return new SetResult
        {
            Success = true,
            MessageKey = FrontendMessageKeys.Success.Category.SetToPrivate
        };
    }

    public readonly record struct SetQuestionsToPrivateJson(List<int> questionIds);

    [HttpGet]
    [AccessOnlyAsLoggedIn]
    public void SetQuestionsToPrivate([FromBody] SetQuestionsToPrivateJson json)
    {
        foreach (var questionId in json.questionIds)
        {
            var questionCacheItem = EntityCache.GetQuestionById(questionId);
            var otherUsersHaveQuestionInWuwi =
                questionCacheItem.TotalRelevancePersonalEntries >
                (questionCacheItem.IsInWishknowledge(_sessionUser.UserId, _extendedUserCache)
                    ? 1
                    : 0);
            if ((questionCacheItem.Creator.Id == _sessionUser.UserId &&
                 !otherUsersHaveQuestionInWuwi) ||
                _sessionUser.IsInstallationAdmin)
            {
                questionCacheItem.Visibility = QuestionVisibility.Owner;
                EntityCache.AddOrUpdate(questionCacheItem);
                var question = _questionReadingRepo.GetById(questionId);
                question.Visibility = QuestionVisibility.Owner;
                _questionWritingRepo.UpdateOrMerge(question, false);
            }
        }
    }
}