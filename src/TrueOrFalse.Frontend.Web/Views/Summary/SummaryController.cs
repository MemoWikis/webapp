using System.Web.Mvc;

public class SummaryController : Controller
{
    public ActionResult Summary()
    {
        return View(new SummaryModel());
    }

}
