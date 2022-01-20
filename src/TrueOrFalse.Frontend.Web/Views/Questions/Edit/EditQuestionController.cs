using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FluentNHibernate.Data;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

public class EditQuestionController : BaseController
{
    private readonly QuestionRepo _questionRepo;
    private const string _viewLocation = "~/Views/Questions/Edit/EditQuestion.aspx";
    private const string _viewLocationBody = "~/Views/Questions/Edit/EditSolutionControls/SolutionType{0}.ascx";

    public EditQuestionController(QuestionRepo questionRepo){
        _questionRepo = questionRepo;
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult VueCreate(QuestionDataJson questionDataJson)
    {
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
        question.Creator = _sessionUser.User;
        question = UpdateQuestion(question, questionDataJson, safeText);

        _questionRepo.Create(question);

        Sl.QuestionChangeRepo.AddUpdateEntry(question);
        if (questionDataJson.AddToWishknowledge)
            QuestionInKnowledge.Pin(Convert.ToInt32(question.Id), _sessionUser.User);

        if (questionDataJson.IsLearningTab)
            LearningSessionCache.InsertNewQuestionToLearningSession(question, questionDataJson.SessionIndex);

        var questionController = new QuestionController(_questionRepo);

        return questionController.LoadQuestion(question.Id);
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

        var question = EntityCache.GetQuestionById(questionDataJson.QuestionId);
        var updatedQuestion = UpdateQuestion(question, questionDataJson, safeText);

        _questionRepo.Update(updatedQuestion);

        if (questionDataJson.IsLearningTab)
        {
            LearningSessionCache.InsertNewQuestionToLearningSession(updatedQuestion, questionDataJson.SessionIndex);
        }
        var questionController = new QuestionController(_questionRepo);
        return questionController.LoadQuestion(updatedQuestion.Id);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult CreateFlashcard(FlashCardLoader flashCardJson)
    {
        var safeText = GetSafeText(flashCardJson.TextHtml);
        if (safeText.Length <= 0)
            return new JsonResult {
                Data = new
                {
                    error = true,
                    key = "missingText",
                }
            };
        var serializer = new JavaScriptSerializer();
        var question = new Question();
        var questionRepo = Sl.QuestionRepo;

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

        question.Creator = _sessionUser.User;
        question.Categories.Add(Sl.CategoryRepo.GetById(flashCardJson.CategoryId));
        var visibility = (QuestionVisibility)flashCardJson.Visibility;
        question.Visibility = visibility;
        question.License = LicenseQuestionRepo.GetDefaultLicense();

        questionRepo.Create(question);

        Sl.QuestionChangeRepo.AddUpdateEntry(question);

        if (flashCardJson.AddToWishknowledge)
            QuestionInKnowledge.Pin(Convert.ToInt32(question.Id), _sessionUser.User);

        LearningSessionCache.InsertNewQuestionToLearningSession(question, flashCardJson.LastIndex);

        var questionController = new QuestionController(_questionRepo);

        var json = questionController.LoadQuestion(question.Id);

        return json;
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
    }
    private Question UpdateQuestion(Question question, QuestionDataJson questionDataJson, string safeText)
    {
        question.TextHtml = questionDataJson.TextHtml;
        question.Text = safeText;
        question.DescriptionHtml = questionDataJson.DescriptionHtml;
        question.SolutionType = (SolutionType)Enum.Parse(typeof(SolutionType), questionDataJson.SolutionType);

        var categories = new List<Category>();

        foreach (var categoryId in questionDataJson.CategoryIds)
            categories.Add(Sl.CategoryRepo.GetById(categoryId));

        question.Categories = categories;
        question.Visibility = (QuestionVisibility) questionDataJson.Visibility;

        if (question.SolutionType == SolutionType.FlashCard)
        {
            var serializer = new JavaScriptSerializer();

            var solutionModelFlashCard = new QuestionSolutionFlashCard();
            solutionModelFlashCard.Text = questionDataJson.Solution;
            question.Solution = serializer.Serialize(solutionModelFlashCard);
        } else
            question.Solution = questionDataJson.Solution;

        question.SolutionMetadataJson = questionDataJson.SolutionMetadataJson;

        if (!String.IsNullOrEmpty(questionDataJson.ReferencesJson))
        {
            var references = ReferenceJson.LoadFromJson(questionDataJson.ReferencesJson, question);
            foreach (var reference in references)
            {
                reference.DateCreated = DateTime.Now;
                reference.DateModified = DateTime.Now;
                question.References.Add(reference);
            }
        }

        if (questionDataJson.SessionIndex > 0)
        {
            var learningSession = LearningSessionCache.GetLearningSession();
            var step = new LearningSessionStep(question);
            learningSession.Steps.Insert(questionDataJson.SessionIndex, step);
        }

        question.License = IsInstallationAdmin
            ? LicenseQuestionRepo.GetById(questionDataJson.LicenseId)
            : LicenseQuestionRepo.GetDefaultLicense();

        return question;
    }

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
    }

    private bool Validate(EditQuestionModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Message = new ErrorMessage("Bitte überprüfe deine Eingaben.");
            return false;
        }

        if (HttpContext.Request["ConfirmContentRights"] == null && !IsInstallationAdmin)
        {
            Logg.r().Error("Client side validation for Content Rights is not working.");
            model.Message = new ErrorMessage("Bitte bestätige die Hinweise zur Lizensierung und zu den Urheberrechten.");

            return false;
        }

        return true;
    }

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
            question.Creator = _sessionUser.User;
            question.IsWorkInProgress = true;
            _questionRepo.Create(question);

