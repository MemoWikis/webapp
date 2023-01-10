﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NHibernate.Linq.Expressions;
using Seedworks.Lib;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;

public class SearchController : BaseController
{
    [HttpGet]
    public JsonResult All(string term, string type)
    {
        var categoryItems = new List<SearchCategoryItem>();
        var questionItems = new List<SearchQuestionItem>();
        var userItems = new List<SearchUserItem>();
        var elements = PrepareSearch.Go(term, type);

        if (elements.Categories.Any())
            AddCategoryItems(categoryItems, elements);

        if (elements.Questions.Any())
            AddQuestionItems(questionItems, elements);

        if (elements.Users.Any())
            AddUserItems(userItems, elements);

        return Json(new
        {
            categories = categoryItems,
            categoryCount = elements.CategoriesResultCount,
            questions = questionItems,
            questionCount = elements.QuestionsResultCount,
            users = userItems,
            userCount = elements.UsersResultCount,
            userSearchUrl = Links.UsersSearch(term)
        }, JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    public JsonResult Category(string term, int[] categoriesToFilter)
    {
        var items = new List<SearchCategoryItem>();
        var elements = PrepareSearch.GoAllCategories(term, categoriesToFilter);

        if (elements.Categories.Any())
            AddCategoryItems(items, elements);

        return Json(new
        {
            totalCount = elements.CategoriesResultCount,
            categories = items,
        }, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult CategoryInWiki(string term, int[] categoriesToFilter)
    {
        var items = new List<SearchCategoryItem>();
        var elements = PrepareSearch.GoAllCategories(term, categoriesToFilter);

        if (elements.Categories.Any())
            AddCategoryItems(items, elements);

        var wikiChildren = EntityCache.GetAllChildren(SessionUser.User.StartTopicId);
        items = items.Where(i => wikiChildren.Any(c => c.Id == i.Id)).ToList();

        return Json(new
        {
            totalCount = elements.CategoriesResultCount,
            categories = items,
        }, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult GetPersonalWikiData(int id)
    {
        if (EntityCache.GetAllChildren(id).Any(c => c.Id == SessionUser.User.StartTopicId))
            return Json(new
            {
                success = false,
            });

        var recentlyUsedRelationTargetTopicIds = new List<SearchCategoryItem>();

        if (SessionUser.User.RecentlyUsedRelationTargetTopicIds != null && SessionUser.User.RecentlyUsedRelationTargetTopicIds.Count > 0)
        {
            foreach (var categoryId in SessionUser.User.RecentlyUsedRelationTargetTopicIds)
            {
                var c = EntityCache.GetCategory(categoryId);
                recentlyUsedRelationTargetTopicIds.Add(FillSearchCategoryItem(c));
            }
        }

        var personalWiki = EntityCache.GetCategory(SessionUser.User.StartTopicId);
        
        return Json(new
        {
            success = true,
            personalWiki = FillSearchCategoryItem(personalWiki),
            addToWikiHistory = recentlyUsedRelationTargetTopicIds.ToArray()
        });
    }

    public static void AddCategoryItems(List<SearchCategoryItem> items, SearchBoxElements elements)
    {
        items.AddRange(
            elements.Categories.Where(PermissionCheck.CanView).Select(FillSearchCategoryItem));
    }

    public static SearchCategoryItem FillSearchCategoryItem(Category c)
    {
        return new SearchCategoryItem
        {
            Id = c.Id,
            Name = c.Name,
            Url = Links.CategoryDetail(c.Name, c.Id),
            QuestionCount = EntityCache.GetCategory(c.Id).GetCountQuestionsAggregated(),
            ImageUrl = new CategoryImageSettings(c.Id).GetUrl_128px(asSquare: true).Url,
            IconHtml = GetIconHtml(c),
            MiniImageUrl = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(c.Id, ImageType.Category))
                .GetImageUrl(30, true, false, ImageType.Category).Url,
            Visibility = (int)c.Visibility
        };
    }

    public static SearchCategoryItem FillSearchCategoryItem(CategoryCacheItem c)
    {
        return new SearchCategoryItem
        {
            Id = c.Id,
            Name = c.Name,
            Url = Links.CategoryDetail(c.Name, c.Id),
            QuestionCount = EntityCache.GetCategory(c.Id).GetCountQuestionsAggregated(),
            ImageUrl = new CategoryImageSettings(c.Id).GetUrl_128px(asSquare: true).Url,
            IconHtml = GetIconHtml(c),
            MiniImageUrl = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(c.Id, ImageType.Category))
                .GetImageUrl(30, true, false, ImageType.Category).Url,
            Visibility = (int)c.Visibility
        };
    }

    public static void AddQuestionItems(List<SearchQuestionItem> items, SearchBoxElements elements)
    {
        items.AddRange(
            elements.Questions.Where(q => PermissionCheck.CanView(q)).Select((q, index) => new SearchQuestionItem
            {
                Id = q.Id,
                Name = q.Text.Wrap(200),
                ImageUrl = new QuestionImageSettings(q.Id).GetUrl_50px_square().Url,
                Url = Links.AnswerQuestion(q, index, "searchbox")
            }));
    }

    public static void AddUserItems(List<SearchUserItem> items, SearchBoxElements elements)
    {
        items.AddRange(
            elements.Users.Select(u => new SearchUserItem
            {
                Id = u.Id,
                Name = u.Name,
                ImageUrl = new UserImageSettings(u.Id).GetUrl_50px_square(u).Url,
                Url = Links.UserDetail(u)
            }));
    }

    public static string GetIconHtml(Category category)
    {
        var iconHTML = "";
        switch (category.Type)
        {
            case CategoryType.Book:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case CategoryType.VolumeChapter:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case CategoryType.Magazine:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case CategoryType.MagazineArticle:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case CategoryType.MagazineIssue:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case CategoryType.WebsiteArticle:
                iconHTML = "<i class=\"fa fa-globe\">&nbsp;</i>";
                break;
            case CategoryType.Daily:
                iconHTML = "<i class=\"fa fa-newspaper-o\">&nbsp;</i>";
                break;
            case CategoryType.DailyIssue:
                iconHTML = "<i class=\"fa fa-newspaper-o\"&nbsp;></i>";
                break;
            case CategoryType.DailyArticle:
                iconHTML = "<i class=\"fa fa-newspaper-o\">&nbsp;</i>";
                break;
        }
        if (category.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Education)
            iconHTML = "<i class=\"fa fa-university\">&nbsp;</i>";

        return iconHTML;
    }

    public static string GetIconHtml(CategoryCacheItem category)
    {
        var iconHTML = "";
        switch (category.Type)
        {
            case CategoryType.Book:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case CategoryType.VolumeChapter:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case CategoryType.Magazine:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case CategoryType.MagazineArticle:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case CategoryType.MagazineIssue:
                iconHTML = "<i class=\"fa fa-book\">&nbsp;</i>";
                break;
            case CategoryType.WebsiteArticle:
                iconHTML = "<i class=\"fa fa-globe\">&nbsp;</i>";
                break;
            case CategoryType.Daily:
                iconHTML = "<i class=\"fa fa-newspaper-o\">&nbsp;</i>";
                break;
            case CategoryType.DailyIssue:
                iconHTML = "<i class=\"fa fa-newspaper-o\"&nbsp;></i>";
                break;
            case CategoryType.DailyArticle:
                iconHTML = "<i class=\"fa fa-newspaper-o\">&nbsp;</i>";
                break;
        }
        if (category.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Education)
            iconHTML = "<i class=\"fa fa-university\">&nbsp;</i>";

        return iconHTML;
    }
}