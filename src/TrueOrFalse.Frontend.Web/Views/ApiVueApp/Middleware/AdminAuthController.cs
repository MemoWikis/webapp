using System.Web.Mvc;

namespace VueApp;

public class AdminAuthController : BaseController
{
    [AccessOnlyAsLoggedIn]
    [AccessOnlyAsAdmin]
    [HttpGet]
    public bool Get()
    {
        return SessionUser.User.IsInstallationAdmin;
    }
}