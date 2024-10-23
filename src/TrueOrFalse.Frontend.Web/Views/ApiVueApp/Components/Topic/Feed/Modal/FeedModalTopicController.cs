using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TrueOrFalse.Web;

namespace VueApp;

public class FeedModalTopicController(
    PermissionCheck _permissionCheck,
    CategoryChangeRepo _categoryChangeRepo) : Controller
{
    public readonly record struct GetContentChangeRequest(int Topicid, int ChangeId, int? OldestChangeId = null);
    public record struct ContentChange(string CurrentContent, string DiffContent);

    [HttpPost]
    public ContentChange GetContentChange([FromBody] GetContentChangeRequest req)
    {
        var topic = EntityCache.GetCategory(req.Topicid);
        if (!_permissionCheck.CanView(topic))
            throw new Exception("No permission");

        var currentChange = _categoryChangeRepo.GetById(req.ChangeId);

        var previousId = topic?.CategoryChangeCacheItems.First(cc => cc.Id == (req.OldestChangeId > 0 ? req.OldestChangeId : req.ChangeId)).CategoryChangeData.PreviousId;

        if (currentChange == null || previousId == null)
            throw new Exception("No content change found");

        var previousChange = _categoryChangeRepo.GetById((int)previousId);

        if (previousChange == null)
            throw new Exception("No content change found");

        var currentContent = currentChange.GetCategoryChangeData().Content;

        var previousChangeData = previousChange.GetCategoryChangeData();
        var previousContent = previousChangeData.Content;

        if (String.IsNullOrEmpty(previousContent) && previousChange.DataVersion == 1 && !String.IsNullOrEmpty(previousChangeData.TopicMardkown))
            previousContent = MarkdownMarkdig.ToHtml(previousChangeData.TopicMardkown);

        if (previousContent == null)
            previousContent = "";

        HtmlDiff.HtmlDiff diffHelper = new HtmlDiff.HtmlDiff(previousContent, currentContent);
        string diffOutput = diffHelper.Build();

        return new ContentChange(currentContent, diffOutput);
    }

}