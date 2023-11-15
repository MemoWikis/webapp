using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using RazorLight;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;



namespace VueApp;

public class VueEditQuestionController
    : Controller
{
    private readonly SessionUser _sessionUser;
    private readonly LearningSessionCache _learningSessionCache;
    private readonly PermissionCheck _permissionCheck;
    private readonly LearningSessionCreator _learningSessionCreator;
    private readonly QuestionInKnowledge _questionInKnowledge;
    private readonly CategoryRepository _categoryRepository;
    private readonly ImageMetaDataReadingRepo _imageMetaDataReadingRepo;
    private readonly ImageStore _imageStore;
    private readonly SessionUiData _sessionUiData;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly QuestionChangeRepo _questionChangeRepo;
    private readonly QuestionWritingRepo _questionWritingRepo;
    private readonly QuestionReadingRepo _questionReadingRepo;
    private readonly SessionUserCache _sessionUserCache;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IActionContextAccessor _actionContextAccessor;

    public VueEditQuestionController(SessionUser sessionUser,
        LearningSessionCache learningSessionCache,
        PermissionCheck permissionCheck,
        LearningSessionCreator learningSessionCreator,
        QuestionInKnowledge questionInKnowledge,
        CategoryRepository categoryRepository,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        ImageStore imageStore,
        SessionUiData sessionUiData,
        UserReadingRepo userReadingRepo,
        QuestionChangeRepo questionChangeRepo,
        QuestionWritingRepo questionWritingRepo,
        QuestionReadingRepo questionReadingRepo,
        SessionUserCache sessionUserCache,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        IActionContextAccessor actionContextAccessor)
    {
        _sessionUser = sessionUser;
        _learningSessionCache = learningSessionCache;
        _permissionCheck = permissionCheck;
        _learningSessionCreator = learningSessionCreator;
        _questionInKnowledge = questionInKnowledge;
        _categoryRepository = categoryRepository;
        _imageMetaDataReadingRepo = imageMetaDataReadingRepo;
        _imageStore = imageStore;
        _sessionUiData = sessionUiData;
        _userReadingRepo = userReadingRepo;
        _questionChangeRepo = questionChangeRepo;
        _questionWritingRepo = questionWritingRepo;
        _questionReadingRepo = questionReadingRepo;
        _sessionUserCache = sessionUserCache;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _actionContextAccessor = actionContextAccessor;
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult VueCreate(QuestionDataJson questionDataJson)
    {
        if (questionDataJson?.SessionConfig?.CurrentUserId <= 0)
            questionDataJson.SessionConfig.CurrentUserId = _sessionUser.UserId; 
        
        var safeText = GetSafeText(questionDataJson.TextHtml);
        if (safeText.Length <= 0)
            return new JsonResult(new
            {
                ErrorMsg = "Fehlender Fragetext",
            });
           
        var question = new Question();
        var sessionUserAsUser = _userReadingRepo.GetById(_sessionUser.UserId);
        question.Creator = sessionUserAsUser;
        question = UpdateQuestion(question, questionDataJson, safeText);

        _questionWritingRepo.Create(question, _categoryRepository);

        var questionCacheItem = EntityCache.GetQuestion(question.Id);

        if (questionDataJson.IsLearningTab) { }
        _learningSessionCreator.InsertNewQuestionToLearningSession(questionCacheItem, questionDataJson.SessionIndex, questionDataJson.SessionConfig);

        if (questionDataJson.AddToWishknowledge)
            _questionInKnowledge.Pin(Convert.ToInt32(question.Id), _sessionUser.UserId);

        return Json(new RequestResult
        {
            success = true,
            data = LoadQuestion(question.Id)
        });
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult VueEdit(QuestionDataJson questionDataJson)
    {
        var safeText = GetSafeText(questionDataJson.TextHtml);
        if (safeText.Length <= 0)
            return new JsonResult(new
            {
                error = true,
                key = "missingText",
            });

        var question = _questionReadingRepo.GetById(questionDataJson.QuestionId);
        var updatedQuestion = UpdateQuestion(question, questionDataJson, safeText);

        _questionWritingRepo.UpdateOrMerge(updatedQuestion, false);

        if (questionDataJson.IsLearningTab)
            _learningSessionCache.EditQuestionInLearningSession(EntityCache.GetQuestion(updatedQuestion.Id));

        return Json(new RequestResult
        {
            success = true,
            data = LoadQuestion(updatedQuestion.Id)
        });
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult CreateFlashcard(FlashCardLoader flashCardJson)
    {
        var safeText = GetSafeText(flashCardJson.TextHtml);
        if (safeText.Length <= 0)
            return Json(new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Question.MissingText
            });

        
        var question = new Question();

        question.TextHtml = flashCardJson.TextHtml;
        question.Text = safeText;
        question.SolutionType = (SolutionType)Enum.Parse(typeof(SolutionType), "9");

        var solutionModelFlashCard = new QuestionSolutionFlashCard();
        solutionModelFlashCard.Text = flashCardJson.Answer;

        if (solutionModelFlashCard.Text.Length <= 0)
            return Json(new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Question.MissingAnswer
            });

        question.Solution = JsonConvert.SerializeObject(solutionModelFlashCard);

        var sessionUserAsUser = _userReadingRepo.GetById(_sessionUser.UserId);
        question.Creator = sessionUserAsUser;
        question.Categories = GetAllParentsForQuestion(flashCardJson.CategoryId, question);
        var visibility = (QuestionVisibility)flashCardJson.Visibility;
        question.Visibility = visibility;
        question.License = LicenseQuestionRepo.GetDefaultLicense();

        _questionWritingRepo.Create(question, _categoryRepository);

        if (flashCardJson.AddToWishknowledge)
            _questionInKnowledge.Pin(Convert.ToInt32(question.Id), _sessionUser.UserId);

        _learningSessionCreator.InsertNewQuestionToLearningSession(EntityCache.GetQuestion(question.Id), flashCardJson.LastIndex, flashCardJson.SessionConfig);
        return Json(new RequestResult
        {
            success = true,
            data = LoadQuestion(question.Id)
        });
    }

    private string GetSafeText(string text)
    {
        return Regex.Replace(text, "<.*?>", "");
    }

    public class FlashCardLoader
    {
        public int CategoryId { get; set; }
        public string TextHtml { get; set; }
        public string Answer { get; set; }
        public int Visibility { get; set; }
        public bool AddToWishknowledge { get; set; }
        public int LastIndex { get; set; }
        public LearningSessionConfig SessionConfig { get; set; }
    }
    private Question UpdateQuestion(Question question, QuestionDataJson questionDataJson, string safeText)
    {
        question.TextHtml = questionDataJson.TextHtml;
        question.Text = safeText;
        question.DescriptionHtml = questionDataJson.DescriptionHtml;
        question.SolutionType = (SolutionType)Enum.Parse(typeof(SolutionType), questionDataJson.SolutionType);

        var preEditedCategoryIds = question.Categories.Select(c => c.Id);
        var newCategoryIds = questionDataJson.CategoryIds.ToList();

        var categoriesToRemove = preEditedCategoryIds.Except(newCategoryIds);

        foreach (var categoryId in categoriesToRemove)
            if (!_permissionCheck.CanViewCategory(categoryId))
                newCategoryIds.Add(categoryId);

        question.Categories = GetAllParentsForQuestion(newCategoryIds, question);
        question.Visibility = (QuestionVisibility)questionDataJson.Visibility;

        if (question.SolutionType == SolutionType.FlashCard)
        {
            var solutionModelFlashCard = new QuestionSolutionFlashCard();
            solutionModelFlashCard.Text = questionDataJson.Solution;
            question.Solution = JsonConvert.SerializeObject(solutionModelFlashCard);
        }
        else
            question.Solution = questionDataJson.Solution;

        question.SolutionMetadataJson = questionDataJson.SolutionMetadataJson;

        if (!String.IsNullOrEmpty(questionDataJson.ReferencesJson))
        {
            var references = ReferenceJson.LoadFromJson(questionDataJson.ReferencesJson, question, _categoryRepository);
            foreach (var reference in references)
            {
                reference.DateCreated = DateTime.Now;
                reference.DateModified = DateTime.Now;
                question.References.Add(reference);
            }
        }

        question.License = _sessionUser.IsInstallationAdmin
            ? LicenseQuestionRepo.GetById(questionDataJson.LicenseId)
            : LicenseQuestionRepo.GetDefaultLicense();
        var questionCacheItem = QuestionCacheItem.ToCacheQuestion(question);
        EntityCache.AddOrUpdate(questionCacheItem); 

        return question;
    }

    public QuestionListJson.Question LoadQuestion(int questionId)
    {
        var user = _sessionUser.User;
        var userQuestionValuation = _sessionUserCache.GetItem(user.Id).QuestionValuations;
        var q = EntityCache.GetQuestionById(questionId);
        var question = new QuestionListJson.Question();
        question.Id = q.Id;
        question.Title = q.Text;

        var links = new Links(_actionContextAccessor, _httpContextAccessor); 
        question.LinkToQuestion = links.GetUrl(q);
        question.ImageData = new ImageFrontendData(_imageMetaDataReadingRepo.GetBy(q.Id, 
                    ImageType.Question),
                _httpContextAccessor,
                _questionReadingRepo)
            .GetImageUrl(40, true)
            .Url;
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

    [HttpGet]
    public int GetCurrentQuestionCount([FromRoute] int topicId) => EntityCache.GetCategory(topicId).GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId).Count;

    public class QuestionDataJson
    {
        public int[] CategoryIds { get; set; }
        public int QuestionId { get; set; }
        public string TextHtml { get; set; }
        public string DescriptionHtml { get; set; }
        public dynamic Solution { get; set; }
        public string SolutionMetadataJson { get; set; }
        public int Visibility { get; set; }
        public string SolutionType { get; set; }
        public bool AddToWishknowledge { get; set; }
        public int SessionIndex { get; set; }
        public int LicenseId { get; set; }
        public string ReferencesJson { get; set; }
        public bool IsLearningTab { get; set; }
        public LearningSessionConfig SessionConfig { get; set; }
    }

    private List<Category> GetAllParentsForQuestion(int newCategoryId, Question question) => GetAllParentsForQuestion(new List<int> { newCategoryId }, question);
    private List<Category> GetAllParentsForQuestion(List<int> newCategoryIds, Question question)
    {
        var categories = new List<Category>();
        var privateCategories = question.Categories.Where(c => !_permissionCheck.CanEdit(c)).ToList();
        categories.AddRange(privateCategories);

        foreach (var categoryId in newCategoryIds)
            categories.Add(_categoryRepository.GetById(categoryId));

        return categories;
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult StoreImage(
        string imageSource,
        int questionId,
        string wikiFileName,
        string uploadImageGuid,
        string uploadImageLicenseOwner,
        string markupEditor
        )
    {
        int newQuestionId = -1;
        Question question;
        if (questionId == -1)
        {
            question = new Question();
            question.Text = string.IsNullOrEmpty(Request.Query["Question"]) ? "Temporäre Frage" : Request.Query["Question"];
            question.Solution = "Temporäre Frage";
            var creator = _userReadingRepo.GetById(_sessionUser.UserId);
            question.Creator = creator;
            question.IsWorkInProgress = true;
            _questionWritingRepo.Create(question, _categoryRepository);

            newQuestionId = questionId = question.Id;
        }
        else
        {
            if (!_permissionCheck.CanEdit(_questionReadingRepo.GetById(questionId)))
                throw new SecurityException("Not allowed to edit question");
        }

        if (imageSource == "wikimedia")
        {
            _imageStore.RunWikimedia<QuestionImageSettings>(
                wikiFileName, questionId, ImageType.Question, _sessionUser.UserId);
        }

        if (imageSource == "upload")
        {
            _imageStore.RunUploaded<QuestionImageSettings>(
                _sessionUiData.TmpImagesStore.ByGuid(uploadImageGuid), questionId, _sessionUser.UserId, uploadImageLicenseOwner);
        }

        question = _questionReadingRepo.GetById(questionId);
        _questionChangeRepo.AddUpdateEntry(question, imageWasChanged: true);

        var imageSettings = new QuestionImageSettings(questionId, _httpContextAccessor, _questionReadingRepo);

        return new JsonResult(new
        {
            PreviewUrl = imageSettings.GetUrl_435px().UrlWithoutTime(),
            NewQuestionId = newQuestionId
        });
    }

    public void PublishQuestions(List<int> questionIds)
    {
        foreach (var questionId in questionIds)
        {
            var questionCacheItem = EntityCache.GetQuestionById(questionId);
            if (questionCacheItem.Creator.Id == _sessionUser.UserId)
            {
                questionCacheItem.Visibility = QuestionVisibility.All;
                EntityCache.AddOrUpdate(questionCacheItem);
                var question = _questionReadingRepo.GetById(questionId);
                question.Visibility = QuestionVisibility.All;
                _questionWritingRepo.UpdateOrMerge(question, false);
            }
        }
    }

    public void SetQuestionsToPrivate(List<int> questionIds)
    {
        foreach (var questionId in questionIds)
        {
            var questionCacheItem = EntityCache.GetQuestionById(questionId);
            var otherUsersHaveQuestionInWuwi =
                questionCacheItem.TotalRelevancePersonalEntries > (questionCacheItem.IsInWishknowledge(_sessionUser.UserId, _sessionUserCache) ? 1 : 0);
            if ((questionCacheItem.Creator.Id == _sessionUser.UserId && !otherUsersHaveQuestionInWuwi) || _sessionUser.IsInstallationAdmin)
            {
                questionCacheItem.Visibility = QuestionVisibility.Owner;
                EntityCache.AddOrUpdate(questionCacheItem);
                var question = _questionReadingRepo.GetById(questionId);
                question.Visibility = QuestionVisibility.Owner;
                _questionWritingRepo.UpdateOrMerge(question, false);
            }
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetEditQuestionModal()
    {
        var engine = new RazorLightEngineBuilder()
            .UseFileSystemProject(Directory.GetCurrentDirectory())
            .UseMemoryCachingProvider()
            .Build();

        string viewPath = "~/Views/Questions/Edit/EditComponents/EditQuestionTemplateLoader.cshtml";
        var html = await engine.CompileRenderAsync(viewPath, new { });

        return Content(html, "text/html");
    }
}