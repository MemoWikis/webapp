using System;
using System.Net.Mail;
using System.Web.Mvc;
using TrueOrFalse.Web;

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
        
        _sessionUser.User.MembershipPeriods.Add(membership);

        SendEmail.Run(new MailMessage(
            Settings.EmailFrom,
            Settings.EmailToMemucho,
            "We have a new member",
            $"New member: {_sessionUser.User.Name} {_sessionUser.User.Id}"));

        return View("~/Views/Users/Account/Membership.aspx", new MembershipModel
        {
            Message = new SuccessMessage(String.Format(
                "Du hast dich erfolgreich als Mitglied angemeldet. " +
                "Die Rechnung für deinen Mitgliedsbeitrag wird an <b>{0}</b> geschickt.",
                model.BillingEmail)),
            IsMember = true,
            Membership = membership
        });
    }

    [AccessOnlyAsAdmin]
    public ActionResult RemoveAdminRights()
    {
        _sessionUser.IsInstallationAdmin = false;

        if (Request.UrlReferrer == null)
            Redirect("/");

        return Redirect(Request.UrlReferrer.AbsolutePath);
    }

    [SetMenu(MenuEntry.None)]
    [AccessOnlyAsLoggedIn]
    public ActionResult WidgetViews()
    {
        return View("~/Views/Users/Account/WidgetViews/WidgetViews.aspx", new WidgetViewsModel());
    }
}
