using System.Web.Mvc;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Web;
using TrueOrFalse.Frontend.Web.Models;


[HandleError]
public class EditQuestionController : Controller
{
    private readonly QuestionRepository _questionRepository;
    private const string _viewLocation = "~/Views/Questions/Edit/EditQuestion.aspx";

    public EditQuestionController(QuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }
        
    public ActionResult Create()
    {
        var model = new EditQuestionModel();
        model.Answer = "Antwort eingeben";
        model.Question = "Frage eingeben";

        return View(_viewLocation, model);
    }

    [HttpPost]
    public ActionResult Create(EditQuestionModel model)
    {
        ViewData["question"] = model.Question;

        _questionRepository.Create(model.ConvertToQuestion());

        model.Message = new SuccessMessage("Die Frage wurde gespeichert");

        return View(_viewLocation, model);
    }

    public ViewResult Edit(int id)
    {
        var model = new EditQuestionModel(_questionRepository.GetById(id));
        return View(_viewLocation, model);
    }


}
