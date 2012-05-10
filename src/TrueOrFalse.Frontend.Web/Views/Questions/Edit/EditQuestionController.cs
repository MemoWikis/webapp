using System.Web.Mvc;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Infrastructure;
using TrueOrFalse.Core.Web;
using TrueOrFalse.Core.Web.Context;

[HandleError]
public class EditQuestionController : Controller
{
    private readonly QuestionRepository _questionRepository;
    private readonly SessionUser _sessionUser;
    private const string _viewLocation = "~/Views/Questions/Edit/EditQuestion.aspx";

    public EditQuestionController(QuestionRepository questionRepository,
                                  SessionUser sessionUser)
    {
        _questionRepository = questionRepository;
        _sessionUser = sessionUser;
    }

    public ActionResult Create()
    {
        var model = new EditQuestionModel();
        model.SetToCreateModel();
        return View(_viewLocation, model);
    }

    public ViewResult Edit(int id)
    {
        var model = new EditQuestionModel(_questionRepository.GetById(id));
        model.SetToUpdateModel();
        if (TempData["createQuestionsMsg"] != null){
            model.Message = (SuccessMessage)TempData["createQuestionsMsg"];
        }

        return View(_viewLocation, model);
    }

    [HttpPost]
    public ActionResult Edit(int id, EditQuestionModel model)
    {
        model.FillCategoriesFromPostData(Request.Form);
        model.SetToUpdateModel();
        _questionRepository.Update(ServiceLocator.Resolve<EditQuestionModel_to_Question>().Update(model, _questionRepository.GetById(id)));
        model.Message = new SuccessMessage("Die Frage wurde gespeichert");

        return View(_viewLocation, model);
    }

    [HttpPost]
    public ActionResult Create(EditQuestionModel model)
    {
        model.FillCategoriesFromPostData(Request.Form);
        var resultModel = new EditQuestionModel(new Question());
        resultModel.SetToCreateModel();

        var editQuestionModelCategoriesExist = ServiceLocator.Resolve<EditQuestionModel_Categories_Exist>();
        if (editQuestionModelCategoriesExist.Yes(model))
        {
            var question = ServiceLocator.Resolve<EditQuestionModel_to_Question>().Create(model);
            question.Creator = _sessionUser.User;
            _questionRepository.Create(question);
            resultModel.Message = new SuccessMessage(string.Format("Die Frage: <i>'{0}'</i> wurde erstellt. Nun wird eine <b>neue</b> Frage erstellt.", question.Text.TruncateAtWord(30)));

            if (Request["btnSave"] != null)
            {
                TempData["createQuestionsMsg"] =
                    new SuccessMessage(
                        string.Format("Die Frage: <i>'{0}'</i> wurde erstellt. Du kannst Sie nun weiter bearbeiten.",
                                      question.Text.TruncateAtWord(30)));
                return Redirect("Edit/" + question.Id);
            }
        }
        else
        {
            var missingCategory = editQuestionModelCategoriesExist.MissingCategory;
            resultModel.Message = new ErrorMessage(
                string.Format("Die Kategorie <strong>'{0}'</strong> existiert nicht. " +
                "Klicke <a href=\"{1}\">hier</a>, um Kategorien anzulegen.",
                missingCategory,
                Url.Action("Create", "EditCategory", new { name = missingCategory })));

            View(_viewLocation, model);
        }

        return View(_viewLocation, resultModel);
    }
}