            newQuestionId = questionId = question.Id;
        }
        else
        {
            if (!PermissionCheck.CanEdit(_questionRepo.GetById(questionId)))
                throw new SecurityException("Not allowed to edit question");
        }

        if (imageSource == "wikimedia"){
            Resolve<ImageStore>().RunWikimedia<QuestionImageSettings>(
                wikiFileName, questionId, ImageType.Question, _sessionUser.User.Id);
        }

        if (imageSource == "upload"){
            Resolve<ImageStore>().RunUploaded<QuestionImageSettings>(
                _sessionUiData.TmpImagesStore.ByGuid(uploadImageGuid), questionId, _sessionUser.User.Id, uploadImageLicenseOwner);
        }

        question = Sl.QuestionRepo.GetById(questionId);
        Sl.QuestionChangeRepo.AddUpdateEntry(question, imageWasChanged:true);

        var imageSettings = new QuestionImageSettings(questionId);

        return new JsonResult{
            Data = new{
                PreviewUrl =    imageSettings.GetUrl_435px().UrlWithoutTime(),
                NewQuestionId = newQuestionId
            }
        };
    }

    public ActionResult SolutionEditBody(int? questionId, SolutionType type)
    {
        object model = null;

        if (questionId.HasValue && questionId.Value > 0)
        {
            var question = _questionRepo.GetById(questionId.Value);
            model = GetQuestionSolution.Run(question);
        }

        return View(string.Format(_viewLocationBody, type), model);
    }

    public ActionResult ReferencePartial(int catId)
    {
        var category = R<CategoryRepository>().GetById(catId);
        return View("Reference", category);
    }

    private void UpdateSound(HttpPostedFileBase soundfile, int questionId)
    {
        if (soundfile == null) return;

        if (!PermissionCheck.CanEdit(_questionRepo.GetById(questionId)))
            throw new SecurityException("Not allowed to edit question");

        new StoreSound().Run(soundfile.InputStream, Path.Combine(Server.MapPath("/Sounds/Questions/"), questionId + ".m4a"));
    }

    public void PublishQuestions(List<int> questionIds)
    {
        foreach (var questionId in questionIds)
        {
            var questionCacheItem = EntityCache.GetQuestionById(questionId);
            if (questionCacheItem.Creator == _sessionUser.User)
            {
                questionCacheItem.Visibility = QuestionVisibility.All;
                EntityCache.AddOrUpdate(questionCacheItem);
                var question = Sl.QuestionRepo.GetById(questionId);
                question.Visibility = QuestionVisibility.All;
                _questionRepo.Update(question);
            }
        }
    }

    public void SetQuestionsToPrivate(List<int> questionIds)
    {
        foreach (var questionId in questionIds)
        {
            var questionCacheItem = EntityCache.GetQuestionById(questionId);
            var otherUsersHaveQuestionInWuwi =
                questionCacheItem.TotalRelevancePersonalEntries > (questionCacheItem.IsInWishknowledge() ? 1 : 0);
            if ((questionCacheItem.Creator == _sessionUser.User && !otherUsersHaveQuestionInWuwi) || IsInstallationAdmin)
            {
                questionCacheItem.Visibility = QuestionVisibility.Owner;
                EntityCache.AddOrUpdate(questionCacheItem);
                var question = Sl.QuestionRepo.GetById(questionId);
                question.Visibility = QuestionVisibility.Owner;
                _questionRepo.Update(question);
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