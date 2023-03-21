using System;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Web;

public class DeleteTopicStoreController : BaseController
{
    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public JsonResult GetDeleteData(int id)
    {
        var topic = EntityCache.GetCategory(id);
        var children = EntityCache.GetAllChildren(id);
        var hasChildren = children.Count > 0;
        if (topic == null)
            throw new Exception("Category couldn't be deleted. Category with specified Id cannot be found.");

        return Json(new {
            name = topic.Name,
            hasChildren = hasChildren,
        }, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult Delete(int id)
    {
        var redirectURL = GetRedirectTopic(id);
        var topic = Sl.CategoryRepo.GetById(id);
        if (topic == null)
            throw new Exception("Category couldn't be deleted. Category with specified Id cannot be found.");

        var parentIds =
            EntityCache.GetCategory(id).ParentCategories().Select(c => c.Id)
                .ToList(); //if the parents are fetched directly from the category there is a problem with the flush
        var parentTopics = Sl.CategoryRepo.GetByIds(parentIds);

        var hasDeleted = Sl.CategoryDeleter.Run(topic, SessionUser.UserId);
        foreach (var parent in parentTopics)
        {
            Sl.CategoryChangeRepo.AddUpdateEntry(parent, SessionUser.UserId, false);
        }

        return Json(new
        {
            hasChildren = hasDeleted.HasChildren,
            isNotCreatorOrAdmin = hasDeleted.IsNotCreatorOrAdmin,
            success = hasDeleted.DeletedSuccessful,
            redirectURL = redirectURL
        });
    }

    private string GetRedirectTopic(int id)
    {
        var topic = EntityCache.GetCategory(id);
        var currentWiki = EntityCache.GetCategory(SessionUser.CurrentWikiId);
        var lastBreadcrumbItem = CrumbtrailService.BuildCrumbtrail(topic, currentWiki).Items.LastOrDefault();

        return "/" + UriSanitizer.Run(lastBreadcrumbItem.Text) + "/" + lastBreadcrumbItem.Category.Id;
    }
}