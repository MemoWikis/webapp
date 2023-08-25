using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class PublishTopicStoreController : Controller
{
    private readonly SessionUser _sessionUser;
    private readonly PermissionCheck _permissionCheck;
    private readonly CategoryValuationReadingRepo _categoryValuationReadingRepo;
    private readonly CategoryRepository _categoryRepository;
    private readonly QuestionReadingRepo _questionReadingRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly QuestionValuationRepo _questionValuationRepo;
    private readonly QuestionWritingRepo _questionWritingRepo;
    private readonly SessionUserCache _sessionUserCache;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PublishTopicStoreController(SessionUser sessionUser,
        PermissionCheck permissionCheck,
        CategoryValuationReadingRepo categoryValuationReadingRepo,
        CategoryRepository categoryRepository,
        QuestionReadingRepo questionReadingRepo,
        UserReadingRepo userReadingRepo,
        QuestionValuationRepo questionValuationRepo,
        QuestionWritingRepo questionWritingRepo,
        SessionUserCache sessionUserCache,
        IWebHostEnvironment webHostEnvironment,
        IHttpContextAccessor httpContextAccessor)
    {
        _sessionUser = sessionUser;
        _permissionCheck = permissionCheck;
        _categoryValuationReadingRepo = categoryValuationReadingRepo;
        _categoryRepository = categoryRepository;
        _questionReadingRepo = questionReadingRepo;
        _userReadingRepo = userReadingRepo;
        _questionValuationRepo = questionValuationRepo;
        _questionWritingRepo = questionWritingRepo;
        _sessionUserCache = sessionUserCache;
        _webHostEnvironment = webHostEnvironment;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public JsonResult PublishTopic(int topicId)
    {
        var topicCacheItem = EntityCache.GetCategory(topicId);

        if (topicCacheItem.HasPublicParent() || topicCacheItem.Creator.StartTopicId == topicId)
        {
            if (topicCacheItem.ParentCategories(true).Any(c => c.Id == 1) && !_sessionUser.IsInstallationAdmin)
                return Json(new
                {
                    success = false,
                    key = "parentIsRoot"
                });

            
            topicCacheItem.Visibility = CategoryVisibility.All;
            var topic = _categoryRepository.GetById(topicId);
            topic.Visibility = CategoryVisibility.All;
            _categoryRepository.Update(topic, _sessionUser.User, type: CategoryChangeType.Published);

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
            var questionCacheItem = EntityCache.GetQuestionById(questionId, _httpContextAccessor, _webHostEnvironment);
            if (questionCacheItem.Creator.Id == _sessionUser.User.Id)
            {
                questionCacheItem.Visibility = QuestionVisibility.All;
                EntityCache.AddOrUpdate(questionCacheItem, _httpContextAccessor, _webHostEnvironment);
                var question = _questionReadingRepo.GetById(questionId);
                question.Visibility = QuestionVisibility.All;
                _questionWritingRepo.UpdateOrMerge(question, false);
            }
        }
    }

    [HttpGet]
    [AccessOnlyAsLoggedIn]
    public JsonResult Get(int topicId)
    {
        var topicCacheItem = EntityCache.GetCategory(topicId);
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