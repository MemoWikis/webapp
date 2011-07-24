using System.Web.Mvc;
using TrueOrFalse.Core;

public class QuestionsController : Controller
{
    private readonly QuestionRepository _questionRepository;

    public QuestionsController (QuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }


    public ActionResult Questions()
    {
        return View(new QuestionsModel(_questionRepository.GetAll()));
    }
}
