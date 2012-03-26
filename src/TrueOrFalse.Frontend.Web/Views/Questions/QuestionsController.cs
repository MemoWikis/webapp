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

    public ActionResult Questions(int? page, QuestionsModel model)
    {
        //_sessionUiData.QuestionSearchSpec.PageSize = 1;

        if (model.FilterByMe)
        {
            //todo
        }
        if (model.FilterByAll)
        {
            //todo
        }

        if (page.HasValue) _sessionUiData.QuestionSearchSpec.CurrentPage = page.Value;
        return View("Questions", 
                    new QuestionsModel(_questionRepository.GetBy(_sessionUiData.QuestionSearchSpec))
                    {Pager = new PagerModel(_sessionUiData.QuestionSearchSpec)});
    }
}
