using System.Web.Mvc;

public class HelpController : BaseController
{
    [SetMenu(MenuEntry.Help)]
    public ActionResult FAQ() { return View(new HelpModel()); }

    [SetMenu(MenuEntry.Help)]
    public ActionResult Widget() { return View(new BaseModel()); }

    [SetMenu(MenuEntry.Help)]
    public ActionResult WidgetPricing() { return View(new BaseModel()); }

    [SetMenu(MenuEntry.Help)]
    public ActionResult WidgetInWordpress() { return View(new BaseModel()); }

    [SetMenu(MenuEntry.Help)]
    public ActionResult WidgetInMoodle() { return View(new BaseModel()); }

    [SetMenu(MenuEntry.Help)]
    public ActionResult WidgetInBlackboard() { return View(new BaseModel()); }

    [SetMenu(MenuEntry.Help)]
    public ActionResult WidgetExamples() { return View(new BaseModel()); }
}
