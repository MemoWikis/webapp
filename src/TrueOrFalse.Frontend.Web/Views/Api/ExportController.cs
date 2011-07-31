using System.Web.Mvc;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Web.Context;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.View.Web.Views.Api;


public class ExportController : Controller
{
    private readonly QuestionRepository _questionRepository;

    public ExportController(QuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public ActionResult Questions()
    {
        var viewLocation = "~/Views/Api/ExportQuestions.aspx";
        var model = new ExportQuestionsModel(_questionRepository.GetAll());
        return View(viewLocation, model);
    }
}