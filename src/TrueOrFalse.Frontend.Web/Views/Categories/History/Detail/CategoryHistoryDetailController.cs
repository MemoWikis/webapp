using System.Web.Mvc;
using NHibernate;

public class CategoryHistoryDetailController : Controller
{
    public ActionResult Detail(int categoryChangeId)
    {
        var currentRevision = Sl.CategoryChangeRepo.GetByIdEager(categoryChangeId);
        var previousRevision = GetPreviousRevision(currentRevision);
        var nextRevision = GetNextRevision(currentRevision);

        return View("~/Views/Categories/History/Detail/CategoryHistoryDetail.aspx", 
            new CategoryHistoryDetailModel(currentRevision, previousRevision, nextRevision));
    }

    private static CategoryChange GetNextRevision(CategoryChange currentRevision)
    {
        var categoryId = currentRevision.Category.Id;
        var currentRevisionDate = currentRevision.DateCreated.ToString("yyyy-MM-dd HH-mm-ss");
        var query = $@"
            
            SELECT * FROM CategoryChange cc
            WHERE cc.Category_id = {categoryId} and cc.DateCreated > '{currentRevisionDate}' 
            ORDER BY cc.DateCreated 
            LIMIT 1

            ";
        var nextRevision = Sl.R<ISession>().CreateSQLQuery(query).AddEntity(typeof(CategoryChange)).UniqueResult<CategoryChange>();
        return nextRevision;
    }

    private static CategoryChange GetPreviousRevision(CategoryChange currentRevision)
    {
        var categoryId = currentRevision.Category.Id;
        var currentRevisionDate = currentRevision.DateCreated.ToString("yyyy-MM-dd HH-mm-ss");
        var query = $@"
            
            SELECT * FROM CategoryChange cc
            WHERE cc.Category_id = {categoryId} and cc.DateCreated < '{currentRevisionDate}' 
            ORDER BY cc.DateCreated DESC 
            LIMIT 1

            ";
        var previousRevision = Sl.R<ISession>().CreateSQLQuery(query).AddEntity(typeof(CategoryChange)).UniqueResult<CategoryChange>();
        return previousRevision;
    }
}