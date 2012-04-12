using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Web.Context;

public class QuestionsController : Controller
{
    private readonly QuestionRepository _questionRepository;
    private readonly SessionUiData _sessionUiData;
    private readonly SessionUser _sessionUser;

    public QuestionsController (QuestionRepository questionRepository, SessionUiData sessionUiData, SessionUser sessionUser)
    {
        _questionRepository = questionRepository;
        _sessionUiData = sessionUiData;
        _sessionUser = sessionUser;
    }

    public ActionResult Questions(int? page, QuestionsModel model)
    {
        //_sessionUiData.QuestionSearchSpec.PageSize = 1;

        _sessionUiData.QuestionSearchSpec.SetFilterByMe(model.FilterByMe);
        _sessionUiData.QuestionSearchSpec.SetFilterByAll(model.FilterByAll);
        _sessionUiData.QuestionSearchSpec.SetFilterByUsers(
             from key in Request.Form.AllKeys
             where key.StartsWith("usr")
             select int.Parse(Request.Form[key]));

        model.FilterByMe = _sessionUiData.QuestionSearchSpec.FilterByMe;
        model.FilterByAll = _sessionUiData.QuestionSearchSpec.FilterByAll;

        if (page.HasValue) _sessionUiData.QuestionSearchSpec.CurrentPage = page.Value;
        return View("Questions", 
                    new QuestionsModel(_questionRepository.GetBy(_sessionUiData.QuestionSearchSpec))
                    {Pager = new PagerModel(_sessionUiData.QuestionSearchSpec),
                     FilterByMe = model.FilterByMe,
                     FilterByAll = model.FilterByAll});
    }
}
