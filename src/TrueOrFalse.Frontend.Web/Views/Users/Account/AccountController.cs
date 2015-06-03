using System.Web.Mvc;

public class AccountController : BaseController
{
    [SetMenu(MenuEntry.None)]
    [HttpGet]
    public ActionResult Membership()
    {
        return View("~/Views/Users/Account/Membership.aspx", new MembershipModel());
    }

    [HttpPost]
    public ActionResult Membership(MembershipModel model)
    {
        var membership = model.ToMembership();
        R<MembershipRepo>().Create(membership);

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
