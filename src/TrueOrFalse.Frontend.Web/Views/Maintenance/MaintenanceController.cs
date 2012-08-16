using System.Web.Mvc;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Web;

public class MaintenanceController : Controller
{
    private readonly UpdateQuestionAnswerCounts _updateQuestionAnswerCounts;

    public MaintenanceController(UpdateQuestionAnswerCounts updateQuestionAnswerCounts)
    {
        _updateQuestionAnswerCounts = updateQuestionAnswerCounts;
    }

    [AccessOnlyLocalAttribute]
    public ActionResult Maintenance()
    {
        return View(new MaintenanceModel());
    }

    [AccessOnlyLocalAttribute]
    public ActionResult RecalculateAllKnowledgeItems()
    {
        Sl.Resolve<RecalculateAllKnowledgeItems>().Run();
        return View("Maintenance", new MaintenanceModel { Message = new SuccessMessage("Antwortwahrscheinlichkeiten wurden neu berechnet.") });
    }

    [AccessOnlyLocalAttribute]
    public ActionResult CalcAggregatedValues()
    {
        _updateQuestionAnswerCounts.Run();
        return View("Maintenance", new MaintenanceModel{Message = new SuccessMessage("Aggregierte Werte wurden aktualisiert.")});
    }
}
