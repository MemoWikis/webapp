using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace VueApp;
public class BreadcrumbController(
    SessionUser _sessionUser,
    CrumbtrailService _crumbtrailService) : Controller
{
    public readonly record struct GetBreadcrumbParam(int WikiId, int CurrentCategoryId);

    [HttpPost]
    public Breadcrumb GetBreadcrumb([FromBody] GetBreadcrumbParam param)
    {
        var wikiId = param.WikiId;
        int currentPageId = param.CurrentCategoryId;

        var defaultWikiId = _sessionUser.IsLoggedIn ? _sessionUser.User.StartTopicId : 1;
        _sessionUser.SetWikiId(wikiId != 0 ? wikiId : defaultWikiId);
        var category = EntityCache.GetPage(currentPageId);
        var currentWiki = _crumbtrailService.GetWiki(category, _sessionUser);
        _sessionUser.SetWikiId(currentWiki);

        var breadcrumb = _crumbtrailService.BuildCrumbtrail(category, currentWiki);

        return GetBreadcrumbItems(breadcrumb, currentWiki);
    }

    [HttpGet]
    public BreadcrumbItem GetPersonalWiki()
    {
        var topic = _sessionUser.IsLoggedIn ? EntityCache.GetPage(_sessionUser.User.StartTopicId) : RootCategory.Get;
        return new BreadcrumbItem
        {
            Name = topic.Name,
            Id = topic.Id
        };
    }

    private Breadcrumb GetBreadcrumbItems(Crumbtrail breadcrumb, PageCacheItem currentWiki)
    {
        var breadcrumbItems = new List<BreadcrumbItem>();

        foreach (var item in breadcrumb.Items)
        {
            if (item.Page.Id != breadcrumb.Root.Page.Id)
                breadcrumbItems.Add(new BreadcrumbItem
                {
                    Name = item.Text,
                    Id = item.Page.Id
                });
        }

        var personalWiki = new BreadcrumbItem();
        if (_sessionUser.IsLoggedIn)
        {
            var personalWikiId = _sessionUser.User.StartTopicId;
            personalWiki.Id = personalWikiId;
            personalWiki.Name = EntityCache.GetPage(personalWikiId)?.Name;
        }
        else
        {
            personalWiki.Id = RootCategory.RootCategoryId;
            personalWiki.Name = RootCategory.Get.Name;
        }

        return new Breadcrumb
        {
            NewWikiId = currentWiki.Id,
            Items = breadcrumbItems,
            PersonalWiki = personalWiki,
            RootTopic = new BreadcrumbItem
            {
                Name = breadcrumb.Root.Text,
                Id = breadcrumb.Root.Page.Id
            },
            CurrentTopic = new BreadcrumbItem
            {
                Name = breadcrumb.Current.Text,
                Id = breadcrumb.Current.Page.Id
            },
            BreadcrumbHasGlobalWiki = breadcrumb.Items.Any(c => c.Page.Id == RootCategory.RootCategoryId),
            IsInPersonalWiki = _sessionUser.IsLoggedIn ? _sessionUser.User.StartTopicId == breadcrumb.Root.Page.Id : RootCategory.RootCategoryId == breadcrumb.Root.Page.Id
        };
    }

    public record struct Breadcrumb(
        int NewWikiId,
        BreadcrumbItem PersonalWiki,
        List<BreadcrumbItem> Items,
        BreadcrumbItem RootTopic,
        BreadcrumbItem CurrentTopic,
        bool BreadcrumbHasGlobalWiki,
        bool IsInPersonalWiki);

    public record struct BreadcrumbItem(string Name, int Id);

}