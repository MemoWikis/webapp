using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using TrueOrFalse.Core;
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
        return View(_viewLocation, model);
    }

    public ViewResult Edit(int id)
    {
        var model = new EditQuestionModel(_questionRepository.GetById(id));
        return View(_viewLocation, model);
    }

    [HttpPost]
    public ActionResult Edit(int id, EditQuestionModel model)
    {
        model.FillCategoriesFromPostData(Request.Form);
        _questionRepository.Update(ServiceLocator.Resolve<EditQuestionModel_to_Question>().Update(model, _questionRepository.GetById(id)));
        model.Message = new SuccessMessage("Die Frage wurde gespeichert");

        return View(_viewLocation, model);
    }

    [HttpPost]
    public ActionResult Create(EditQuestionModel model)
    {
        model.FillCategoriesFromPostData(Request.Form);
        var editQuestionModelCategoriesExist = ServiceLocator.Resolve<EditQuestionModel_Categories_Exist>();
        if (editQuestionModelCategoriesExist.Yes(model))
        {
            var question = ServiceLocator.Resolve<EditQuestionModel_to_Question>().Create(model);
            question.Creator = _sessionUser.User;
            _questionRepository.Create(question);
            model.Message = new SuccessMessage("Die Frage wurde gespeichert");
        }
        else
        {
            var missingCategory = editQuestionModelCategoriesExist.MissingCategory;
            model.Message = new ErrorMessage(
                string.Format("Die Kategorie <strong>'{0}'</strong> existiert nicht. " +
                "Klicke <a href=\"{1}\">hier</a>, um Kategorien anzulegen.",
                missingCategory,
                Url.Action("Create", "EditCategory", new { name = missingCategory })));
        }
        return View(_viewLocation, model);
    }
}
