using Microsoft.AspNetCore.Mvc;
using TrueOrFalse.Domain;

namespace VueApp;

public class UserMembershipPlansController : BaseController
{
    public UserMembershipPlansController(SessionUser sessionUser) : base(sessionUser)
    {
    }

    [HttpGet]
    public JsonResult GetBasicLimits()
    {
        var limits = LimitCheck.GetBasicLimits();
        return Json(limits);
    }
}