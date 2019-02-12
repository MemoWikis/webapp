using System.Web.Mvc;

public class QuestionChangesOverviewController : Controller
{
    private const string _viewLocation = "~/Views/Questions/History/QuestionChangesOverview.aspx";

    public ActionResult List(int? pageToShow)
    {
        return View(_viewLocation, new QuestionChangesOverviewModel(pageToShow.GetValueOrDefault(1)));
    }
}
