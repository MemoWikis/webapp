using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Web.Context;
using TrueOrFalse.Frontend.Web.Models;


[HandleError]
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

}
