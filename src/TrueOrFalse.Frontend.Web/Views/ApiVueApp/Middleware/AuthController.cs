using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class MiddlewareAuthController(SessionUser _sessionUser) : Controller
{
    [HttpGet]
    public bool Get()
    {
        return _sessionUser.IsInstallationAdmin;
    }
}