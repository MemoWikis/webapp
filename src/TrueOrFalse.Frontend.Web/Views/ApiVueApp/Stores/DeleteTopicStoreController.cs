using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TrueOrFalse.Web;

public class DeleteTopicStoreController(
    SessionUser sessionUser,
    CategoryDeleter categoryDeleter) : BaseController(sessionUser)
{
    public record DeleteData(string Name, bool HasChildren); 
    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public OkObjectResult GetDeleteData([FromRoute] int id)
    {
        var topic = EntityCache.GetCategory(id);
        var children = GraphService.Descendants(id);
        var hasChildren = children.Count > 0;
        if (topic == null)
            throw new Exception("Category couldn't be deleted. Category with specified Id cannot be found.");

        return Ok(new DeleteData(topic.Name, hasChildren));
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public OkObjectResult Delete([FromRoute] int id)
    {
        var result = categoryDeleter.DeleteTopic(id); 

        return Ok(result);
    }
}