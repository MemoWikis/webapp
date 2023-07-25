using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;


namespace VueApp;

public class VueEditQuestionController : Controller
{
    private readonly SessionUser _sessionUser;
    private readonly LearningSessionCache _learningSessionCache;
    private readonly PermissionCheck _permissionCheck;
    private readonly LearningSessionCreator _learningSessionCreator;
    private readonly QuestionInKnowledge _questionInKnowledge;
    private readonly CategoryValuationRepo _categoryValuationRepo;
    private readonly CategoryRepository _categoryRepository;
    private readonly ImageMetaDataRepo _imageMetaDataRepo;
    private readonly ImageStore _imageStore;
    private readonly SessionUiData _sessionUiData;
    private readonly UserRepo _userRepo;
    private readonly QuestionValuationRepo _questionValuationRepo;
    private readonly QuestionChangeRepo _questionChangeRepo;
    private readonly QuestionWritingRepo _questionWritingRepo;
    private readonly QuestionReadingRepo _questionReadingRepo;

    public VueEditQuestionController(SessionUser sessionUser,
        LearningSessionCache learningSessionCache,
        PermissionCheck permissionCheck,
        LearningSessionCreator learningSessionCreator,
        QuestionInKnowledge questionInKnowledge,
        CategoryValuationRepo categoryValuationRepo,
        CategoryRepository categoryRepository,
        ImageMetaDataRepo imageMetaDataRepo,
        ImageStore imageStore,
        SessionUiData sessionUiData,
        UserRepo userRepo,
        QuestionValuationRepo questionValuationRepo,
        QuestionChangeRepo questionChangeRepo, 
        QuestionWritingRepo questionWritingRepo,
        QuestionReadingRepo questionReadingRepo) 
    {
        _sessionUser = sessionUser;
        _learningSessionCache = learningSessionCache;
        _permissionCheck = permissionCheck;
        _learningSessionCreator = learningSessionCreator;
        _questionInKnowledge = questionInKnowledge;
        _categoryValuationRepo = categoryValuationRepo;
        _categoryRepository = categoryRepository;
        _imageMetaDataRepo = imageMetaDataRepo;
        _imageStore = imageStore;
        _sessionUiData = sessionUiData;
        _userRepo = userRepo;
        _questionValuationRepo = questionValuationRepo;
        _questionChangeRepo = questionChangeRepo;
        _questionWritingRepo = questionWritingRepo;
        _questionReadingRepo = questionReadingRepo;
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult VueCreate(QuestionDataJson questionDataJson)
    {
        if (questionDataJson?.SessionConfig?.CurrentUserId <= 0)
            questionDataJson.SessionConfig.CurrentUserId = _sessionUser.UserId; 
        
        var safeText = GetSafeText(questionDataJson.TextHtml);
        if (safeText.Length <= 0)
            return new JsonResult
            {
                Data = new
                {
                    ErrorMsg = "Fehlender Fragetext",
                }
            };
        var question = new Question();
        var sessionUser = _userRepo.GetById(_sessionUser.UserId);
        question.Creator = sessionUser;
        question = UpdateQuestion(question, questionDataJson, safeText);

        _questionWritingRepo.Create(question, _categoryRepository);

        var questionCacheItem = EntityCache.GetQuestion(question.Id);

        if (questionDataJson.IsLearningTab) { }
        _learningSessionCreator.InsertNewQuestionToLearningSession(questionCacheItem, questionDataJson.SessionIndex, questionDataJson.SessionConfig);

        if (questionDataJson.AddToWishknowledge)
            _questionInKnowledge.Pin(Convert.ToInt32(question.Id), sessionUser.Id);

        return LoadQuestion(question.Id);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult VueEdit(QuestionDataJson questionDataJson)
    {
        var safeText = GetSafeText(questionDataJson.TextHtml);
        if (safeText.Length <= 0)
            return new JsonResult
            {
                Data = new
                {
                    error = true,
                    key = "missingText",
                }
            };

        var question =_questionReadingRepo.GetById(questionDataJson.QuestionId);
        var updatedQuestion = UpdateQuestion(question, questionDataJson, safeText);

        _questionWritingRepo.UpdateOrMerge(updatedQuestion, false);

        if (questionDataJson.IsLearningTab)
            _learningSessionCache.EditQuestionInLearningSession(EntityCache.GetQuestion(updatedQuestion.Id));

        return LoadQuestion(updatedQuestion.Id);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult CreateFlashcard(FlashCardLoader flashCardJson)
    {
        var safeText = GetSafeText(flashCardJson.TextHtml);
        if (safeText.Length <= 0)
            return new JsonResult
            {
                Data = new
                {
                    error = true,
                    key = "missingText",
                }
            };
        var serializer = new JavaScriptSerializer();
        var question = new Question();

        question.TextHtml = flashCardJson.TextHtml;
        question.Text = safeText;
        question.SolutionType = (SolutionType)Enum.Parse(typeof(SolutionType), "9");

        var solutionModelFlashCard = new QuestionSolutionFlashCard();
        solutionModelFlashCard.Text = flashCardJson.Answer;

        if (solutionModelFlashCard.Text.Length <= 0)
            return new JsonResult
            {
                Data = new
                {
                    error = true,
                    key = "missingAnswer",
                }
            };

        question.Solution = serializer.Serialize(solutionModelFlashCard);

        var sessionUser = _userRepo.GetById(_sessionUser.UserId);
        question.Creator =  sessionUser;
        question.Categories = GetAllParentsForQuestion(flashCardJson.CategoryId, question);
        var visibility = (QuestionVisibility)flashCardJson.Visibility;
        question.Visibility = visibility;
        question.License = LicenseQuestionRepo.GetDefaultLicense();

        _questionWritingRepo.Create(question, _categoryRepository);

        if (flashCardJson.AddToWishknowledge)
            _questionInKnowledge.Pin(Convert.ToInt32(question.Id), sessionUser.Id);

        _learningSessionCreator.InsertNewQuestionToLearningSession(EntityCache.GetQuestion(question.Id), flashCardJson.LastIndex, flashCardJson.SessionConfig);
        return LoadQuestion(question.Id);
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
            var serializer = new JavaScriptSerializer();

            var solutionModelFlashCard = new QuestionSolutionFlashCard();
            solutionModelFlashCard.Text = questionDataJson.Solution;
            question.Solution = serializer.Serialize(solutionModelFlashCard);
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

    public JsonResult LoadQuestion(int questionId)
    {
        var user = _sessionUser.User;
        var userQuestionValuation = SessionUserCache.GetItem(user.Id, _categoryValuationRepo, _userRepo, _questionValuationRepo).QuestionValuations;
        var q = EntityCache.GetQuestionById(questionId);
        var question = new QuestionListJson.Question();
        question.Id = q.Id;
        question.Title = q.Text;
        question.LinkToQuestion = Links.GetUrl(q);
        question.ImageData = new ImageFrontendData(_imageMetaDataRepo.GetBy(q.Id, ImageType.Question)).GetImageUrl(40, true).Url;
        question.LinkToQuestion = Links.GetUrl(q);
        question.LinkToQuestionVersions = Links.QuestionHistory(q.Id);
        question.LinkToComment = Links.GetUrl(q) + "#JumpLabel";
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

        return Json(question);
    }

    [HttpGet]
    public int GetCurrentQuestionCount(int topicId) => EntityCache.GetCategory(topicId).GetAggregatedQuestionsFromMemoryCache(_sessionUser.UserId).Count;

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
            question.Text = String.IsNullOrEmpty(Request["Question"]) ? "Temporäre Frage" : Request["Question"];
            question.Solution = "Temporäre Frage";
            var creator = _userRepo.GetById(_sessionUser.UserId);
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

        var imageSettings = new QuestionImageSettings(questionId);

        return new JsonResult
        {
            Data = new
            {
                PreviewUrl = imageSettings.GetUrl_435px().UrlWithoutTime(),
                NewQuestionId = newQuestionId
            }
        };
    }
    //todo: (DaMa) mit Jun schauen scheint nicht benötigt zu werden 
    public ActionResult ReferencePartial(int catId)
    {
        var category = _categoryRepository.GetById(catId);
        return View("Reference", category);
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
                questionCacheItem.TotalRelevancePersonalEntries > (questionCacheItem.IsInWishknowledge(_sessionUser.UserId, _categoryValuationRepo, _userRepo, _questionValuationRepo) ? 1 : 0);
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
    public string GetEditQuestionModal()
    {
        var html = ViewRenderer.RenderPartialView("~/Views/Questions/Edit/EditComponents/EditQuestionTemplateLoader.ascx", null, ControllerContext);
        return html;
    }
}