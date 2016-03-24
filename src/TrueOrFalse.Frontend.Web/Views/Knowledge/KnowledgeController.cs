using System.Web.Mvc;

public class KnowledgeController : BaseController
{
    [SetMenu(MenuEntry.Knowledge)]
    public ActionResult Knowledge()
    {
        return View(new KnowledgeModel());
    }

    [SetMenu(MenuEntry.Knowledge)]
    public ActionResult EmailConfirmation(string emailKey)
    {
        return View("Knowledge", new KnowledgeModel(emailKey:emailKey));
    }

    [SetMenu(MenuEntry.Knowledge)]
    public ActionResult KnowledgeScreenshot()
    {
        return View(new KnowledgeModel());
    }
}