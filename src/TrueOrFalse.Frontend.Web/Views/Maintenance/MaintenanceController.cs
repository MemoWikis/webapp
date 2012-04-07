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

    public ActionResult Maintenance()
    {
        return View(new MaintenanceModel());
    }

    public ActionResult CalcAggregatedValues()
    {
        _updateQuestionAnswerCounts.Run();
        return View("Maintenance", new MaintenanceModel{Message = new SuccessMessage("Aggregierte Werte wurden aktualisiert.")});
    }
}
