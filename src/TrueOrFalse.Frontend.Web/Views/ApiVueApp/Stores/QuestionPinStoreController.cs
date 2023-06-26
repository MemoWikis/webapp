using System.Web.Mvc;
using System.Web.SessionState;

[SessionState(SessionStateBehavior.ReadOnly)]
public class QuestionPinStoreController : Controller
{
    private readonly SessionUser _sessionUser;

    public QuestionPinStoreController(SessionUser sessionUser)
    {
        _sessionUser = sessionUser;
    }

    [HttpPost]
    public JsonResult Pin(int id)
    {
        return Json(new QuestionPinStoreControllerLogic().Pin(id, _sessionUser));
    }

    [HttpPost]
    public JsonResult Unpin(int id)
    {
        return Json(new QuestionPinStoreControllerLogic().Unpin(id, _sessionUser));
    }
}