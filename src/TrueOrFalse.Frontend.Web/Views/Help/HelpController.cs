using System.Web.Mvc;

public class HelpController : BaseController
{
    [SetMainMenu(MainMenuEntry.Help)]
    public ActionResult FAQ() { return View(new HelpModel()); }

    [SetMainMenu(MainMenuEntry.Help)]
    public ActionResult Widget() { return View(new BaseModel()); }

    [SetMainMenu(MainMenuEntry.Help)]
    public ActionResult WidgetPricing() { return View(new BaseModel()); }

    [SetMainMenu(MainMenuEntry.Help)]
    public ActionResult WidgetInWordpress() { return View(new BaseModel()); }

    [SetMainMenu(MainMenuEntry.Help)]
    public ActionResult WidgetInMoodle() { return View(new BaseModel()); }

    [SetMainMenu(MainMenuEntry.Help)]
    public ActionResult WidgetInBlackboard() { return View(new BaseModel()); }

    [SetMainMenu(MainMenuEntry.Help)]
    public ActionResult WidgetExamples() { return View(new BaseModel()); }
}
