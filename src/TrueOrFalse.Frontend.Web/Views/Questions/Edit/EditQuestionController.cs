using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Mapping;
using TrueOrFalse;
using TrueOrFalse.Infrastructure;
using TrueOrFalse.Web;
using TrueOrFalse.Web.Context;

[HandleError]
public class EditQuestionController : BaseController
{
    private readonly QuestionRepository _questionRepository;
    private const string _viewLocation = "~/Views/Questions/Edit/EditQuestion.aspx";
    private const string _viewLocationBody = "~/Views/Questions/Edit/EditSolutionControls/SolutionType{0}.ascx";

    public EditQuestionController(QuestionRepository questionRepository){
        _questionRepository = questionRepository;
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
        var question = _questionRepository.GetById(id);
        _sessionUiData.VisitedQuestions.Add(new QuestionHistoryItem(question, HistoryItemType.Edit));
        var model = new EditQuestionModel(question);
        
        model.SetToUpdateModel();
        if (TempData["createQuestionsMsg"] != null){
            model.Message = (SuccessMessage) TempData["createQuestionsMsg"];
        }

        return View(_viewLocation, model);
    }

    [HttpPost]
    public ActionResult Edit(int id, EditQuestionModel model, HttpPostedFileBase imagefile, HttpPostedFileBase soundfile)
    {
        var question = _questionRepository.GetById(id);
        _sessionUiData.VisitedQuestions.Add(new QuestionHistoryItem(question, HistoryItemType.Edit));

        model.Id = id;
        model.FillCategoriesFromPostData(Request.Form);
        model.SetToUpdateModel();
        _questionRepository.Update(
            Resolve<EditQuestionModel_to_Question>()
                .Update(model, question, Request.Form)
        );
        UpdateSound(soundfile, id);
        model.Message = new SuccessMessage("Die Frage wurde gespeichert");

        return View(_viewLocation, model);
    }

    [HttpPost]
    public ActionResult Create(EditQuestionModel model, HttpPostedFileBase soundfile)
    {
        model.FillCategoriesFromPostData(Request.Form);
        
        var question = Resolve<EditQuestionModel_to_Question>().Create(model, Request.Form);

        question.Creator = _sessionUser.User;
        _questionRepository.Create(question);
        
        UpdateSound(soundfile, question.Id);

        if (Request["btnSave"] == "saveAndNew")
        {
            model.Reset();
            model.SetToCreateModel();
            TempData["createQuestionsMsg"] = new SuccessMessage(
                string.Format("Die Frage: <i>'{0}'</i> wurde erstellt. Du kannst nun eine <b>neue</b> Frage erstellen.",
                                question.Text.TruncateAtWord(30)));

            return Redirect("Create/");
        }
        
        TempData["createQuestionsMsg"] = new SuccessMessage(
            string.Format("Die Frage: <i>'{0}'</i> wurde erstellt. Du kannst Sie nun weiter bearbeiten.",
                          question.Text.TruncateAtWord(30)));

        return Redirect("Bearbeite/" + question.Id);
    }

    [HttpPost]
    public JsonResult StoreImage(
        string imageSource,
        int questionId,
        string wikiFileName,
        string uploadImageGuid,
        string uploadImageLicenceOwner,
        string markupEditor
        )
    {

        int newQuestionId = -1;
        if (questionId == -1){
            var question = new Question();
            question.Text = Request["Question"];
            question.Creator = _sessionUser.User;
            _questionRepository.Create(question);

            newQuestionId = questionId = question.Id;
        }

        if (imageSource == "wikimedia"){
            Resolve<ImageStore>().RunWikimedia<QuestionImageSettings>(
                wikiFileName, questionId, ImageType.Question, _sessionUser.User.Id);
        }
        
        if (imageSource == "upload"){
            Resolve<ImageStore>().RunUploaded<QuestionImageSettings>(
                _sessionUiData.TmpImagesStore.ByGuid(Request["ImageGuid"]), questionId, _sessionUser.User.Id, uploadImageLicenceOwner);
        }

        var imageSettings = new QuestionImageSettings(questionId);

        return new JsonResult{
            Data = new{
                PreviewUrl = imageSettings.GetUrl_435px().UrlWithoutTime(),
                NewQuestionId = newQuestionId
            }
        };
    }

    public ActionResult SolutionEditBody(int? questionId, SolutionType type)
    {
        object model = null;

        if (questionId.HasValue && questionId.Value > 0)
        {
            var question = _questionRepository.GetById(questionId.Value);
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