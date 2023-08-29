using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TrueOrFalse.Frontend.Web.Code;


public class AnswerQuestionDetailsController: Controller
{
    private readonly SessionUser _sessionUser;
    private readonly PermissionCheck _permissionCheck;
    private readonly CategoryValuationReadingRepo _categoryValuationReadingRepo;
    private readonly ImageMetaDataReadingRepo _imageMetaDataReadingRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly QuestionValuationReadingRepo _questionValuationReadingRepo;
    private readonly TotalsPersUserLoader _totalsPersUserLoader;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly SessionUserCache _sessionUserCache;
    private readonly IActionContextAccessor _actionContextAccessor;

    public AnswerQuestionDetailsController(SessionUser sessionUser,
        PermissionCheck permissionCheck,
        CategoryValuationReadingRepo categoryValuationReadingRepo,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        UserReadingRepo userReadingRepo,
        QuestionValuationReadingRepo questionValuationReadingRepo,
        TotalsPersUserLoader totalsPersUserLoader,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        SessionUserCache sessionUserCache,
        IActionContextAccessor actionContextAccessor)
    {
        _sessionUser = sessionUser;
        _permissionCheck = permissionCheck;
        _categoryValuationReadingRepo = categoryValuationReadingRepo;
        _imageMetaDataReadingRepo = imageMetaDataReadingRepo;
        _userReadingRepo = userReadingRepo;
        _questionValuationReadingRepo = questionValuationReadingRepo;
        _totalsPersUserLoader = totalsPersUserLoader;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _sessionUserCache = sessionUserCache;
        _actionContextAccessor = actionContextAccessor;
    }
    [HttpGet]
    public JsonResult Get(int id) => Json(GetData(id));

    public dynamic GetData(int id)
    {
        var question = EntityCache.GetQuestionById(id,
            _httpContextAccessor, _webHostEnvironment );

        if (question.Id == 0 || !_permissionCheck.CanView(question))
            return Json(null);

        var dateNow = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var answerQuestionModel = new AnswerQuestionModel(question, 
            _sessionUser.UserId,
            _totalsPersUserLoader,
            _sessionUserCache);

        var correctnessProbability = answerQuestionModel.HistoryAndProbability.CorrectnessProbability;
        var history = answerQuestionModel.HistoryAndProbability.AnswerHistory;

        var userQuestionValuation = _sessionUser.IsLoggedIn
            ? _sessionUserCache.GetItem(_sessionUser.UserId).QuestionValuations
            : new ConcurrentDictionary<int, QuestionValuationCacheItem>();
        var hasUserValuation = userQuestionValuation.ContainsKey(question.Id) && _sessionUser.IsLoggedIn;

        return new {
            knowledgeStatus = hasUserValuation ? userQuestionValuation[question.Id].KnowledgeStatus : KnowledgeStatus.NotLearned,
            personalProbability = correctnessProbability.CPPersonal,
            personalColor = correctnessProbability.CPPColor,
            avgProbability = correctnessProbability.CPAll,
            personalAnswerCount = history.TimesAnsweredUser,
            personalAnsweredCorrectly = history.TimesAnsweredUserTrue,
            personalAnsweredWrongly = history.TimesAnsweredUserWrong,
            overallAnswerCount = history.TimesAnsweredTotal,
            overallAnsweredCorrectly = history.TimesAnsweredCorrect,
            overallAnsweredWrongly = history.TimesAnsweredWrongTotal,
            isInWishknowledge = answerQuestionModel.HistoryAndProbability.QuestionValuation.IsInWishKnowledge,
            topics = question.CategoriesVisibleToCurrentUser(_permissionCheck).Select(t => new
            {
                Id = t.Id,
                Name = t.Name,
                Url = new Links(_actionContextAccessor, _httpContextAccessor ).CategoryDetail(t.Name, t.Id),
                QuestionCount = t.GetCountQuestionsAggregated(_sessionUser.UserId),
                ImageUrl = new CategoryImageSettings(t.Id, _httpContextAccessor, _webHostEnvironment).GetUrl_128px(asSquare: true).Url,
                IconHtml = CategoryCachedData.GetIconHtml(t),
                MiniImageUrl = new ImageFrontendData(
                        _imageMetaDataReadingRepo.GetBy(t.Id, ImageType.Category),
                        _httpContextAccessor, _webHostEnvironment)
                    .GetImageUrl(30, true, false, ImageType.Category).Url,

                Visibility = (int)t.Visibility,
                IsSpoiler = IsSpoilerCategory.Yes(t.Name, question)
            }).Distinct().ToArray(),

            visibility = question.Visibility,
            dateNow,
            endTimer = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            creator = new
            {
                id = question.CreatorId,
                name = question.Creator.Name
            },
            creationDate = DateTimeUtils.TimeElapsedAsText(question.DateCreated),
            totalViewCount = question.TotalViews,
            wishknowledgeCount = question.TotalRelevancePersonalEntries,
            license = new
            {
                isDefault = question.License.IsDefault(),
                shortText = question.License.DisplayTextShort,
                fullText = question.License.DisplayTextFull
            }
        };
    }
}