using System.Linq;
using System.Web.Mvc;
using NHibernate;

public class CategoryHistoryDetailController : Controller
{
    public ActionResult Detail(int categoryChangeId, int categoryId)
    {
        return GroupedDetail(categoryId, categoryChangeId, categoryChangeId);
    }

    public ActionResult GroupedDetail(int categoryId, int firstEditId, int selectedRevId)
    {
        var model = GetCategoryHistoryDetailModel(categoryId, firstEditId, selectedRevId);

        return View("~/Views/Categories/History/Detail/CategoryHistoryDetail.aspx", model);
    }

    public CategoryHistoryDetailModel GetCategoryHistoryDetailModel(int categoryId, int firstEditId, int selectedRevId)
    {
        var listWithAllVersions = Sl.CategoryChangeRepo.GetForCategory(categoryId).OrderBy(c => c.Id);
        var isCategoryDeleted = listWithAllVersions.Any(cc => cc.Type == CategoryChangeType.Delete);

        var currentRevision = listWithAllVersions.FirstOrDefault(c => c.Id == selectedRevId);
        var previousRevision = listWithAllVersions.LastOrDefault(c => c.Id < firstEditId);
        var nextRevision = listWithAllVersions.FirstOrDefault(c => c.Id > selectedRevId);
        return new CategoryHistoryDetailModel(currentRevision, previousRevision, nextRevision, isCategoryDeleted);
    }
}