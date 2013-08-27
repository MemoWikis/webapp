using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

    public ActionResult Create()
    {
        var model = new EditQuestionModel();
        
        if (TempData["createQuestionsMsg"] != null)
            model.Message = (SuccessMessage)TempData["createQuestionsMsg"];

        model.SetToCreateModel();
        return View(_viewLocation, model);
    }

    public ViewResult Edit(int id)
    {
        var model = new EditQuestionModel(_questionRepository.GetById(id));
        model.SetToUpdateModel();
        if (TempData["createQuestionsMsg"] != null){
            model.Message = (SuccessMessage) TempData["createQuestionsMsg"];
        }

        return View(_viewLocation, model);
    }

    [HttpPost]
    public ActionResult Edit(int id, EditQuestionModel model, HttpPostedFileBase imagefile, HttpPostedFileBase soundfile)
    {
        model.Id = id;
        model.FillCategoriesFromPostData(Request.Form);
        model.SetToUpdateModel();
        _questionRepository.Update(
            Resolve<EditQuestionModel_to_Question>()
                .Update(model, _questionRepository.GetById(id), Request.Form)
        );
        UpdateSound(soundfile, id);
        model.Message = new SuccessMessage("Die Frage wurde gespeichert");

        return View(_viewLocation, model);
    }

    [HttpPost]
    public ActionResult Create(EditQuestionModel model, HttpPostedFileBase soundfile)
    {
        model.FillCategoriesFromPostData(Request.Form);
        
        var categoriesExist = Resolve<CategoryNamesExist>();
        if (categoriesExist.No(model.Categories)){
            model.Message = categoriesExist.GetErrorMsg(Url);
            return View(_viewLocation, model);
        }

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

        return Redirect("Edit/" + question.Id);
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
        if (imageSource == "wikimedia")
        {
            Resolve<ImageStore>().RunWikimedia<QuestionImageSettings>(
                wikiFileName, questionId, _sessionUser.User.Id);
        }
        if (imageSource == "upload")
        {
            Resolve<ImageStore>().RunUploaded<QuestionImageSettings>(
                _sessionUiData.TmpImagesStore.ByGuid(Request["ImageGuid"]), questionId, _sessionUser.User.Id, uploadImageLicenceOwner);
        }

        var imageSettings = new QuestionImageSettings(questionId);

        return new JsonResult{
            Data = new{
                PreviewUrl = imageSettings.GetUrl_435px().UrlWithoutTime(),
            }
        };
    }

    public ActionResult SolutionEditBody(int? questionId, SolutionType type)
    {
        object model = null;

        if (questionId.HasValue && questionId.Value > 0)
        {
            var question = _questionRepository.GetById(questionId.Value);
            model = new GetQuestionSolution().Run(type, question.Solution);
        }

        return View(string.Format(_viewLocationBody, type), model);
    }

    private void UpdateSound(HttpPostedFileBase soundfile, int questionId)
    {
        if (soundfile == null) return;
        
        new StoreSound().Run(soundfile.InputStream, Path.Combine(Server.MapPath("/Sounds/Questions/"), questionId + ".m4a"));
    }


}