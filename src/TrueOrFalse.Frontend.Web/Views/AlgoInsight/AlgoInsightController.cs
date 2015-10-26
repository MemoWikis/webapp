using System.Web.Mvc;
using TrueOrFalse.Web;

[SetMenu(MenuEntry.None)]
public class AlgoInsightController : BaseController
{
    private string _viewName = "AlgoInsight";

    public ActionResult Forecast()
    {
        return View(_viewName, new AlgoInsightModel{IsActiveTabForecast = true});
    }

    public ActionResult ForgettingCurve()
    {
        return View(_viewName, new AlgoInsightModel{IsActiveTabForgettingCurve = true});
    }

    [HttpPost]
    public JsonResult ForgettingCurvesJson()
    {
        return Json(ForgettingCurveJson.GetSample());
    }

    public ActionResult Repetition()
    {
        return View(_viewName, new AlgoInsightModel{IsActiveTabRepetition = true});
    }

    public ActionResult Various()
    {
        return View(_viewName, new AlgoInsightModel { IsActiveTabVarious = true });
    }

    [AccessOnlyAsAdmin]
	public ActionResult Reevaluate()
	{
        Sl.R<AnswerHistoryTestRepo>().TruncateTable();
        Sl.R<AnswerFeatureRepo>().TruncateTables();

        GenerateAnswerFeatures.Run();
        AssignAnswerFeatures.Run();
        AlgoTester.Run();

        return View("AlgoInsight", new AlgoInsightModel
        {
	        Message = new SuccessMessage("Eine Algorithmus-Bewertung wurde ausgeführt.")
        });
    }
}