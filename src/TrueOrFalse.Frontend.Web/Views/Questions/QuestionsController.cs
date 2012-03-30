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

        if (model.FilterByMe && !model.FilterByAll)
        {
            _sessionUiData.QuestionSearchSpec.Filter.CreatorId.EqualTo(_sessionUser.User.Id);
        }
        else if (!model.FilterByMe && model.FilterByAll)
        {
            _sessionUiData.QuestionSearchSpec.Filter.CreatorId.IsNotEqualTo(_sessionUser.User.Id);
        }
        else
        {
            _sessionUiData.QuestionSearchSpec.Filter.CreatorId.Remove();
        }

        if (page.HasValue) _sessionUiData.QuestionSearchSpec.CurrentPage = page.Value;
        return View("Questions", 
                    new QuestionsModel(_questionRepository.GetBy(_sessionUiData.QuestionSearchSpec))
                    {Pager = new PagerModel(_sessionUiData.QuestionSearchSpec)});
    }
}
