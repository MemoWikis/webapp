using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class MiddlewareAuthController(SessionUser _sessionUser) : Controller
{
    [AccessOnlyAsAdmin]
    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public bool Get()
    {
        return _sessionUser.IsInstallationAdmin;
    }
}