using System.Web.Mvc;

[SetUserMenu(UserMenuEntry.None)]
public class HelpController : Controller
{
    [SetMainMenu(MainMenuEntry.Help)]
    public ActionResult FAQ() { return View(new HelpModel()); }

}
