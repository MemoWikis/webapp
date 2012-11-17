using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Search;
using TrueOrFalse.Web;

public class MaintenanceController : Controller
{
    private readonly UpdateQuestionAnswerCounts _updateQuestionAnswerCounts;

    public MaintenanceController(UpdateQuestionAnswerCounts updateQuestionAnswerCounts)
    {
        _updateQuestionAnswerCounts = updateQuestionAnswerCounts;
    }

    [AccessOnlyAsAdminAndLocal]
    public ActionResult Maintenance()
    {
        return View(new MaintenanceModel());
    }

    [AccessOnlyAsAdminAndLocal]
    public ActionResult RecalculateAllKnowledgeItems()
    {
        Sl.Resolve<RecalculateAllKnowledgeItems>().Run();
        return View("Maintenance", new MaintenanceModel { Message = new SuccessMessage("Antwortwahrscheinlichkeiten wurden neu berechnet.") });
    }

    [AccessOnlyAsAdminAndLocal]
    public ActionResult CalcAggregatedValues()
    {
        _updateQuestionAnswerCounts.Run();
        return View("Maintenance", new MaintenanceModel{Message = new SuccessMessage("Aggregierte Werte wurden aktualisiert.")});
    }

    [AccessOnlyAsAdminAndLocal]
    public ActionResult ReIndexAllQuestions()
    {
        Sl.Resolve<ReIndexAllQuestions>().Run();
        return View("Maintenance", new MaintenanceModel { Message = new SuccessMessage("Fragen wurden neu indiziert.") });
    }
}
