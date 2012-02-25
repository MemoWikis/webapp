using System.Web.Mvc;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Web.Context;

public class QuestionsController : Controller
{
    private readonly QuestionRepository _questionRepository;
    private readonly SessionUiData _sessionUiData;

    public QuestionsController (QuestionRepository questionRepository, SessionUiData sessionUiData)
    {
        _questionRepository = questionRepository;
        _sessionUiData = sessionUiData;
    }

    public ActionResult Questions(int? page)
    {
        if (page.HasValue) _sessionUiData.QuestionSearchSpec.CurrentPage = page.Value;
        return View(new QuestionsModel(_questionRepository.GetBy(_sessionUiData.QuestionSearchSpec))
                        {Pager = _sessionUiData.QuestionSearchSpec});
    }
}
