﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Utilities.ScheduledJobs;

namespace VueApp;

public class TopicRelationEditController : BaseController
{
    private readonly CategoryRepository _categoryRepository = Sl.CategoryRepo;
    private readonly IGlobalSearch _search;
    public TopicRelationEditController(IGlobalSearch search)
    {
        _search = search ?? throw new ArgumentNullException(nameof(search));
    }

    [HttpPost]
    public JsonResult ValidateName(string name)
    {
        var dummyTopic = new Category();
        dummyTopic.Name = name;
        dummyTopic.Type = CategoryType.Standard;
        var topicNameAllowed = new CategoryNameAllowed();
        if (topicNameAllowed.No(dummyTopic))
        {
            var topic = EntityCache.GetCategoryByName(name).FirstOrDefault();
            var url = topic.Visibility == CategoryVisibility.All ? Links.CategoryDetail(topic) : "";
            return Json(new
            {
                categoryNameAllowed = false,
                name,
                url,
                key = "nameIsTaken"
            });
        }

        if (topicNameAllowed.ForbiddenWords(name))   
        {
            return Json(new
            {
                categoryNameAllowed = false,
                name,
                key = "nameIsForbidden"
            });
        }

        return Json(new
        {
            categoryNameAllowed = true
        });
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult QuickCreate(string name, int parentTopicId)
    {
        var topic = new Category(name);
        ModifyRelationsForCategory.AddParentCategory(topic, parentTopicId);

        topic.Creator = Sl.UserRepo.GetById(SessionUser.UserId);
        topic.Type = CategoryType.Standard;
        topic.Visibility = CategoryVisibility.Owner;
        _categoryRepository.Create(topic);

        return Json(new
        {
            success = true,
            url = Links.CategoryDetail(topic),
            id = topic.Id
        });
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<JsonResult> SearchTopic(string term, int[] topicIdsToFilter = null)
    {
        var items = new List<SearchTopicItem>();
        var elements = await _search.GoAllCategories(term, topicIdsToFilter);

        if (elements.Categories.Any())
            SearchHelper.AddTopicItems(items, elements);

        return Json(new
        {
            totalCount = elements.CategoriesResultCount,
            topics = items,
        });
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<JsonResult> SearchTopicInPersonalWiki(string term, int[] topicIdsToFilter = null)
    {
        var items = new List<SearchTopicItem>();
        var elements = await _search.GoAllCategories(term, topicIdsToFilter);

        if (elements.Categories.Any())
            SearchHelper.AddTopicItems(items, elements);

        var wikiChildren = EntityCache.GetAllChildren(SessionUser.User.StartTopicId);
        items = items.Where(i => wikiChildren.Any(c => c.Id == i.Id)).ToList();

        return Json(new
        {
            totalCount = elements.CategoriesResultCount,
            topics = items,
        });
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult MoveChild(int childId, int parentIdToRemove, int parentIdToAdd)
    {
        if (childId == parentIdToRemove || childId == parentIdToAdd)
            return Json(new
            {
                success = false,
                key = "loopLink"
            });
        if (parentIdToRemove == RootCategory.RootCategoryId && !IsInstallationAdmin || parentIdToAdd == RootCategory.RootCategoryId && !IsInstallationAdmin)
            return Json(new
            {
                success = false,
                key = "parentIsRoot"
            });
        var json = AddChild(childId, parentIdToAdd, parentIdToRemove);
        RemoveParent(parentIdToRemove, childId, new int[] { parentIdToAdd, parentIdToRemove });
        return json;
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult AddChild(int childId, int parentId, int parentIdToRemove = -1, bool redirectToParent = false, bool addIdToWikiHistory = false)
    {
        if (childId == parentId)
            return Json(new
            {
                success = false,
                key = "loopLink"
            });
        if (parentId == RootCategory.RootCategoryId && !IsInstallationAdmin)
            return Json(new
            {
                success = false,
                key = "parentIsRoot"
            });
        var children = EntityCache.GetAllChildren(parentId, true);
        var isChildrenLinked = children.Any(c => c.Id == childId) && children.All(c => c.Id != parentIdToRemove);

        if (isChildrenLinked)
            return Json(new
            {
                success = false,
                key = "isAlreadyLinkedAsChild"
            });
        var selectedTopicIsParent = GraphService.GetAllParentsFromEntityCache(parentId)
            .Any(c => c.Id == childId);

        if (selectedTopicIsParent)
        {
            Logg.r().Error("Child is Parent ");
            return Json(new
            {
                success = false,
                key = "childIsParent"
            });
        }

        if (addIdToWikiHistory)
            RecentlyUsedRelationTargets.Add(SessionUser.UserId, parentId);

        var child = EntityCache.GetCategory(childId);
        ModifyRelationsEntityCache.AddParent(child, parentId);
        JobScheduler.StartImmediately_ModifyCategoryRelation(childId, parentId);
        EntityCache.GetCategory(parentId).CachedData.AddChildId(childId);
        EntityCache.GetCategory(parentId).DirectChildrenIds = EntityCache.GetChildren(parentId).Select(cci => cci.Id).ToList();

        if (redirectToParent)
            return Json(new
            {
                success = true,
                url = Links.CategoryDetail(EntityCache.GetCategory(parentId)),
                id = parentId
            });

        return Json(new
        {
            success = true,
            url = Links.CategoryDetail(child),
            id = childId
        });
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult RemoveParent(int parentIdToRemove, int childId, int[] affectedParentIdsByMove = null)
    {
        var parentHasBeenRemoved = ModifyRelationsForCategory.RemoveChildCategoryRelation(parentIdToRemove, childId);
        if (!parentHasBeenRemoved)
            return Json(new
            {
                success = false,
                key = "noRemainingParents",
            });

        var parent = _categoryRepository.GetById(parentIdToRemove);
        _categoryRepository.Update(parent, SessionUser.User, type: CategoryChangeType.Relations);
        var child = _categoryRepository.GetById(childId);
        if (affectedParentIdsByMove != null)
            _categoryRepository.Update(child, SessionUser.User, type: CategoryChangeType.Moved, affectedParentIdsByMove: affectedParentIdsByMove);
        else
            _categoryRepository.Update(child, SessionUser.User, type: CategoryChangeType.Relations);
        EntityCache.GetCategory(parentIdToRemove).CachedData.RemoveChildId(childId);
        EntityCache.GetCategory(parentIdToRemove).DirectChildrenIds = EntityCache.GetChildren(parentIdToRemove).Select(cci => cci.Id).ToList();
        return Json(new
        {
            success = true,
            key = "unlinked"
        });
    }
}