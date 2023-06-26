using System.Web.Mvc;
using System.Web.SessionState;

[SessionState(SessionStateBehavior.ReadOnly)]
public class QuestionPinStoreController : Controller
{
    private readonly SessionUser _sessionUser;
    private readonly QuestionInKnowledge _questionInKnowledge;

    public QuestionPinStoreController(SessionUser sessionUser, QuestionInKnowledge questionInKnowledge)
    {
        _sessionUser = sessionUser;
        _questionInKnowledge = questionInKnowledge;
    }

    [HttpPost]
    public JsonResult Pin(int id)
    {
        return Json(new QuestionPinStoreControllerLogic(_questionInKnowledge).Pin(id, _sessionUser));
    }

    [HttpPost]
    public JsonResult Unpin(int id)
    {
        return Json(new QuestionPinStoreControllerLogic(_questionInKnowledge).Unpin(id, _sessionUser));
    }
}