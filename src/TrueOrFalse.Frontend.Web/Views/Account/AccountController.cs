using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Web.Context;

public class AccountController : Controller
{
    private readonly SessionUser _sessionUser;
    private readonly RemovePersistentLoginFromCookie _removePersistentLoginFromCookie;

    public AccountController(SessionUser sessionUser, 
                             RemovePersistentLoginFromCookie removePersistentLoginFromCookie)
    {
        _sessionUser = sessionUser;
        _removePersistentLoginFromCookie = removePersistentLoginFromCookie;
    }

    public ActionResult LogOn()
    {
        return View();
    }

    public ActionResult Logout()
    {
        _removePersistentLoginFromCookie.Run();
        _sessionUser.Logout();
        return View(new BaseModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult RemoveAdminRights()
    {
        _sessionUser.IsInstallationAdmin = false;

        if (Request.UrlReferrer == null)
            Redirect("/");

        return Redirect(Request.UrlReferrer.AbsolutePath);
    }

}
