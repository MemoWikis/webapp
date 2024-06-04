using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

public class DeleteTopicStoreController(
    SessionUser sessionUser,
    CategoryDeleter categoryDeleter,
    CrumbtrailService _crumbtrailService) : BaseController(sessionUser)
{
    public readonly record struct DeleteData(string Name, bool HasChildren, BreadcrumbItem Parent);


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

        var currentWiki = EntityCache.GetCategory(_sessionUser.CurrentWikiId);

        var parents = _crumbtrailService.BuildCrumbtrail(topic, currentWiki);
        var breadcrumbItem = GetLastBreadcrumbItem(parents);
        return new DeleteData(topic.Name, hasChildren, breadcrumbItem);
    }


    public readonly record struct DeleteJson(int id, int parentForQuestionsId);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public CategoryDeleter.DeleteTopicResult Delete([FromBody] DeleteJson deleteJson) =>
        categoryDeleter.DeleteTopic(deleteJson.id, deleteJson.parentForQuestionsId);

    private BreadcrumbItem GetLastBreadcrumbItem(Crumbtrail breadcrumb)
    {
        var breadcrumbItem = breadcrumb.Items.Last();

        return new BreadcrumbItem
        {
            Name = breadcrumbItem.Text,
            Id = breadcrumbItem.Category.Id
        };
    }

    public record struct BreadcrumbItem(string Name, int Id);
}