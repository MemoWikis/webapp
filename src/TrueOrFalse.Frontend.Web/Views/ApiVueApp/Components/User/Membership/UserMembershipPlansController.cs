using System.Web.Helpers;
using System.Web.Mvc;
using TrueOrFalse.Domain;

namespace VueApp;

public class UserMembershipPlansController : Controller
{
    [HttpGet]
    public JsonResult GetBasicLimits()
    {
        var limits = LimitCheck.GetBasicLimits();
        return Json(limits, JsonRequestBehavior.AllowGet);
    }

}