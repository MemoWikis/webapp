using System.Web.Mvc;

namespace VueApp;

public class TopicStoreController : BaseController
{
    public TopicStoreController(SessionUser sessionUser) : base(sessionUser)
    {
        
    }
    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public JsonResult SaveTopic(int id, string name, bool saveName, string content, bool saveContent)
    {
        if (!PermissionCheck.CanEditCategory(id))
            return Json("Dir fehlen leider die Rechte um die Seite zu bearbeiten");

        var categoryCacheItem = EntityCache.GetCategory(id);
        var category = Sl.CategoryRepo.GetById(categoryCacheItem.Id);

        if (categoryCacheItem == null || category == null)
            return Json(false);

        if (saveName)
        {
            categoryCacheItem.Name = name;
            category.Name = name;
        }

        if (saveContent)
        {
            categoryCacheItem.Content = content;
            category.Content = content;
        }
        EntityCache.AddOrUpdate(categoryCacheItem);
        Sl.CategoryRepo.Update(category, _sessionUser.User, type: CategoryChangeType.Text);

        return Json(true);
    }

    [HttpGet]
    public string GetTopicImageUrl(int id)
    {
        if (PermissionCheck.CanViewCategory(id))
            return new CategoryImageSettings(id).GetUrl_128px(asSquare: true).Url;

        return "";
    }


    [HttpGet]
    public JsonResult GetUpdatedKnowledgeSummary(int id)
    {
        var knowledgeSummary = KnowledgeSummaryLoader.RunFromMemoryCache(id, _sessionUser.UserId);

        return Json(new
        {
            notLearned = knowledgeSummary.NotLearned + knowledgeSummary.NotInWishknowledge,
            needsLearning = knowledgeSummary.NeedsLearning,
            needsConsolidation = knowledgeSummary.NeedsConsolidation,
            solid = knowledgeSummary.Solid,
        }, JsonRequestBehavior.AllowGet);
    }
}

