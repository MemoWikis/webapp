using System.Web.Mvc;

public class KnowledgeController : BaseController
{
    [SetMenu(MenuEntry.Knowledge)]
    public ActionResult Knowledge()
    {
        if (!_sessionUser.IsLoggedIn)
            return View(new KnowledgeModel());

        return View(
            new KnowledgeModel()
        );
    }
}