using System.Web.Mvc;

public class AlgoInsightController : BaseController
{
    public ActionResult AlgoInsight()
    {
        return View(new AlgoInsightModel());
    }
}