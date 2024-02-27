using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TrueOrFalse.Web;

public class DeleteTopicStoreController(
    SessionUser sessionUser,
    CategoryDeleter categoryDeleter) : BaseController(sessionUser)
{
    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public JsonResult GetDeleteData([FromRoute] int id)
    {
        var topic = EntityCache.GetCategory(id);
        var children = EntityCache.GetAllChildren(id);
        var hasChildren = children.Count > 0;
        if (topic == null)
            throw new Exception("Category couldn't be deleted. Category with specified Id cannot be found.");

        return Json(new {
            name = topic.Name,
            hasChildren = hasChildren,
        });
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult Delete([FromRoute] int id)
    {
        var result = categoryDeleter.DeleteTopic(id); 

        return Json(new
        {
            hasChildren = result.data.hasChildren,
            isNotCreatorOrAdmin = result.data.isNotCreatorOrAdmin,
            success = result.data.success,
            redirectParent = result.data.redirectParent
        });
    }
}