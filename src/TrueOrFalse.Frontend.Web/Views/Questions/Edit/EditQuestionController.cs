﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using QuestionListJson;
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

    [SetMainMenu(MainMenuEntry.Questions)]
    public ActionResult Create(int? categoryId)
    {
        var model = new EditQuestionModel();
        
        if (TempData["createQuestionsMsg"] != null)
            model.Message = (SuccessMessage)TempData["createQuestionsMsg"];

        model.SetToCreateModel();

        if (categoryId != null)
        {
                model.Categories.Add(Sl.CategoryRepo.GetByIdEager((int) categoryId));
        }
        return View(_viewLocation, model);
    }

    [SetMainMenu(MainMenuEntry.None)]
    [SetThemeMenu(isQuestionPage: true)]
    public ViewResult Edit(int id)
    {
        var question = _questionRepo.GetById(id);
        _sessionUiData.VisitedQuestions.Add(new QuestionHistoryItem(question, HistoryItemType.Edit));
        var model = new EditQuestionModel(question);

        if (!IsAllowedTo.ToEdit(question))
            throw new SecurityException("Not allowed to edit question");

        model.SetToUpdateModel();
        if (TempData["createQuestionsMsg"] != null){
            model.Message = (SuccessMessage) TempData["createQuestionsMsg"];
        }

        return View(_viewLocation, model);
    }

    [HttpPost]
    [SetMainMenu(MainMenuEntry.None)]
    [SetThemeMenu(isQuestionPage: true)]
    public ActionResult Edit(int id, EditQuestionModel model, HttpPostedFileBase imagefile, HttpPostedFileBase soundfile)
    {
        var question = _questionRepo.GetById(id);
        _sessionUiData.VisitedQuestions.Add(new QuestionHistoryItem(question, HistoryItemType.Edit));

        model.Id = id;
        model.Question = question;
        model.FillCategoriesFromPostData(Request.Form);
        model.FillReferencesFromPostData(Request, question);
        model.SetToUpdateModel();

        DeleteUnusedImages.Run(model.QuestionExtended, id);

        if(!_sessionUser.IsInstallationAdmin && model.LicenseId > 0)
            Logg.r().Warning("Unallowed access to license selection by non-admin");

        if (!IsAllowedTo.ToEdit(question))
            throw new SecurityException("Not allowed to edit question");

        if (!Validate(model))
            return View(_viewLocation, model);

        _questionRepo.Update(
            EditQuestionModel_to_Question.Update(model, question, Request.Form)
        );

        Sl.QuestionChangeRepo.AddUpdateEntry(question);

        UpdateSound(soundfile, id);
        model.Message = new SuccessMessage("Die Frage wurde gespeichert.");

        return View(_viewLocation, model);
    }

    [HttpPost]
    [SetMainMenu(MainMenuEntry.Questions)]
    public ActionResult Create(EditQuestionModel model, HttpPostedFileBase soundfile)
    {
        model.FillCategoriesFromPostData(Request.Form);

        if (!Validate(model))
            return View(_viewLocation, model); ;

        Question question;
        if (!String.IsNullOrEmpty(Request["questionId"]) && Request["questionId"] != "-1")
        {
            question = _questionRepo.GetById(Convert.ToInt32(Request["questionId"]));
            _questionRepo.Update(EditQuestionModel_to_Question.Update(model, question, Request.Form));

            Sl.QuestionChangeRepo.AddUpdateEntry(question);
        }
        else
        {
            question = EditQuestionModel_to_Question.Create(model, Request.Form);
            question.Creator = _sessionUser.User;
            _questionRepo.Create(question);
        }

        var references = model.FillReferencesFromPostData(Request, question);
        foreach (var reference in references){
            reference.DateCreated = DateTime.Now;
            reference.DateModified = DateTime.Now;
            question.References.Add(reference);
        }

        DeleteUnusedImages.Run(model.QuestionExtended, question.Id);

        _questionRepo.Update(question);

        Sl.QuestionChangeRepo.AddUpdateEntry(question);

        UpdateSound(soundfile, question.Id);

        var setLink = (WasInSet: false, SetId: -1);

        if (Request["btnSave"] == "saveAndNew")
        {
            model.Reset();
            model.SetToCreateModel();

            TempData["createQuestionsMsg"] = new SuccessMessage(
                $"Die Frage <i>'{question.Text.TruncateAtWord(30)}'</i> wurde erstellt. Du kannst nun eine <b>neue</b> Frage erstellen.");

            if(setLink.WasInSet)
                return Redirect(Links.CreateQuestion(setId: setLink.SetId));

            return Redirect(Links.CreateQuestion());
        }
        
        TempData["createQuestionsMsg"] = new SuccessMessage(
            $"Die Frage <i>'{question.Text.TruncateAtWord(30)}'</i> wurde erstellt. Du kannst sie nun weiter bearbeiten.");

        return Redirect(Links.EditQuestion(question));
    }

    public JsonResult VueCreate(QuestionDataJson questionDataJson)
    {
        var question = new Question();
        question.Creator = _sessionUser.User;
        question = UpdateQuestion(question, questionDataJson);

        _questionRepo.Create(question);

        Sl.QuestionChangeRepo.AddUpdateEntry(question);
        if (questionDataJson.AddToWishknowledge)
            QuestionInKnowledge.Pin(Convert.ToInt32(question.Id), _sessionUser.User);

        if (questionDataJson.SessionIndex > 0)
            InsertNewQuestionToLearningSession(question, questionDataJson.SessionIndex);

        return LoadQuestion(question.Id);
    }

    [HttpPost]
    public JsonResult VueEdit(QuestionDataJson questionDataJson)
    {
        var question = Sl.QuestionRepo.GetById(questionDataJson.QuestionId);
        question = UpdateQuestion(question, questionDataJson);
            
        _questionRepo.Update(question);
        Sl.QuestionChangeRepo.AddUpdateEntry(question);
        EntityCache.AddOrUpdate(question);

        if (questionDataJson.SessionIndex > 0)
            InsertNewQuestionToLearningSession(question, questionDataJson.SessionIndex);
        

        return LoadQuestion(question.Id);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult CreateFlashcard(FlashCardLoader flashCardJson)
    {
        var serializer = new JavaScriptSerializer();
        var question = new Question();
        var questionRepo = Sl.QuestionRepo;

        question.Text = flashCardJson.Text;
        question.SolutionType = (SolutionType)Enum.Parse(typeof(SolutionType), "9");

        var solutionModelFlashCard = new QuestionSolutionFlashCard();
        solutionModelFlashCard.Text = flashCardJson.Answer;
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

        InsertNewQuestionToLearningSession(question, flashCardJson.LastIndex);

        var json = LoadQuestion(question.Id);

        return json;
    }


    private void InsertNewQuestionToLearningSession(Question question, int sessionIndex)
    {
        var learningSession = LearningSessionCache.GetLearningSession();
        var step = new LearningSessionStep(question);
        learningSession.Steps.Insert(sessionIndex, step);
    }
    public class FlashCardLoader
    {
        public int CategoryId { get; set; }
        public string Text { get; set; }
        public string Answer { get; set; }
        public int Visibility { get; set; }
        public bool AddToWishknowledge { get; set; }
        public int LastIndex { get; set; }
    }
    private JsonResult LoadQuestion(int questionId)
    {
        var user = Sl.R<SessionUser>().User;
        var userQuestionValuation = UserCache.GetItem(user.Id).QuestionValuations;
        var q = EntityCache.GetQuestionById(questionId);
        var question = new QuestionListJson.Question();
        question.Id = q.Id;
        question.Title = q.Text;
        question.LinkToQuestion = Links.GetUrl(q);
        question.ImageData = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(q.Id, ImageType.Question)).GetImageUrl(40, true).Url;
        question.LinkToQuestion = Links.GetUrl(q);
        question.LinkToEditQuestion = Links.EditQuestion(q.Text, q.Id);
        question.LinkToQuestionVersions = Links.QuestionHistory(q.Id);
        question.LinkToComment = Links.GetUrl(q) + "#JumpLabel";
        question.CorrectnessProbability = q.CorrectnessProbability;
        question.Visibility = q.Visibility;

        var learningSession = LearningSessionCache.GetLearningSession();
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

    [HttpPost]
    public JsonResult GetQuestionData(int questionId)
    {
        var question = EntityCache.GetQuestionById(questionId);
        var categoryController = new CategoryController();
        var solution = question.SolutionType == SolutionType.FlashCard ? GetQuestionSolution.Run(question).GetCorrectAnswerAsHtml() : question.Solution;
        var json = new JsonResult
        {
            Data = new
            {
                SolutionType = (int)question.SolutionType,
                Solution = solution,
                SolutionMetadataJson = question.SolutionMetadataJson,
                Text = question.TextHtml,
                TextExtended = question.TextExtendedHtml,
                CategoryIds = question.Categories.Select(c => c.Id).ToList(),
                DescriptionHtml = question.DescriptionHtml,
                Categories = question.Categories.Select(c => categoryController.FillMiniCategoryItem(c)),
                LicenseId = question.LicenseId,
                Visibility = question.Visibility
            }
        };

        return json;
    }

    private Question UpdateQuestion(Question question, QuestionDataJson questionDataJson)
    {
        question.Text = questionDataJson.Text;
        question.SolutionType = (SolutionType)Enum.Parse(typeof(SolutionType), questionDataJson.SolutionType);

        var categories = new List<Category>();
        foreach (var categoryId in questionDataJson.CategoryIds)
            categories.Add(Sl.CategoryRepo.GetById(categoryId));
        question.Categories = categories;

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

        var learningSession = LearningSessionCache.GetLearningSession();
        var step = new LearningSessionStep(question);
        learningSession.Steps.Insert(questionDataJson.SessionIndex, step);

        question.License = Sl.R<SessionUser>().IsInstallationAdmin
            ? LicenseQuestionRepo.GetById(questionDataJson.LicenseId)
            : LicenseQuestionRepo.GetDefaultLicense();
        return question;
    }

    public class QuestionDataJson
    {
        public int[] CategoryIds { get; set; }
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public dynamic Solution { get; set; }
        public string SolutionMetadataJson { get; set; }
        public int Visibility { get; set; }
        public string SolutionType { get; set; }
        public bool AddToWishknowledge { get; set; }
        public int SessionIndex { get; set; }
        public int LicenseId { get; set; }
        public string ReferencesJson { get; set; }
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
            if (!IsAllowedTo.ToEdit(_questionRepo.GetById(questionId)))
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

        if (!IsAllowedTo.ToEdit(_questionRepo.GetById(questionId)))
            throw new SecurityException("Not allowed to edit question");

        new StoreSound().Run(soundfile.InputStream, Path.Combine(Server.MapPath("/Sounds/Questions/"), questionId + ".m4a"));
    }

    public void PublishQuestions(List<int> questionIds)
    {
        foreach (var questionId in questionIds)
        {
            var questionCacheItem = EntityCache.GetQuestionById(questionId);
            if (questionCacheItem.Creator == Sl.SessionUser.User)
            {
                questionCacheItem.Visibility = QuestionVisibility.All;
                EntityCache.AddOrUpdate(questionCacheItem);
                JobExecute.RunAsTask(scope =>
                {
                    var question = Sl.QuestionRepo.GetById(questionId);
                    question.Visibility = QuestionVisibility.All;
                    _questionRepo.Update(question);
                }, "PublishQuestion");
            }
        }
    }
}