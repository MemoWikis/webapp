using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TrueOrFalse.Web;

namespace VueApp;

public class FeedModalPageController(
    PermissionCheck _permissionCheck,
    PageChangeRepo pageChangeRepo) : Controller
{
    public readonly record struct GetContentChangeRequest(int Pageid, int ChangeId, int? OldestChangeId = null);
    public record struct ContentChange(string CurrentContent, string DiffContent);

    [HttpPost]
    public ContentChange GetContentChange([FromBody] GetContentChangeRequest req)
    {
        var topic = EntityCache.GetPage(req.Pageid);
        if (!_permissionCheck.CanView(topic))
            throw new Exception("No permission");

        var currentChange = pageChangeRepo.GetById(req.ChangeId);

        var previousId = topic?.CategoryChangeCacheItems.First(cc => cc.Id == (req.OldestChangeId > 0 ? req.OldestChangeId : req.ChangeId)).PageChangeData.PreviousId;

        if (currentChange == null || previousId == null)
            throw new Exception("No content change found");

        var previousChange = pageChangeRepo.GetById((int)previousId);

        if (previousChange == null)
            throw new Exception("No content change found");

        var currentContent = currentChange.GetPageChangeData().Content;

        var previousChangeData = previousChange.GetPageChangeData();
        var previousContent = previousChangeData.Content;

        if (String.IsNullOrEmpty(previousContent) && previousChange.DataVersion == 1 && !String.IsNullOrEmpty(previousChangeData.PageMardkown))
            previousContent = MarkdownMarkdig.ToHtml(previousChangeData.PageMardkown);

        if (previousContent == null)
            previousContent = "";

        HtmlDiff.HtmlDiff diffHelper = new HtmlDiff.HtmlDiff(previousContent, currentContent);
        string diffOutput = diffHelper.Build();

        return new ContentChange(currentContent, diffOutput);
    }

}