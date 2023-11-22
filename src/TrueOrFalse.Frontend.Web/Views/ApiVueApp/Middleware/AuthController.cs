using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class MiddlewareAuthController : BaseController
{
    public MiddlewareAuthController(SessionUser sessionUser) :base(sessionUser)
    {
        
    }
    [AccessOnlyAsLoggedIn]
    [AccessOnlyAsAdmin]
    [HttpGet]
    public JsonResult Get()
    {
        return Json(_sessionUser.IsInstallationAdmin);
    }
}   