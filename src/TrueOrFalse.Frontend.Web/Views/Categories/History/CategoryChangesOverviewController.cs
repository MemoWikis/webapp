using System.Web.Mvc;

public class CategoryChangesOverviewController : Controller
{
    private const string _viewLocation = "~/Views/Categories/History/CategoryChangesOverview.aspx";

    public ActionResult List(int? pageToShow)
    {
        return View(_viewLocation, new CategoryChangesOverviewModel(pageToShow.GetValueOrDefault(1)));
    }
}
