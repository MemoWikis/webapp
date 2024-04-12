using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrueOrFalse.Web;
using static CategoryDeleter;

public class DeleteTopicStoreController(
    SessionUser sessionUser,
    CategoryDeleter categoryDeleter) : BaseController(sessionUser)
{
    public readonly record struct DeleteData(string Name, bool HasChildren);

    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public DeleteData GetDeleteData([FromRoute] int id)
    {
        var topic = EntityCache.GetCategory(id);
        var children = GraphService.Descendants(id);
        var hasChildren = children.Count > 0;
        if (topic == null)
            throw new Exception(
                "Category couldn't be deleted. Category with specified Id cannot be found.");

        return new DeleteData(topic.Name, hasChildren);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<DeleteTopicResult> Delete([FromRoute] int id) =>
        await categoryDeleter.DeleteTopic(id);
}