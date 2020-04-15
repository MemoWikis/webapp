using System.Linq;
using System.Web.Mvc;
using NHibernate;

public class CategoryHistoryDetailController : Controller
{
    public ActionResult Detail(int categoryChangeId, int categoryId)
    {
        var ListWithAllVersions = Sl.CategoryChangeRepo.GetForCategory(categoryId).OrderBy(c => c.Id);
        var isCategoryDeleted = ListWithAllVersions.Any(cc => cc.Type == CategoryChangeType.Delete);

        var currentRevision = ListWithAllVersions.FirstOrDefault(c => c.Id == categoryChangeId);
        var previousRevision = ListWithAllVersions.LastOrDefault(c => c.Id < categoryChangeId);
        var nextRevision = ListWithAllVersions.FirstOrDefault(c => c.Id > categoryChangeId);

        return View("~/Views/Categories/History/Detail/CategoryHistoryDetail.aspx", 
            new CategoryHistoryDetailModel(currentRevision, previousRevision, nextRevision,isCategoryDeleted));
    }
}