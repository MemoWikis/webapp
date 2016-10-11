﻿using System;
using System.IO;
using System.Security;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Web;

public class EditQuestionController : BaseController
{
    private readonly QuestionRepo _questionRepo;
    private const string _viewLocation = "~/Views/Questions/Edit/EditQuestion.aspx";
    private const string _viewLocationBody = "~/Views/Questions/Edit/EditSolutionControls/SolutionType{0}.ascx";

    public EditQuestionController(QuestionRepo questionRepo){
        _questionRepo = questionRepo;
    }

    public ActionResult Create(int? categoryId)
    {
        var model = new EditQuestionModel();
        
        if (TempData["createQuestionsMsg"] != null)
            model.Message = (SuccessMessage)TempData["createQuestionsMsg"];

        model.SetToCreateModel();
        if(categoryId != null)
            model.Categories.Add(Resolve<CategoryRepository>().GetById((int)categoryId));

        return View(_viewLocation, model);
    }

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

        if(!_sessionUser.IsInstallationAdmin && model.LicenseId != LicenseQuestionRepo.DefaultLicenseId)
            throw new Exception("Invalid license type");

        if (!ModelState.IsValid)
        {
            model.Message = new ErrorMessage("Bitte überprüfe deine Eingaben.");
            return View(_viewLocation, model);
        }

        if (!IsAllowedTo.ToEdit(question))
            throw new SecurityException("Not allowed to edit question");

        _questionRepo.Update(
            EditQuestionModel_to_Question.Update(model, question, Request.Form)
        );

        UpdateSound(soundfile, id);
        model.Message = new SuccessMessage("Die Frage wurde gespeichert.");

        return View(_viewLocation, model);
    }

    [HttpPost]
    public ActionResult Create(EditQuestionModel model, HttpPostedFileBase soundfile)
    {
        model.FillCategoriesFromPostData(Request.Form);

        if (!ModelState.IsValid)
        {
            model.Message = new ErrorMessage("Bitte überprüfe deine Eingaben.");
            return View(_viewLocation, model);
        }

        Question question;
        if (!String.IsNullOrEmpty(Request["questionId"]) && Request["questionId"] != "-1")
        {
            question = _questionRepo.GetById(Convert.ToInt32(Request["questionId"]));
            _questionRepo.Update(EditQuestionModel_to_Question.Update(model, question, Request.Form));
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

        UpdateSound(soundfile, question.Id);

        if (Request["btnSave"] == "saveAndNew")
        {
            model.Reset();
            model.SetToCreateModel();
            TempData["createQuestionsMsg"] = new SuccessMessage(
                string.Format("Die Frage <i>'{0}'</i> wurde erstellt. Du kannst nun eine <b>neue</b> Frage erstellen.",
                                question.Text.TruncateAtWord(30)));

            return Redirect("Erstelle/");
        }
        
        TempData["createQuestionsMsg"] = new SuccessMessage(
            string.Format("Die Frage <i>'{0}'</i> wurde erstellt. Du kannst sie nun weiter bearbeiten.",
                          question.Text.TruncateAtWord(30)));

        return Redirect("Bearbeite/" + question.Id);
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
        if (questionId == -1)
        {    
            var question = new Question();
            question.Text = String.IsNullOrEmpty(Request["Question"]) ? "Temporäre Frage" : Request["Question"];
            question.Solution = "Temporäre Frage";
            question.Creator = _sessionUser.User;
            question.IsWorkInProgress = true;
            _questionRepo.Create(question);

            newQuestionId = questionId = question.Id;
        }

        if (imageSource == "wikimedia"){
            Resolve<ImageStore>().RunWikimedia<QuestionImageSettings>(
                wikiFileName, questionId, ImageType.Question, _sessionUser.User.Id);
        }

        if (imageSource == "upload"){
            Resolve<ImageStore>().RunUploaded<QuestionImageSettings>(
                _sessionUiData.TmpImagesStore.ByGuid(uploadImageGuid), questionId, _sessionUser.User.Id, uploadImageLicenseOwner);
        }

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
            model = new GetQuestionSolution().Run(question);
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
        
        new StoreSound().Run(soundfile.InputStream, Path.Combine(Server.MapPath("/Sounds/Questions/"), questionId + ".m4a"));
    }
}