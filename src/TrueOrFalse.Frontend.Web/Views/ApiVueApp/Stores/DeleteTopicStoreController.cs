using System;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Web;

public class DeleteTopicStoreController : BaseController
{
    private readonly CategoryDeleter _categoryDeleter;
    private readonly CrumbtrailService _crumbtrailService;

    public DeleteTopicStoreController(SessionUser sessionUser,CategoryDeleter categoryDeleter,CrumbtrailService crumbtrailService) :base(sessionUser)
    {
        _categoryDeleter = categoryDeleter;
        _crumbtrailService = crumbtrailService;
    }

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
        var redirectParent = GetRedirectTopic(id);
        var topic = Sl.CategoryRepo.GetById(id);
        if (topic == null)
            throw new Exception("Category couldn't be deleted. Category with specified Id cannot be found.");

        var parentIds =
            EntityCache.GetCategory(id).ParentCategories().Select(c => c.Id)
                .ToList(); //if the parents are fetched directly from the category there is a problem with the flush
        var parentTopics = Sl.CategoryRepo.GetByIds(parentIds);

        var hasDeleted = _categoryDeleter.Run(topic, _sessionUser.UserId);
        foreach (var parent in parentTopics)
        {
            Sl.CategoryChangeRepo.AddUpdateEntry(parent, _sessionUser.UserId, false);
        }

        return Json(new
        {
            hasChildren = hasDeleted.HasChildren,
            isNotCreatorOrAdmin = hasDeleted.IsNotCreatorOrAdmin,
            success = hasDeleted.DeletedSuccessful,
            redirectParent = redirectParent
        });
    }

    private dynamic GetRedirectTopic(int id)
    {
        var topic = EntityCache.GetCategory(id);
        var currentWiki = EntityCache.GetCategory(_sessionUser.CurrentWikiId);
        var lastBreadcrumbItem = _crumbtrailService.BuildCrumbtrail(topic, currentWiki).Items.LastOrDefault();

        return new
        {
            name = lastBreadcrumbItem.Category.Name,
            id = lastBreadcrumbItem.Category.Id
        };
    }
}