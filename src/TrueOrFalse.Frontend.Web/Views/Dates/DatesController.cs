using System.Web.Mvc;

public class DatesController : BaseController
{
    [SetMenu(MenuEntry.Dates)]
    public ActionResult Dates()
    {
        return View(new DatesModel());
    }
}