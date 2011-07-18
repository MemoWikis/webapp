using System.Web.Mvc;
using TrueOrFalse.Core.Web.Context;
using TrueOrFalse.Frontend.Web.Models;


[HandleError]
public class AccountController : Controller
{
    private readonly SessionUser _sessionUser;

    public AccountController(SessionUser sessionUser)
    {
        _sessionUser = sessionUser;
    }

    public ActionResult LogOn()
    {
        return View();
    }

    public ActionResult Logout()
    {
        _sessionUser.Logout();
        return View(new ModelBase());
    }

}
