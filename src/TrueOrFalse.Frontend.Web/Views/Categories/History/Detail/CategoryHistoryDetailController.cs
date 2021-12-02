using System.Linq;
using System.Web.Mvc;
using NHibernate;

public class CategoryHistoryDetailController : Controller
{
    public ActionResult Detail(int categoryChangeId, int categoryId)
    {
        var model = GetCategoryHistoryDetailModel(categoryId, categoryChangeId);

        return View("~/Views/Categories/History/Detail/CategoryHistoryDetail.aspx", model);
    }

    public CategoryHistoryDetailModel GetCategoryHistoryDetailModel(int categoryId, int categoryChangeId)
    {
        var listWithAllVersions = Sl.CategoryChangeRepo.GetForCategory(categoryId).OrderBy(c => c.Id);
        var isCategoryDeleted = listWithAllVersions.Any(cc => cc.Type == CategoryChangeType.Delete);

        var currentRevision = listWithAllVersions.FirstOrDefault(c => c.Id == categoryChangeId);
        var previousRevision = listWithAllVersions.LastOrDefault(c => c.Id < categoryChangeId);
        var nextRevision = listWithAllVersions.FirstOrDefault(c => c.Id > categoryChangeId);
        return new CategoryHistoryDetailModel(currentRevision, previousRevision, nextRevision, isCategoryDeleted);
    }
}