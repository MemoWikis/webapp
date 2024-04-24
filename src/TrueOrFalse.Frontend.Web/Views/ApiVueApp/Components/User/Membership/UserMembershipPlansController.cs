using Microsoft.AspNetCore.Mvc;
using TrueOrFalse.Domain;
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