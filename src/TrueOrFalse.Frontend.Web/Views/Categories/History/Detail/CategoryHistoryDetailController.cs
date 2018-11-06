using System.Web.Mvc;
using NHibernate;

public class CategoryHistoryDetailController : Controller
{
    public ActionResult Detail(int categoryChangeId)
    {
        var currentRevision = Sl.CategoryChangeRepo.GetByIdEager(categoryChangeId);
        var previousRevision = currentRevision.GetPreviousRevision();
        var nextRevision = currentRevision.GetNextRevision();

        return View("~/Views/Categories/History/Detail/CategoryHistoryDetail.aspx", 
            new CategoryHistoryDetailModel(currentRevision, previousRevision, nextRevision));
    }
}