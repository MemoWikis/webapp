using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace VueApp;
public class BreadcrumbController(
    SessionUser _sessionUser,
    CrumbtrailService _crumbtrailService) : Controller
{
    public readonly record struct GetBreadcrumbParam(int WikiId, int CurrentPageId);

    [HttpPost]
    public Breadcrumb GetBreadcrumb([FromBody] GetBreadcrumbParam param)
    {
        var wikiId = param.WikiId;
        int currentPageId = param.CurrentPageId;

        var defaultWikiId = _sessionUser.IsLoggedIn ? _sessionUser.User.StartPageId : 1;
        _sessionUser.SetWikiId(wikiId != 0 ? wikiId : defaultWikiId);
        var page = EntityCache.GetPage(currentPageId);
        var currentWiki = _crumbtrailService.GetWiki(page, _sessionUser);
        _sessionUser.SetWikiId(currentWiki);

        var breadcrumb = _crumbtrailService.BuildCrumbtrail(page, currentWiki);

        return GetBreadcrumbItems(breadcrumb, currentWiki);
    }

    [HttpGet]
    public BreadcrumbItem GetPersonalWiki()
    {
        var page = _sessionUser.IsLoggedIn ? EntityCache.GetPage(_sessionUser.User.StartPageId) : RootPage.Get;
        return new BreadcrumbItem
        {
            Name = page.Name,
            Id = page.Id
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
            var personalWikiId = _sessionUser.User.StartPageId;
            personalWiki.Id = personalWikiId;
            personalWiki.Name = EntityCache.GetPage(personalWikiId)?.Name;
        }
        else
        {
            personalWiki.Id = RootPage.RootCategoryId;
            personalWiki.Name = RootPage.Get.Name;
        }

        return new Breadcrumb
        {
            NewWikiId = currentWiki.Id,
            Items = breadcrumbItems,
            PersonalWiki = personalWiki,
            RootPage = new BreadcrumbItem
            {
                Name = breadcrumb.Root.Text,
                Id = breadcrumb.Root.Page.Id
            },
            CurrentPage = new BreadcrumbItem
            {
                Name = breadcrumb.Current.Text,
                Id = breadcrumb.Current.Page.Id
            },
            BreadcrumbHasGlobalWiki = breadcrumb.Items.Any(c => c.Page.Id == RootPage.RootCategoryId),
            IsInPersonalWiki = _sessionUser.IsLoggedIn ? _sessionUser.User.StartPageId == breadcrumb.Root.Page.Id : RootPage.RootCategoryId == breadcrumb.Root.Page.Id
        };
    }

    public record struct Breadcrumb(
        int NewWikiId,
        BreadcrumbItem PersonalWiki,
        List<BreadcrumbItem> Items,
        BreadcrumbItem RootPage,
        BreadcrumbItem CurrentPage,
        bool BreadcrumbHasGlobalWiki,
        bool IsInPersonalWiki);

    public record struct BreadcrumbItem(string Name, int Id);

}