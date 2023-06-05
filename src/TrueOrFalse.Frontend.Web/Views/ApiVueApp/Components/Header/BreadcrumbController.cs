﻿using System;
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
        var defaultWikiId = SessionUserLegacy.IsLoggedIn ? SessionUserLegacy.User.StartTopicId : 1;
        SessionUserLegacy.SetWikiId(wikiId != 0 ? wikiId : defaultWikiId);
        var category = EntityCache.GetCategory(currentCategoryId);
        var currentWiki = CrumbtrailService.GetWiki(category);
        SessionUserLegacy.SetWikiId(currentWiki);

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
        if (SessionUserLegacy.IsLoggedIn)
        {
            var personalWikiId = SessionUserLegacy.User.StartTopicId;
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
            isInPersonalWiki = SessionUserLegacy.IsLoggedIn ? SessionUserLegacy.User.StartTopicId == breadcrumb.Root.Category.Id : RootCategory.RootCategoryId == breadcrumb.Root.Category.Id
        });
    }


    [HttpPost]
    public JsonResult GetPersonalWiki()
    {
        var topic = SessionUserLegacy.IsLoggedIn ? EntityCache.GetCategory(SessionUserLegacy.User.StartTopicId) : RootCategory.Get;
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