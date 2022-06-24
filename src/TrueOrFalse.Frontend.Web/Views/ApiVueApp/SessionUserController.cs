using System.Web.Mvc;

namespace VueApp;

public class SessionUserController : BaseController
{
    [HttpGet]
    public JsonResult GetLoginState()
    {   
        return Json(new
        {
            isLoggedIn = SessionUser.IsLoggedIn
        }, JsonRequestBehavior.AllowGet);
    }
}