using System.Web.Mvc;

public class AccountController : BaseController
{
    public AccountController(SessionUser sessionUser) : base(sessionUser)
    {
        
    }

    [AccessOnlyAsAdmin]
    public ActionResult RemoveAdminRights()
    {
        SessionUserLegacy.IsInstallationAdmin = false;

        if (Request.UrlReferrer == null)
        {
            Redirect("/");
        }

        return Redirect(Request.UrlReferrer.AbsolutePath);
    }
}