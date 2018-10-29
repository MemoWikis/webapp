using System.Web.Mvc;
using NHibernate;

public class CategoryHistoryDetailController : Controller
{
    public ActionResult Detail(int categoryChangeId)
    {
        var currentRevision = Sl.CategoryChangeRepo.GetByIdEager(categoryChangeId);

        var categoryId = currentRevision.Category.Id;
        var currentRevisionDate = currentRevision.DateCreated.ToString("yyyy-MM-dd HH-mm-ss");
        var query = $@"
            
            SELECT * FROM CategoryChange cc
            WHERE cc.Category_id = {categoryId} and cc.DateCreated < '{currentRevisionDate}' 
            ORDER BY cc.DateCreated DESC LIMIT 1

            ";
        var previousRevision = Sl.R<ISession>().CreateSQLQuery(query).AddEntity(typeof(CategoryChange)).UniqueResult<CategoryChange>();

        return View("~/Views/Categories/History/Detail/CategoryHistoryDetail.aspx", new CategoryHistoryDetailModel(currentRevision, previousRevision));
    }
}