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
    public JsonResult ForgettingCurvesJson(CurvesJsonCmd curvesJsonCmd)
    {
        var interval = curvesJsonCmd.Interval.ToForgettingCurveInterval();
        return Json(ForgettingCurveJson.Load(interval, curvesJsonCmd));
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
        R<AnswerHistoryTestRepo>().TruncateTable();
        R<AnswerFeatureRepo>().TruncateTables();
        R<QuestionFeatureRepo>().TruncateTables();

        GenerateAnswerFeatures.Run();
        AssignAnswerFeatures.Run();
        AlgoTester.Run();

        GenerateQuestionFeatures.Run();
        AssignQuestionFeatures.Run();

        return View("AlgoInsight", new AlgoInsightModel
        {
	        Message = new SuccessMessage("Eine Algorithmus-Bewertung wurde ausgeführt.")
        });
    }
}