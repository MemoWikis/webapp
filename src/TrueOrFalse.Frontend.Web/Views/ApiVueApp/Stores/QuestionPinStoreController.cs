using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;

[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class QuestionPinStoreController: BaseController
{
    [HttpPost]
    public JsonResult Pin(int id)
    {
        if (SessionUser.User == null)
            return Json(false);
        QuestionInKnowledge.Pin(id, SessionUser.UserId);
        return Json(true);
    }

    [HttpPost]
    public JsonResult Unpin(int id)
    {
        if (SessionUser.User == null)
            return Json(false);
        QuestionInKnowledge.Unpin(id, SessionUser.UserId);
        return Json(true);
    }
}