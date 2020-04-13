using System.Linq;
using System.Web.Mvc;
using NHibernate;

public class CategoryHistoryDetailController : Controller
{
    public ActionResult Detail(int categoryChangeId, int categoryId)
    {
        var ListWithAllVersions = Sl.CategoryChangeRepo.GetForCategory(categoryId).OrderBy(c => c.Id);
        var isCategoryDeleted = ListWithAllVersions.Where(cc => cc.Type == CategoryChangeType.Delete).Any();

        var currentRevision = ListWithAllVersions.Where(c => c.Id == categoryChangeId).FirstOrDefault();
        var previousRevision = ListWithAllVersions.Where(c => c.Id < categoryChangeId ).LastOrDefault();
        var nextRevision = ListWithAllVersions.Where(c => c.Id > categoryChangeId).FirstOrDefault();

        return View("~/Views/Categories/History/Detail/CategoryHistoryDetail.aspx", 
            new CategoryHistoryDetailModel(currentRevision, previousRevision, nextRevision,isCategoryDeleted));
    }
}