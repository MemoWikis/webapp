using Microsoft.AspNetCore.Mvc;
using static LimitCheck;

namespace VueApp;

public class UserMembershipPlansController : Controller
{
    [HttpGet]
    public BasicLimits GetBasicLimits()
    {
        var limits = LimitCheck.GetBasicLimits();
        return limits;
    }
}