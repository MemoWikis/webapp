using System.Collections.Concurrent;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TrueOrFalse.Frontend.Web.Code;

public class TopicLearningQuestionListController: BaseController
{
    private readonly LearningSessionCreator _learningSessionCreator;
    private readonly LearningSessionCache _learningSessionCache;
    private readonly CategoryValuationReadingRepo _categoryValuationReadingRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly QuestionValuationReadingRepo _questionValuationReadingRepo;
    private readonly SessionUserCache _sessionUserCache;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly QuestionReadingRepo _questionReadingRepo;
    private readonly ImageMetaDataReadingRepo _imageMetaDataReadingRepo;

    public TopicLearningQuestionListController(SessionUser sessionUser,
        LearningSessionCreator learningSessionCreator,
        LearningSessionCache learningSessionCache,
        CategoryValuationReadingRepo categoryValuationReadingRepo, 
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        UserReadingRepo userReadingRepo,
        QuestionValuationReadingRepo questionValuationReadingRepo,
        SessionUserCache sessionUserCache,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        IActionContextAccessor actionContextAccessor,
        QuestionReadingRepo questionReadingRepo) : base(sessionUser)
    {
        _sessionUser = sessionUser;
        _learningSessionCreator = learningSessionCreator;
        _learningSessionCache = learningSessionCache;
        _categoryValuationReadingRepo = categoryValuationReadingRepo;
        _userReadingRepo = userReadingRepo;
        _questionValuationReadingRepo = questionValuationReadingRepo;
        _sessionUserCache = sessionUserCache;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _actionContextAccessor = actionContextAccessor;
        _questionReadingRepo = questionReadingRepo;
        _imageMetaDataReadingRepo = imageMetaDataReadingRepo;
    }

    public readonly record struct LoadQuestionsJson(int ItemCountPerPage, int PageNumber, int TopicId);
    [HttpPost]
    public JsonResult LoadQuestions([FromBody] LoadQuestionsJson json)
    {
        if (_learningSessionCache.GetLearningSession() == null || json.TopicId != _learningSessionCache.GetLearningSession()?.Config.CategoryId)
            _learningSessionCreator.LoadDefaultSessionIntoCache(json.TopicId, _sessionUser.UserId);

        return Json(new QuestionListModel(_learningSessionCache,
                _sessionUser,
                _categoryValuationReadingRepo, 
                _imageMetaDataReadingRepo, 
                _userReadingRepo, 
                _questionValuationReadingRepo,
                _sessionUserCache,
                _actionContextAccessor,
                _httpContextAccessor,
                _webHostEnvironment,
                _questionReadingRepo)
            .PopulateQuestionsOnPage(json.PageNumber, json.ItemCountPerPage));
    }

    [HttpGet]
    public JsonResult LoadNewQuestion([FromRoute] int id)
    {
        var index = id;
        var session = _learningSessionCache.GetLearningSession();
        if (session == null)
            return Json(new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Default
            });
        var steps = session.Steps;

        var question = steps[index].Question;

        var userQuestionValuation = _sessionUser.IsLoggedIn
            ? _sessionUserCache.GetItem(_sessionUser.UserId)?.QuestionValuations
            : new ConcurrentDictionary<int, QuestionValuationCacheItem>();

        var hasUserValuation = userQuestionValuation != null && userQuestionValuation.ContainsKey(question.Id) && _sessionUser.IsLoggedIn;

        return Json(new RequestResult
        {
            success = true,
            data = new
            {
                Id = question.Id,
                Title = question.Text,
                LinkToQuestion = new Links(_actionContextAccessor, _httpContextAccessor).GetUrl(question),
                ImageData = new ImageFrontendData(_imageMetaDataReadingRepo.GetBy(question.Id, ImageType.Question),
                        _httpContextAccessor,
                        _questionReadingRepo)
                    .GetImageUrl(40, true)
                    .Url,
                LearningSessionStepCount = steps.Count,
                LinkToQuestionVersions = new Links(_actionContextAccessor, _httpContextAccessor).QuestionHistory(question.Id),
                LinkToComment = new Links(_actionContextAccessor, _httpContextAccessor).GetUrl(question) + "#JumpLabel",
                CorrectnessProbability = hasUserValuation ? userQuestionValuation[question.Id].CorrectnessProbability : question.CorrectnessProbability,
                KnowledgeStatus = hasUserValuation ? userQuestionValuation[question.Id].KnowledgeStatus : KnowledgeStatus.NotLearned,
                Visibility = question.Visibility,
                SessionIndex = index,
                IsInWishknowledge = hasUserValuation && userQuestionValuation[question.Id].IsInWishKnowledge,
                HasPersonalAnswer = false
            }
        });
    }
}
