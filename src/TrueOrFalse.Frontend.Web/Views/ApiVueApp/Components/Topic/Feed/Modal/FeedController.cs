using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace VueApp;

public class FeedModalTopicController(
    PermissionCheck _permissionCheck) : Controller
{
    public readonly record struct GetContentChangeRequest(int Topicid, int ChangeId);
    public record struct ContentChange(string OldContent, string NewContent, string DiffContent);

    [HttpPost]
    public ContentChange GetContentChange([FromBody] GetContentChangeRequest req)
    {
        var topic = EntityCache.GetCategory(req.Topicid);
        if (!_permissionCheck.CanView(topic))
            throw new Exception("No permission");

        var contentChange = topic?.CategoryChangeCacheItems.First(cc => cc.Id == req.ChangeId).CategoryChangeData.ContentChange;

        if (contentChange == null)
            throw new Exception("No content change found");

        return new ContentChange(contentChange?.OldContent, contentChange?.NewContent, contentChange?.DiffContent);
    }

}