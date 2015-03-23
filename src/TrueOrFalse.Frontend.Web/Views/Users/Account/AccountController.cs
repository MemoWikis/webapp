using System.Web.Mvc;
using TrueOrFalse;

public class AccountController : BaseController
{
    [SetMenu(MenuEntry.None)]
    public ActionResult Membership()
    {
        return View("~/Views/Users/Account/Membership.aspx", new MembershipModel());
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
