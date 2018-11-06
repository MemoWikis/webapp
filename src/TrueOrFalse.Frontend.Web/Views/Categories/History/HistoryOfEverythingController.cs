using System.Web.Mvc;

public class HistoryOfEverythingController : Controller
{
    private const string _viewLocation = "~/Views/Categories/History/HistoryOfEverything.aspx";

    public ActionResult List(int? pageToShow)
    {
        return View(_viewLocation, new HistoryOfEverythingModel(pageToShow.GetValueOrDefault(1)));
    }
}
