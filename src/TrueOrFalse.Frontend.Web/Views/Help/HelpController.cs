using System.Web.Mvc;

public class HelpController : BaseController
{
    [SetMenu(MenuEntry.Help)]
    public ActionResult FAQ() {return View(new HelpModel()); }
}
