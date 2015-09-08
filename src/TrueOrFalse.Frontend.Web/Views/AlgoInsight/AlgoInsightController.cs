using System.Web.Mvc;

[SetMenu(MenuEntry.None)]
public class AlgoInsightController : BaseController
{
    public ActionResult AlgoInsight()
    {
        return View(new AlgoInsightModel());
    }
}