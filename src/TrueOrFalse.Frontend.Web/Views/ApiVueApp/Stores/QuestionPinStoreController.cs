using System.Web.Mvc;
using System.Web.SessionState;

[SessionState(SessionStateBehavior.ReadOnly)]
public class QuestionPinStoreController : BaseController
{
    [HttpPost]
    public JsonResult Pin(int id)
    {
        return Json(new QuestionPinStoreControllerLogic().Pin(id));
    }

    [HttpPost]
    public JsonResult Unpin(int id)
    {
        return Json(new QuestionPinStoreControllerLogic().Unpin(id));
    }
}