using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Search;
using TrueOrFalse.Web;

public class MaintenanceController : BaseController
{
    [AccessOnlyAsAdminAndLocal]
    public ActionResult Maintenance()
    {
        return View(new MaintenanceModel());
    }

    [AccessOnlyAsAdminAndLocal]
    public ActionResult RecalculateAllKnowledgeItems()
    {
        Resolve<RecalculateAllKnowledgeItems>().Run();
        return View("Maintenance", new MaintenanceModel { Message = new SuccessMessage("Antwortwahrscheinlichkeiten wurden neu berechnet.") });
    }

    [AccessOnlyAsAdminAndLocal]
    public ActionResult CalcAggregatedValues()
    {
        Resolve<UpdateQuestionAnswerCounts>().Run();
        return View("Maintenance", new MaintenanceModel{Message = new SuccessMessage("Aggregierte Werte wurden aktualisiert.")});
    }

    [AccessOnlyAsAdminAndLocal]
    public ActionResult UpdateFieldQuestionCountForCategories()
    {
        Resolve<UpdateQuestionCountForAllCategories>().Run();
        return View("Maintenance", new MaintenanceModel { Message = new SuccessMessage("Feld: AnzahlFragen für Kategorien wurde aktualisiert.") });
    }

    [AccessOnlyAsAdminAndLocal]
    public ActionResult ReIndexAllQuestions()
    {
        Resolve<ReIndexAllQuestions>().Run();
        return View("Maintenance", new MaintenanceModel { Message = new SuccessMessage("Fragen wurden neu indiziert.") });
    }

    [AccessOnlyAsAdminAndLocal]
    public ActionResult ReIndexAllSets()
    {
        Resolve<ReIndexAllSets>().Run();
        return View("Maintenance", new MaintenanceModel { Message = new SuccessMessage("Fragesätze wurden neu indiziert.") });
    }

    [AccessOnlyAsAdminAndLocal]
    public ActionResult ReIndexAllCategories()
    {
        Resolve<ReIndexAllCategories>().Run();
        return View("Maintenance", new MaintenanceModel { Message = new SuccessMessage("Kategorien wurden neu indiziert.") });
    }
}
