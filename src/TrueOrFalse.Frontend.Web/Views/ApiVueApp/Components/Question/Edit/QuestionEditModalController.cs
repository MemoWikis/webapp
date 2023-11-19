using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using TrueOrFalse.Domain;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace VueApp;
public class QuestionEditModalController : BaseController
{
    private readonly LearningSessionCache _learningSessionCache;
    private readonly PermissionCheck _permissionCheck;
    private readonly LearningSessionCreator _learningSessionCreator;
    private readonly QuestionInKnowledge _questionInKnowledge;
    private readonly CategoryRepository _categoryRepository;
    private readonly ImageMetaDataReadingRepo _imageMetaDataReadingRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly QuestionWritingRepo _questionWritingRepo;
    private readonly QuestionReadingRepo _questionReadingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly SessionUserCache _sessionUserCache;
    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly Logg _logg;

    public QuestionEditModalController(SessionUser sessionUser,
        LearningSessionCache learningSessionCache,
        PermissionCheck permissionCheck,
        LearningSessionCreator learningSessionCreator,
        QuestionInKnowledge questionInKnowledge,
        CategoryRepository categoryRepository,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        UserReadingRepo userReadingRepo,
        QuestionWritingRepo questionWritingRepo,
        QuestionReadingRepo questionReadingRepo,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        SessionUserCache sessionUserCache,
        IActionContextAccessor actionContextAccessor,
        Logg logg) 
        : base(sessionUser)
    {
        _learningSessionCache = learningSessionCache;
        _permissionCheck = permissionCheck;
        _learningSessionCreator = learningSessionCreator;
        _questionInKnowledge = questionInKnowledge;
        _categoryRepository = categoryRepository;
        _imageMetaDataReadingRepo = imageMetaDataReadingRepo;
        _userReadingRepo = userReadingRepo;
        _questionWritingRepo = questionWritingRepo;
        _questionReadingRepo = questionReadingRepo;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _sessionUserCache = sessionUserCache;
        _actionContextAccessor = actionContextAccessor;
        _logg = logg;
    }
    public readonly record struct QuestionDataParam(
        int[] CategoryIds,
        int? QuestionId,
        string TextHtml,
        string DescriptionHtml,
        string Solution,
        string SolutionMetadataJson,
        int Visibility,
        SolutionType SolutionType,
        bool? AddToWishknowledge,
        int SessionIndex,
        int LicenseId,
        string ReferencesJson,
        bool IsLearningTab,
        LearningSessionConfig SessionConfig
    );

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult Create([FromBody] QuestionDataParam param)
    {
        var sessionUser = _sessionUser;
        if (!new LimitCheck(_httpContextAccessor, _webHostEnvironment, _logg, _sessionUser).CanSavePrivateQuestion(
                logExceedance: true))
        {
            return Json(new RequestResult
                { success = false, messageKey = FrontendMessageKeys.Error.Subscription.CantSavePrivateQuestion });
        }

        var safeText = RemoveHtmlTags(param.TextHtml);
        if (safeText.Length <= 0)
        {
            return Json(new RequestResult
                { success = false, messageKey = FrontendMessageKeys.Error.Question.MissingText });
        }

        var question = new Question();
        question.Creator = _userReadingRepo.GetById(_sessionUser.UserId);
        question = UpdateQuestion(question, param, safeText);

        _questionWritingRepo.Create(question, _categoryRepository);

        var questionCacheItem = EntityCache.GetQuestion(question.Id);

        if (param.IsLearningTab)
        {
        }

        _learningSessionCreator.InsertNewQuestionToLearningSession(questionCacheItem, param.SessionIndex,
            param.SessionConfig);

        if (param.AddToWishknowledge != null && (bool)param.AddToWishknowledge)
            _questionInKnowledge.Pin(Convert.ToInt32(question.Id), _sessionUser.UserId);

        return Json(new RequestResult { success = true, data = LoadQuestion(question.Id) });
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult Edit([FromBody] QuestionDataParam param)
    {
        var safeText = RemoveHtmlTags(param.TextHtml);
        if (safeText.Length <= 0)
        {
            return Json(new RequestResult
                { success = false, messageKey = FrontendMessageKeys.Error.Question.MissingText });
        }

        if (param.QuestionId == null)
            return Json(new RequestResult { success = false, messageKey = FrontendMessageKeys.Error.Default });

        var question = _questionReadingRepo.GetById((int)param.QuestionId);
        var updatedQuestion = UpdateQuestion(question, param, safeText);

        _questionWritingRepo.UpdateOrMerge(updatedQuestion, false);

        if (param.IsLearningTab)
            _learningSessionCache.EditQuestionInLearningSession(EntityCache.GetQuestion(updatedQuestion.Id));

        return Json(new RequestResult { success = true, data = LoadQuestion(updatedQuestion.Id) });

    }


    [HttpGet]
    public JsonResult GetData([FromRoute] int id)
    {
        var question = EntityCache.GetQuestionById(id);
        var solution = question.SolutionType == SolutionType.FlashCard
            ? GetQuestionSolution.Run(question).GetCorrectAnswerAsHtml()
            : question.Solution;
        var topicsVisibleToCurrentUser =
            question.Categories.Where(_permissionCheck.CanView).Distinct();

        return Json(new
        {
            Id = id,
            SolutionType = (int)question.SolutionType,
            Solution = solution,
            SolutionMetadataJson = question.SolutionMetadataJson,
            Text = question.TextHtml,
            TextExtended = question.TextExtendedHtml,
            PublicTopicIds = topicsVisibleToCurrentUser.Select(t => t.Id).ToArray(),
            DescriptionHtml = question.DescriptionHtml,
            Topics = topicsVisibleToCurrentUser.Select(t => FillMiniTopicItem(t)).ToArray(),
            TopicIds = topicsVisibleToCurrentUser.Select(t => t.Id).ToArray(),
            LicenseId = question.LicenseId,
            Visibility = question.Visibility,
        });
    }

    [HttpGet]
    public int GetCurrentQuestionCount([FromRoute] int id) => EntityCache.GetCategory(id).GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId).Count;


