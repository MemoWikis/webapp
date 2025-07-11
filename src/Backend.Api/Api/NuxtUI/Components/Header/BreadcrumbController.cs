﻿public class BreadcrumbController(
    SessionUser _sessionUser,
    CrumbtrailService _crumbtrailService,
    IHttpContextAccessor _httpContextAccessor) : ApiBaseController
{
    public readonly record struct GetBreadcrumbParam(int WikiId, int CurrentPageId);

    [HttpPost]
    public Breadcrumb GetBreadcrumb([FromBody] GetBreadcrumbParam param)
    {
        var wikiId = param.WikiId;
        int currentPageId = param.CurrentPageId;

        var defaultWikiId = _sessionUser.FirstWikiId();
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
        var page = _sessionUser.IsLoggedIn
            ? _sessionUser.User.FirstWiki()
            : FeaturedPage.GetRootPage;

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

        return new Breadcrumb
        {
            NewWikiId = currentWiki.Id,
            Items = breadcrumbItems,
            CurrentWiki = new TinyPageModel
            {
                Name = breadcrumb.Root.Text,
                Id = breadcrumb.Root.Page.Id,
                ImgUrl = new PageImageSettings(breadcrumb.Root.Page.Id,
                        _httpContextAccessor)
                    .GetUrl_128px(true)
                    .Url
            },
            CurrentPage = new BreadcrumbItem
            {
                Name = breadcrumb.Current.Text,
                Id = breadcrumb.Current.Page.Id
            },
            BreadcrumbHasGlobalWiki = breadcrumb.Items.Any(c => c.Page.Id == FeaturedPage.RootPageId),
            IsInPersonalWiki = _sessionUser.IsLoggedIn && breadcrumb.Root.Page.Id == _sessionUser.User.FirstWikiId
        };
    }

    public record struct Breadcrumb(
        int NewWikiId,
        List<BreadcrumbItem> Items,
        TinyPageModel CurrentWiki,
        BreadcrumbItem CurrentPage,
        bool BreadcrumbHasGlobalWiki,
        bool IsInPersonalWiki);

    public record struct BreadcrumbItem(string Name, int Id);

    public record struct TinyPageModel(string Name, int Id, string ImgUrl);
}
