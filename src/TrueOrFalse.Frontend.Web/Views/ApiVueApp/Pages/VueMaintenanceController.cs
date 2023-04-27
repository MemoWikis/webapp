using System.Web.Helpers;
using System.Web.Mvc;

namespace VueApp;

public class VueMaintenanceController : BaseController
{
    [AccessOnlyAsLoggedIn]
    [AccessOnlyAsAdmin]
    [HttpGet]
    public JsonResult Get()
    {
        if (SessionUser.IsInstallationAdmin)
        {
            AntiForgery.GetTokens(null, out string cookieToken, out string formToken);

            return Json(new
            {
                isAdmin = true,
                antiForgeryFormToken = formToken,
                antiForgeryCookieToken = cookieToken
            },JsonRequestBehavior.AllowGet);
        }

        return Json(new { isAdmin = false }, JsonRequestBehavior.AllowGet);
    }
}