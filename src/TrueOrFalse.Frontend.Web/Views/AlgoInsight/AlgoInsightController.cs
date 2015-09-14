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
        Sl.Resolve<AnswerHistoryTestRepo>().TruncateTable();

        AlgoTester.Run();

        return View("AlgoInsight", new AlgoInsightModel
        {
	        Message = new SuccessMessage("Eine Algorithmus-Bewertung wurde ausgeführt.")
        });
    }
}