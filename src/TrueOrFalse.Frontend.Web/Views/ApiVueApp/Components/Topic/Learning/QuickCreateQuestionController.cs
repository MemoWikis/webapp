using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using Seedworks.Web.State;
using TrueOrFalse;

namespace VueApp;
public class QuickCreateQuestionController : Controller
{
    private readonly SessionUser _sessionUser;
    private readonly LearningSessionCreator _learningSessionCreator;
    private readonly QuestionInKnowledge _questionInKnowledge;
    private readonly LearningSessionCache _learningSessionCache;
    private readonly CategoryRepository _categoryRepository;
    private readonly ImageMetaDataReadingRepo _imageMetaDataReadingRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly QuestionWritingRepo _questionWritingRepo;
    private readonly SessionUserCache _sessionUserCache;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly QuestionReadingRepo _questionReadingRepo;
    private readonly PermissionCheck _permissionCheck;
    private readonly ImageStore _imageStore;
    private readonly SessionUiData _sessionUiData;
    private readonly QuestionChangeRepo _questionChangeRepo;

    public QuickCreateQuestionController(SessionUser sessionUser,
        LearningSessionCreator learningSessionCreator,
        QuestionInKnowledge questionInKnowledge,
        LearningSessionCache learningSessionCache,
        CategoryRepository categoryRepository,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        UserReadingRepo userReadingRepo, 
        QuestionWritingRepo questionWritingRepo,
        SessionUserCache sessionUserCache,
        IHttpContextAccessor httpContextAccessor,
        IActionContextAccessor actionContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        QuestionReadingRepo questionReadingRepo, QuestionChangeRepo questionChangeRepo, SessionUiData sessionUiData, ImageStore imageStore, PermissionCheck permissionCheck)
    {
        _sessionUser = sessionUser;
        _learningSessionCreator = learningSessionCreator;
        _questionInKnowledge = questionInKnowledge;
        _learningSessionCache = learningSessionCache;
        _categoryRepository = categoryRepository;
        _imageMetaDataReadingRepo = imageMetaDataReadingRepo;
        _userReadingRepo = userReadingRepo;
        _questionWritingRepo = questionWritingRepo;
        _sessionUserCache = sessionUserCache;
        _httpContextAccessor = httpContextAccessor;
        _actionContextAccessor = actionContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _questionReadingRepo = questionReadingRepo;
        _questionChangeRepo = questionChangeRepo;
        _sessionUiData = sessionUiData;
        _imageStore = imageStore;
        _permissionCheck = permissionCheck;
    }
    public readonly record struct CreateFlashcardParam(
        int TopicId,
        string TextHtml,
        string Answer,
        int Visibility,
        bool AddToWishknowledge,
        int LastIndex,
        LearningSessionConfig SessionConfig
    );
    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult CreateFlashcard([FromBody] CreateFlashcardParam param)
    {
        var safeText = GetSafeText(param.TextHtml);
        if (safeText.Length <= 0)
            return Json(new RequestResult
            {
                success = false,
                data = FrontendMessageKeys.Error.Question.MissingText
            });

        var question = new Question();

        question.TextHtml = param.TextHtml;
        question.Text = safeText;
        question.SolutionType = SolutionType.FlashCard;

        var solutionModelFlashCard = new QuestionSolutionFlashCard();
        solutionModelFlashCard.Text = param.Answer;

        if (solutionModelFlashCard.Text.Length <= 0)
            return Json(new RequestResult
            {
                success = false,
                data = FrontendMessageKeys.Error.Question.MissingAnswer
            });

        question.Solution = JsonConvert.SerializeObject(solutionModelFlashCard);

        question.Creator = _userReadingRepo.GetById(_sessionUser.UserId);
        question.Categories = new List<Category>
        {
            _categoryRepository.GetById(param.TopicId)
        };
        var visibility = (QuestionVisibility)param.Visibility;
        question.Visibility = visibility;
        question.License = LicenseQuestionRepo.GetDefaultLicense();

        _questionWritingRepo.Create(question, _categoryRepository);

        if (param.AddToWishknowledge)
            _questionInKnowledge.Pin(Convert.ToInt32(question.Id), _sessionUser.UserId);

        _learningSessionCreator.InsertNewQuestionToLearningSession(EntityCache.GetQuestion(question.Id), param.LastIndex, param.SessionConfig);
        var questionController = new VueEditQuestionController(_sessionUser,
            _learningSessionCache,
            _permissionCheck,
            _learningSessionCreator,
            _questionInKnowledge,
            _categoryRepository,
            _imageMetaDataReadingRepo,
            _imageStore,
            _sessionUiData,
            _userReadingRepo,
            _questionChangeRepo,
            _questionWritingRepo,
            _questionReadingRepo,
            _sessionUserCache,
            _httpContextAccessor,
            _webHostEnvironment,
            _actionContextAccessor); 

        return Json(new RequestResult
        {
            success = true,
            data = questionController.LoadQuestion(question.Id).SessionIndex
        });
    }

    private string GetSafeText(string text)
    {
        return Regex.Replace(text, "<.*?>", "");
    }
}