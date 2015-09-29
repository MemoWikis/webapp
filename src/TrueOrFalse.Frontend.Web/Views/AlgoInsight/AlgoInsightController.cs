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

    public ActionResult LearningCurve()
    {
        return View(_viewName, new AlgoInsightModel{IsActiveTabLearningCurve = true});
    }

    public ActionResult Repetition()
    {
        return View(_viewName, new AlgoInsightModel{IsActiveTabRepetition = true});
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