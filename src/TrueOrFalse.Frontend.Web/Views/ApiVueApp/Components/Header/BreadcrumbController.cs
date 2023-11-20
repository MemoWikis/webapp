using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class BreadcrumbController : BaseController
{
    private readonly CrumbtrailService _crumbtrailService;

    public BreadcrumbController(SessionUser sessionUser, CrumbtrailService crumbtrailService) : base(sessionUser)
    {
        _crumbtrailService = crumbtrailService;
    }

    public readonly record struct GetBreadcrumbParam(int wikiId, int currentCategoryId);
    [HttpPost]
    public JsonResult GetBreadcrumb([FromBody] GetBreadcrumbParam param)
    {
        var wikiId = param.wikiId;
        int currentCategoryId = param.currentCategoryId;

        var defaultWikiId = IsLoggedIn ? _sessionUser.User.StartTopicId : 1;
        _sessionUser.SetWikiId(wikiId != 0 ? wikiId : defaultWikiId);
        var category = EntityCache.GetCategory(currentCategoryId);
        var currentWiki = _crumbtrailService.GetWiki(category,_sessionUser);
        _sessionUser.SetWikiId(currentWiki);

        var breadcrumb = _crumbtrailService.BuildCrumbtrail(category, currentWiki);

        var breadcrumbItems = new List<BreadcrumbItem>();

        foreach (var item in breadcrumb.Items)
        {
            if (item.Category.Id != breadcrumb.Root.Category.Id)
                breadcrumbItems.Add(new BreadcrumbItem
                {
                    Name = item.Text,
                    Id = item.Category.Id
                });
        }

        var personalWiki = new BreadcrumbItem();
        if (_sessionUser.IsLoggedIn)
        {
            var personalWikiId = _sessionUser.User.StartTopicId;
            personalWiki.Id = personalWikiId;
            personalWiki.Name = EntityCache.GetCategory(personalWikiId).Name;
        }
        else
        {
            personalWiki.Id = RootCategory.RootCategoryId;
            personalWiki.Name = RootCategory.Get.Name;
        }

        return Json(new Breadcrumb
        {
            newWikiId = currentWiki.Id,
            items = breadcrumbItems,
            personalWiki = personalWiki,
            rootTopic = new BreadcrumbItem
            {
                Name = breadcrumb.Root.Text,
                Id = breadcrumb.Root.Category.Id
            },
            currentTopic = new BreadcrumbItem
            {
                Name = breadcrumb.Current.Text,
                Id = breadcrumb.Current.Category.Id
            },
            breadcrumbHasGlobalWiki = breadcrumb.Items.Any(c => c.Category.Id == RootCategory.RootCategoryId),
            isInPersonalWiki = _sessionUser.IsLoggedIn ? _sessionUser.User.StartTopicId == breadcrumb.Root.Category.Id : RootCategory.RootCategoryId == breadcrumb.Root.Category.Id
        });
    }

    [HttpGet]
    public JsonResult GetPersonalWiki()
    {
        var topic = _sessionUser.IsLoggedIn ? EntityCache.GetCategory(_sessionUser.User.StartTopicId) : RootCategory.Get;
        return Json(new BreadcrumbItem
        {
            Name = topic.Name,
            Id = topic.Id
        });
    }

    public class Breadcrumb
    {
        public int newWikiId { get; set; }
        public BreadcrumbItem personalWiki { get; set; }
        public List<BreadcrumbItem> items { get; set; }
        public BreadcrumbItem rootTopic { get; set; }
        public BreadcrumbItem currentTopic { get; set; }
        public bool breadcrumbHasGlobalWiki { get; set; }
        public bool isInPersonalWiki { get; set; }
    }

    public class BreadcrumbItem
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}