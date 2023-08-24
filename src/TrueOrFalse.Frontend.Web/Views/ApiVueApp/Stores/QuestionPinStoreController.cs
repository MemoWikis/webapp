using Microsoft.AspNetCore.Mvc;

public class QuestionPinStoreController : Controller
{
    private readonly SessionUser _sessionUser;
    private readonly QuestionPinStoreControllerLogic _questionPinStoreControllerLogic;

    public QuestionPinStoreController(SessionUser sessionUser,
        QuestionPinStoreControllerLogic questionPinStoreControllerLogic )
    {
        _sessionUser = sessionUser;
        _questionPinStoreControllerLogic = questionPinStoreControllerLogic;
    }

    [HttpPost]
    public JsonResult Pin(int id)
    {
        return Json(_questionPinStoreControllerLogic.Pin(id, _sessionUser));
    }

    [HttpPost]
    public JsonResult Unpin(int id)
    {
        return Json(_questionPinStoreControllerLogic.Unpin(id, _sessionUser));
    }
}