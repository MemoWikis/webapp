using System.Web.Mvc;

public class KnowledgeController : Controller
{
    public ActionResult Knowledge()
    {
        return View(new KnowledgeModel());
    }
}
