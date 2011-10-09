using System.Web.Mvc;
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
        model.Answer = "Antwort eingeben";
        model.Question = "Frage eingeben";

        return View(_viewLocation, model);
    }

    [HttpPost]
    public ActionResult Edit(int id, EditQuestionModel model)
    {
        _questionRepository.Update(model.UpdateQuestion(_questionRepository.GetById(id)));
        model.Message = new SuccessMessage("Die Frage wurde gespeichert");

        return View(_viewLocation, model);
    }

    [HttpPost]
    public ActionResult Create(EditQuestionModel model)
    {
        var question = model.ConvertToQuestion();
        question.Creator = _sessionUser.User;
        _questionRepository.Create(question);
        model.Message = new SuccessMessage("Die Frage wurde gespeichert");

        return View(_viewLocation, model);
    }

    public ViewResult Edit(int id)
    {
        var model = new EditQuestionModel(_questionRepository.GetById(id));
        return View(_viewLocation, model);
    }


}
