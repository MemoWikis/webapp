using System.Web.Mvc;
using NHibernate;

public class CategoryHistoryDetailController : Controller
{
    public ActionResult Detail(int categoryChangeId)
    {
        var currentRevision = Sl.CategoryChangeRepo.GetByIdEager(categoryChangeId);
        var previousRevision = Sl.CategoryChangeRepo.GetPreviousRevision(currentRevision);
        var nextRevision = Sl.CategoryChangeRepo.GetNextRevision(currentRevision);

        return View("~/Views/Categories/History/Detail/CategoryHistoryDetail.aspx", 
            new CategoryHistoryDetailModel(currentRevision, previousRevision, nextRevision));
    }
}