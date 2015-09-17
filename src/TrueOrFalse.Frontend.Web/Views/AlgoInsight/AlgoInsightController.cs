using System.Web.Mvc;
using TrueOrFalse.Web;

[SetMenu(MenuEntry.None)]
public class AlgoInsightController : BaseController
{
    public ActionResult AlgoInsight()
    {
        return View(new AlgoInsightModel());
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