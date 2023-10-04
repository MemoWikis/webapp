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

public class VueEditQuestionController(SessionUser sessionUser,
        LearningSessionCache learningSessionCache,
        PermissionCheck permissionCheck,
        LearningSessionCreator learningSessionCreator,
        QuestionInKnowledge questionInKnowledge,
        CategoryValuationReadingRepo categoryValuationReadingRepo,
        CategoryRepository categoryRepository,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        ImageStore imageStore,
        SessionUiData sessionUiData,
        UserReadingRepo userReadingRepo,
        QuestionValuationReadingRepo questionValuationReadingRepo,
        QuestionChangeRepo questionChangeRepo,
        QuestionWritingRepo questionWritingRepo,
        QuestionReadingRepo questionReadingRepo,
        SessionUserCache sessionUserCache,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        IActionContextAccessor actionContextAccessor)
    : Controller
{
    private readonly CategoryValuationReadingRepo _categoryValuationReadingRepo = categoryValuationReadingRepo;
  

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult VueCreate(QuestionDataJson questionDataJson)
    {
        if (questionDataJson?.SessionConfig?.CurrentUserId <= 0)
            questionDataJson.SessionConfig.CurrentUserId = sessionUser.UserId; 
        
        var safeText = GetSafeText(questionDataJson.TextHtml);
        if (safeText.Length <= 0)
            return new JsonResult(new
            {
                ErrorMsg = "Fehlender Fragetext",
            });
           
        var question = new Question();
        var sessionUserAsUser = userReadingRepo.GetById(sessionUser.UserId);
        question.Creator = sessionUserAsUser;
        question = UpdateQuestion(question, questionDataJson, safeText);

        questionWritingRepo.Create(question, categoryRepository);

        var questionCacheItem = EntityCache.GetQuestion(question.Id);

        if (questionDataJson.IsLearningTab) { }
        learningSessionCreator.InsertNewQuestionToLearningSession(questionCacheItem, questionDataJson.SessionIndex, questionDataJson.SessionConfig);

        if (questionDataJson.AddToWishknowledge)
            questionInKnowledge.Pin(Convert.ToInt32(question.Id), sessionUser.UserId);

        return LoadQuestion(question.Id);
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

        var question =questionReadingRepo.GetById(questionDataJson.QuestionId);
        var updatedQuestion = UpdateQuestion(question, questionDataJson, safeText);

        questionWritingRepo.UpdateOrMerge(updatedQuestion, false);

        if (questionDataJson.IsLearningTab)
            learningSessionCache.EditQuestionInLearningSession(EntityCache.GetQuestion(updatedQuestion.Id));

        return LoadQuestion(updatedQuestion.Id);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult CreateFlashcard(FlashCardLoader flashCardJson)
    {
        var safeText = GetSafeText(flashCardJson.TextHtml);
        if (safeText.Length <= 0)
            return new JsonResult(new
            {
                error = true,
                key = "missingText",
            });
        
        var question = new Question();

        question.TextHtml = flashCardJson.TextHtml;
        question.Text = safeText;
        question.SolutionType = (SolutionType)Enum.Parse(typeof(SolutionType), "9");

        var solutionModelFlashCard = new QuestionSolutionFlashCard();
        solutionModelFlashCard.Text = flashCardJson.Answer;

        if (solutionModelFlashCard.Text.Length <= 0)
            return new JsonResult(new
            {
                error = true,
                key = "missingAnswer",
            });

        question.Solution = JsonConvert.SerializeObject(solutionModelFlashCard);

        var sessionUserAsUser = userReadingRepo.GetById(sessionUser.UserId);
        question.Creator = sessionUserAsUser;
        question.Categories = GetAllParentsForQuestion(flashCardJson.CategoryId, question);
        var visibility = (QuestionVisibility)flashCardJson.Visibility;
        question.Visibility = visibility;
        question.License = LicenseQuestionRepo.GetDefaultLicense();

        questionWritingRepo.Create(question, categoryRepository);

        if (flashCardJson.AddToWishknowledge)
            questionInKnowledge.Pin(Convert.ToInt32(question.Id), sessionUser.UserId);

        learningSessionCreator.InsertNewQuestionToLearningSession(EntityCache.GetQuestion(question.Id), flashCardJson.LastIndex, flashCardJson.SessionConfig);
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
            if (!permissionCheck.CanViewCategory(categoryId))
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
            var references = ReferenceJson.LoadFromJson(questionDataJson.ReferencesJson, question, categoryRepository);
            foreach (var reference in references)
            {
                reference.DateCreated = DateTime.Now;
                reference.DateModified = DateTime.Now;
                question.References.Add(reference);
            }
        }

        question.License = sessionUser.IsInstallationAdmin
            ? LicenseQuestionRepo.GetById(questionDataJson.LicenseId)
            : LicenseQuestionRepo.GetDefaultLicense();
        var questionCacheItem = QuestionCacheItem.ToCacheQuestion(question);
        EntityCache.AddOrUpdate(questionCacheItem); 

        return question;
    }

    public JsonResult LoadQuestion(int questionId)
    {
        var user = sessionUser.User;
        var userQuestionValuation = sessionUserCache.GetItem(user.Id).QuestionValuations;
        var q = EntityCache.GetQuestionById(questionId, httpContextAccessor, webHostEnvironment);
        var question = new QuestionListJson.Question();
        question.Id = q.Id;
        question.Title = q.Text;

        var links = new Links(actionContextAccessor, httpContextAccessor); 
        question.LinkToQuestion = links.GetUrl(q);
        question.ImageData = new ImageFrontendData(imageMetaDataReadingRepo.GetBy(q.Id, 
                    ImageType.Question),
                httpContextAccessor,
                webHostEnvironment,
                questionReadingRepo)
            .GetImageUrl(40, true)
            .Url;
        question.LinkToQuestion = links.GetUrl(q);
        question.LinkToQuestionVersions = links.QuestionHistory(q.Id);
        question.LinkToComment = links.GetUrl(q) + "#JumpLabel";
        question.CorrectnessProbability = q.CorrectnessProbability;
        question.Visibility = q.Visibility;

        var learningSession = learningSessionCache.GetLearningSession();
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
    public int GetCurrentQuestionCount(int topicId) => EntityCache.GetCategory(topicId).GetAggregatedQuestionsFromMemoryCache(sessionUser.UserId).Count;

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
        var privateCategories = question.Categories.Where(c => !permissionCheck.CanEdit(c)).ToList();
        categories.AddRange(privateCategories);

        foreach (var categoryId in newCategoryIds)
            categories.Add(categoryRepository.GetById(categoryId));

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
            var creator = userReadingRepo.GetById(sessionUser.UserId);
            question.Creator = creator;
            question.IsWorkInProgress = true;
            questionWritingRepo.Create(question, categoryRepository);

            newQuestionId = questionId = question.Id;
        }
        else
        {
            if (!permissionCheck.CanEdit(questionReadingRepo.GetById(questionId)))
                throw new SecurityException("Not allowed to edit question");
        }

        if (imageSource == "wikimedia")
        {
            imageStore.RunWikimedia<QuestionImageSettings>(
                wikiFileName, questionId, ImageType.Question, sessionUser.UserId);
        }

        if (imageSource == "upload")
        {
            imageStore.RunUploaded<QuestionImageSettings>(
                sessionUiData.TmpImagesStore.ByGuid(uploadImageGuid), questionId, sessionUser.UserId, uploadImageLicenseOwner);
        }

        question = questionReadingRepo.GetById(questionId);
        questionChangeRepo.AddUpdateEntry(question, imageWasChanged: true);

        var imageSettings = new QuestionImageSettings(questionId, httpContextAccessor, webHostEnvironment, questionReadingRepo);

        return new JsonResult(new
        {
            PreviewUrl = imageSettings.GetUrl_435px().UrlWithoutTime(),
            NewQuestionId = newQuestionId
        });
    }
    //todo: (DaMa) mit Jun schauen scheint nicht benötigt zu werden 
    public ActionResult ReferencePartial(int catId)
    {
        var category = categoryRepository.GetById(catId);
        return View("Reference", category);
    }

    public void PublishQuestions(List<int> questionIds)
    {
        foreach (var questionId in questionIds)
        {
            var questionCacheItem = EntityCache.GetQuestionById(questionId, httpContextAccessor, webHostEnvironment);
            if (questionCacheItem.Creator.Id == sessionUser.UserId)
            {
                questionCacheItem.Visibility = QuestionVisibility.All;
                EntityCache.AddOrUpdate(questionCacheItem);
                var question = questionReadingRepo.GetById(questionId);
                question.Visibility = QuestionVisibility.All;
                questionWritingRepo.UpdateOrMerge(question, false);
            }
        }
    }

    public void SetQuestionsToPrivate(List<int> questionIds)
    {
        foreach (var questionId in questionIds)
        {
            var questionCacheItem = EntityCache.GetQuestionById(questionId, httpContextAccessor, webHostEnvironment);
            var otherUsersHaveQuestionInWuwi =
                questionCacheItem.TotalRelevancePersonalEntries > (questionCacheItem.IsInWishknowledge(sessionUser.UserId,sessionUserCache) ? 1 : 0);
            if ((questionCacheItem.Creator.Id == sessionUser.UserId && !otherUsersHaveQuestionInWuwi) || sessionUser.IsInstallationAdmin)
            {
                questionCacheItem.Visibility = QuestionVisibility.Owner;
                EntityCache.AddOrUpdate(questionCacheItem);
                var question = questionReadingRepo.GetById(questionId);
                question.Visibility = QuestionVisibility.Owner;
                questionWritingRepo.UpdateOrMerge(question, false);
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