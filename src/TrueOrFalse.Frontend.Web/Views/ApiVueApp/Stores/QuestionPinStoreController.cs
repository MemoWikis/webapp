using Microsoft.AspNetCore.Mvc;

public class QuestionPinStoreController : BaseController
{
    private readonly QuestionPinStoreControllerLogic _questionPinStoreControllerLogic;

    public QuestionPinStoreController(SessionUser sessionUser,
        QuestionPinStoreControllerLogic questionPinStoreControllerLogic) : base(sessionUser)
    {
        _questionPinStoreControllerLogic = questionPinStoreControllerLogic;
    }

    [HttpPost]
    public JsonResult Pin([FromRoute] int id)
    {
        return Json(_questionPinStoreControllerLogic.Pin(id, _sessionUser));
    }

    [HttpPost]
    public JsonResult Unpin([FromRoute] int id)
    {
        return Json(_questionPinStoreControllerLogic.Unpin(id, _sessionUser));
    }
}