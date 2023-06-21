﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;

public class SearchController : BaseController
{

    private readonly IGlobalSearch _search;
    private readonly PermissionCheck _permissionCheck;

    public SearchController(IGlobalSearch search,
        SessionUser sessionUser,
        PermissionCheck permissionCheck) :base(sessionUser)
    {
        _search = search ?? throw new ArgumentNullException(nameof(search));
        _permissionCheck = permissionCheck;
    }

    [HttpGet]
    public async Task<JsonResult> All(string term, string type)
    {
        var topicItems = new List<SearchTopicItem>();
        var questionItems = new List<SearchQuestionItem>();
        var userItems = new List<SearchUserItem>();
        var elements = await _search.Go(term, type);

        if (elements.Categories.Any())
            SearchHelper.AddTopicItems(topicItems, elements,_permissionCheck, UserId);

        if (elements.Questions.Any())
            SearchHelper.AddQuestionItems(questionItems, elements,_permissionCheck);

        if (elements.Users.Any())
            SearchHelper.AddUserItems(userItems, elements);

        return Json(new
        {
            topics = topicItems,
            topicCount = elements.CategoriesResultCount,
            questions = questionItems,
            questionCount = elements.QuestionsResultCount,
            users = userItems,
            userCount = elements.UsersResultCount,
            userSearchUrl = Links.UsersSearch(term)
        }, JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    public async Task<JsonResult> Topic(string term, int[] topicIdsToFilter = null)
    {
        var items = new List<SearchTopicItem>();
        var elements = await _search.GoAllCategories(term, topicIdsToFilter);

        if (elements.Categories.Any())
            SearchHelper.AddTopicItems(items, elements,_permissionCheck, UserId);

        return Json(new
        {
            totalCount = elements.CategoriesResultCount,
            topics = items,
        }, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult GetPersonalWikiData(int id)
    {
        if (EntityCache.GetAllChildren(id).Any(c => c.Id == SessionUserLegacy.User.StartTopicId))
            return Json(new
            {
                success = false,
            });

        var recentlyUsedRelationTargetTopicIds = new List<SearchCategoryItem>();

        if (SessionUserLegacy.User.RecentlyUsedRelationTargetTopicIds != null && SessionUserLegacy.User.RecentlyUsedRelationTargetTopicIds.Count > 0)
        {
            foreach (var categoryId in SessionUserLegacy.User.RecentlyUsedRelationTargetTopicIds)
            {
                var c = EntityCache.GetCategory(categoryId);
                recentlyUsedRelationTargetTopicIds.Add(SearchHelper.FillSearchCategoryItem(c, UserId));
            }
        }

        var personalWiki = EntityCache.GetCategory(SessionUserLegacy.User.StartTopicId);

        return Json(new
        {
            success = true,
            personalWiki = SearchHelper.FillSearchCategoryItem(personalWiki, UserId),
            addToWikiHistory = recentlyUsedRelationTargetTopicIds.ToArray()
        });
    }
}