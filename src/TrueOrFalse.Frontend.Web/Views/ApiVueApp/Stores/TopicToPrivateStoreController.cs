using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TopicToPrivateStoreHelper;

namespace VueApp;

public class TopicToPrivateStoreController : Controller
{
    private readonly SessionUser _sessionUser;
    private readonly PermissionCheck _permissionCheck;
    private readonly CategoryValuationReadingRepo _categoryValuationReadingRepo;
    private readonly CategoryRepository _categoryRepository;
    private readonly QuestionReadingRepo _questionReadingRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly QuestionValuationReadingRepo _questionValuationReadingRepo;
    private readonly QuestionWritingRepo _questionWritingRepo;
    private readonly SessionUserCache _sessionUserCache;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public TopicToPrivateStoreController(SessionUser sessionUser,
        PermissionCheck permissionCheck, 
        CategoryValuationReadingRepo categoryValuationReadingRepo,
        CategoryRepository categoryRepository,
        QuestionReadingRepo questionReadingRepo,
        UserReadingRepo userReadingRepo,
        QuestionValuationReadingRepo questionValuationReadingRepo,
        QuestionWritingRepo questionWritingRepo,
        SessionUserCache sessionUserCache,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment) 
    {
        _sessionUser = sessionUser;
        _permissionCheck = permissionCheck;
        _categoryValuationReadingRepo = categoryValuationReadingRepo;
        _categoryRepository = categoryRepository;
        _questionReadingRepo = questionReadingRepo;
        _userReadingRepo = userReadingRepo;
        _questionValuationReadingRepo = questionValuationReadingRepo;
        _questionWritingRepo = questionWritingRepo;
        _sessionUserCache = sessionUserCache;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    [AccessOnlyAsLoggedIn]
    public JsonResult Get([FromRoute] int id)
    {
        var topicCacheItem = EntityCache.GetCategory(id);
        var userCacheItem = _sessionUserCache.GetItem(_sessionUser.UserId);
        if (topicCacheItem == null)
            return Json(new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Default
            });

        if (!_permissionCheck.CanEdit(topicCacheItem))
            return Json(new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Category.MissingRights
            });

        var aggregatedTopics = topicCacheItem.AggregatedCategories(_permissionCheck)
            .Where(c => c.Value.Visibility == CategoryVisibility.All);
        var publicAggregatedQuestions = topicCacheItem.GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId, true)
            .Where(q => q.Visibility == QuestionVisibility.All).ToList();
        var pinCount = topicCacheItem.TotalRelevancePersonalEntries;
        if (!_sessionUser.IsInstallationAdmin)
        {
            if (id == RootCategory.RootCategoryId)
                return Json(new RequestResult
                {
                    success = false,
                    messageKey = FrontendMessageKeys.Error.Category.RootCategoryMustBePublic
                });

            foreach (var c in aggregatedTopics)
            {
                bool childHasPublicParent = c.Value.ParentCategories()
                    .Any(p => p.Visibility == CategoryVisibility.All && p.Id != id);
                if (!childHasPublicParent)
                    return Json(new RequestResult
                    {
                        success = false,
                        messageKey = FrontendMessageKeys.Error.Category.PublicChildCategories
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
                return Json(new RequestResult
                {
                    success = false,
                    messageKey = FrontendMessageKeys.Error.Category.PinnedQuestions,
                    data = pinnedQuestionIds
                });


            if (pinCount >= 10)
            {
                return Json(new RequestResult
                {
                    success = false,
                    messageKey = FrontendMessageKeys.Error.Category.TooPopular
                });
            }
        }

        var filteredAggregatedQuestions = publicAggregatedQuestions
            .Where(q => q.Creator != null && q.Creator.Id == userCacheItem.Id)
            .Select(q => q.Id).ToList();

        return Json(new RequestResult
        {
            success = true,
            data = new
            {
                name = topicCacheItem.Name,
                personalQuestionIds = filteredAggregatedQuestions,
                personalQuestionCount = filteredAggregatedQuestions.Count,
                allQuestionIds = publicAggregatedQuestions.Select(q => q.Id).ToList(),
                allQuestionCount = publicAggregatedQuestions.Count
            }
        });
    }

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public JsonResult Set([FromRoute] int id)
    {
        var topicCacheItem = EntityCache.GetCategory(id);
        if (topicCacheItem == null)
            return Json(new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Default
            });

        if (!_permissionCheck.CanEdit(topicCacheItem))
            return Json(new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Category.MissingRights
            });

        var aggregatedTopics = topicCacheItem.AggregatedCategories(_permissionCheck, false)
            .Where(c => c.Value.Visibility == CategoryVisibility.All);
        var topic = _categoryRepository.GetById(id);
        var pinCount = topic.TotalRelevancePersonalEntries;
        if (!_sessionUser.IsInstallationAdmin)
        {
            if (id == RootCategory.RootCategoryId)
                return Json(new RequestResult
                {
                    success = false,
                    messageKey = FrontendMessageKeys.Error.Category.RootCategoryMustBePublic
                });

            foreach (var c in aggregatedTopics)
            {
                bool childHasPublicParent = c.Value.ParentCategories()
                    .Any(p => p.Visibility == CategoryVisibility.All && p.Id != id);
                if (!childHasPublicParent)
                    return Json(new RequestResult
                    {
                        success = false,
                        messageKey = FrontendMessageKeys.Error.Category.PublicChildCategories
                    });
            }

            if (pinCount >= 10)
            {
                return Json(new RequestResult
                {
                    success = false,
                    messageKey = FrontendMessageKeys.Error.Category.TooPopular
                });
            }
        }

        topicCacheItem.Visibility = CategoryVisibility.Owner;
        topic.Visibility = CategoryVisibility.Owner;
        _categoryRepository.Update(topic, _sessionUser.User, type: CategoryChangeType.Privatized);

        return Json(new RequestResult
        {
            success = true,
            messageKey = FrontendMessageKeys.Success.Category.SetToPrivate
        });
    }

    [HttpGet]
    [AccessOnlyAsLoggedIn]
    public void SetQuestionsToPrivate([FromBody] SetQuestionsToPrivateJson json)
    {
        foreach (var questionId in json.questionIds)
        {
            var questionCacheItem = EntityCache.GetQuestionById(questionId, _httpContextAccessor, _webHostEnvironment);
            var otherUsersHaveQuestionInWuwi =
                questionCacheItem.TotalRelevancePersonalEntries > (questionCacheItem.IsInWishknowledge(_sessionUser.UserId, _sessionUserCache) ? 1 : 0);
            if ((questionCacheItem.Creator.Id == _sessionUser.UserId && !otherUsersHaveQuestionInWuwi) ||
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