    private dynamic LoadQuestion(int questionId)
    {
        var user = _sessionUser.User;
        var userQuestionValuation = _sessionUserCache.GetItem(user.Id).QuestionValuations;
        var q = EntityCache.GetQuestionById(questionId);
        var question = new QuestionListJson.Question();
        question.Id = q.Id;
        question.Title = q.Text;
        question.LinkToQuestion = new Links(_actionContextAccessor, _httpContextAccessor).GetUrl(q);
        question.ImageData = new ImageFrontendData(_imageMetaDataReadingRepo.GetBy(q.Id, ImageType.Question),
                _httpContextAccessor,
                _questionReadingRepo)
            .GetImageUrl(40, true)
            .Url;

        var links = new Links(_actionContextAccessor, _httpContextAccessor);
        question.LinkToQuestion = links.GetUrl(q);
        question.LinkToQuestionVersions = links.QuestionHistory(q.Id);
        question.LinkToComment = links.GetUrl(q) + "#JumpLabel";
        question.CorrectnessProbability = q.CorrectnessProbability;
        question.Visibility = q.Visibility;

        var learningSession = _learningSessionCache.GetLearningSession();
        if (learningSession != null)
        {
            var steps = learningSession.Steps;
            var index = steps.IndexOf(s => s.Question.Id == q.Id);
            question.SessionIndex = index;
        }

        if (userQuestionValuation.ContainsKey(q.Id) && user != null)
        {
            question.CorrectnessProbability = userQuestionValuation[q.Id].CorrectnessProbability;
            question.IsInWishknowledge = userQuestionValuation[q.Id].IsInWishKnowledge;
            question.HasPersonalAnswer = userQuestionValuation[q.Id].CorrectnessProbabilityAnswerCount > 0;
        }

        return question;
    }

    private SearchTopicItem FillMiniTopicItem(CategoryCacheItem topic)
    {
        var miniTopicItem = new SearchTopicItem
        {
            Id = topic.Id,
            Name = topic.Name,
            QuestionCount = topic.GetCountQuestionsAggregated(_sessionUser.UserId),
            ImageUrl = new CategoryImageSettings(topic.Id, _httpContextAccessor)
                .GetUrl_128px(asSquare: true).Url,
            MiniImageUrl = new ImageFrontendData(_imageMetaDataReadingRepo.GetBy(topic.Id, ImageType.Category),
                    _httpContextAccessor,
                    _questionReadingRepo)
                .GetImageUrl(30, true, false, ImageType.Category)
                .Url,
            Visibility = (int)topic.Visibility
        };

        return miniTopicItem;
    }

    private string RemoveHtmlTags(string text)
    {
        return Regex.Replace(text, "<.*?>", "");
    }

    private Question UpdateQuestion(Question question, QuestionDataParam param, string safeText)
    {
        question.TextHtml = param.TextHtml;
        question.Text = safeText;
        question.DescriptionHtml = param.DescriptionHtml;
        question.SolutionType = param.SolutionType;

        var preEditedCategoryIds = question.Categories.Select(c => c.Id);
        var newCategoryIds = param.CategoryIds.ToList();

        var categoriesToRemove = preEditedCategoryIds.Except(newCategoryIds);

        foreach (var categoryId in categoriesToRemove)
            if (!_permissionCheck.CanViewCategory(categoryId))
                newCategoryIds.Add(categoryId);

        question.Categories = GetAllParentsForQuestion(newCategoryIds, question);
        question.Visibility = (QuestionVisibility)param.Visibility;

        if (question.SolutionType == SolutionType.FlashCard)
        {
            var solutionModelFlashCard = new QuestionSolutionFlashCard();
            solutionModelFlashCard.Text = param.Solution;
            question.Solution = JsonConvert.SerializeObject(solutionModelFlashCard);
        }
        else
            question.Solution = param.Solution;

        question.SolutionMetadataJson = param.SolutionMetadataJson;

        if (!String.IsNullOrEmpty(param.ReferencesJson))
        {
            var references =
                ReferenceJson.LoadFromJson(param.ReferencesJson, question, _categoryRepository);
            foreach (var reference in references)
            {
                reference.DateCreated = DateTime.Now;
                reference.DateModified = DateTime.Now;
                question.References.Add(reference);
            }
        }

        question.License = _sessionUser.IsInstallationAdmin
            ? LicenseQuestionRepo.GetById(param.LicenseId)
            : LicenseQuestionRepo.GetDefaultLicense();
        var questionCacheItem = QuestionCacheItem.ToCacheQuestion(question);
        EntityCache.AddOrUpdate(questionCacheItem);

        return question;
    }

    private List<Category> GetAllParentsForQuestion(List<int> newCategoryIds, Question question)
    {
        var topics = new List<Category>();
        var privateTopics = question.Categories.Where(c => !_permissionCheck.CanEdit(c)).ToList();
        topics.AddRange(privateTopics);

        foreach (var categoryId in newCategoryIds)
            topics.Add(_categoryRepository.GetById(categoryId));

        return topics;
    }
}
