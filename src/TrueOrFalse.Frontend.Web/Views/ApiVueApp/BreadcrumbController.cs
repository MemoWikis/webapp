using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FluentNHibernate.Utils;

namespace VueApp;

public class BreadcrumbController : BaseController
{
    [HttpPost]
    public JsonResult GetBreadcrumb(int wikiId, int currentCategoryId)
    {
        var defaultWikiId = SessionUser.IsLoggedIn ? SessionUser.User.StartTopicId : 1;
        SessionUser.SetWikiId(wikiId != 0 ? wikiId : defaultWikiId);
        var category = EntityCache.GetCategory(currentCategoryId);
        var currentWiki = CrumbtrailService.GetWiki(category);
        SessionUser.SetWikiId(currentWiki);

        var breadcrumb = CrumbtrailService.BuildCrumbtrail(category, currentWiki);

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
        if (SessionUser.IsLoggedIn)
        {
            var personalWikiId = SessionUser.User.StartTopicId;
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
            items = breadcrumbItems.ToArray(),
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
            isInPersonalWiki = SessionUser.IsLoggedIn ? SessionUser.User.StartTopicId == breadcrumb.Root.Category.Id : RootCategory.RootCategoryId == breadcrumb.Root.Category.Id
        });
    }

    public class Breadcrumb
    {
        public int newWikiId { get; set; }
        public BreadcrumbItem personalWiki { get; set; }
        public BreadcrumbItem[] items { get; set; }
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