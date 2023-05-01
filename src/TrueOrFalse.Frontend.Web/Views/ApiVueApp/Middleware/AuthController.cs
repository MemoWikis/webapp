using System.Web.Mvc;

namespace VueApp;

public class MiddlewareAuthController : BaseController
{
    [AccessOnlyAsLoggedIn]
    [AccessOnlyAsAdmin]
    [HttpGet]
    public JsonResult Get()
    {
        return Json(SessionUser.IsInstallationAdmin, JsonRequestBehavior.AllowGet);
    }
}   