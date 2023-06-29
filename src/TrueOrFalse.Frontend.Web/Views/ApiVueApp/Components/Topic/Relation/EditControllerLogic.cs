using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueOrFalse.Domain;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Utilities.ScheduledJobs;

namespace VueApp;

public class EditControllerLogic
{
    private readonly CategoryRepository _categoryRepository; 
    private readonly IGlobalSearch _search;
    private readonly bool _isInstallationAdmin;
    private readonly PermissionCheck _permissionCheck;
    private readonly int _sessionUserId;
    private readonly SessionUser _sessionUser;

    public EditControllerLogic(IGlobalSearch search,
        bool isInstallationAdmin,
        PermissionCheck permissionCheck,
        SessionUser sessionUser,
        CategoryRepository categoryRepository)
    {
        _search = search; 
        _isInstallationAdmin = isInstallationAdmin;
        _permissionCheck = permissionCheck;
        _sessionUserId = sessionUser.UserId;
        _sessionUser = sessionUser;
        _categoryRepository = categoryRepository;
    }

    public dynamic ValidateName(string name)
    {
        var dummyTopic = new Category();
        dummyTopic.Name = name;
        dummyTopic.Type = CategoryType.Standard;
        var topicNameAllowed = new CategoryNameAllowed();
        if (topicNameAllowed.No(dummyTopic))
        {
            var topic = EntityCache.GetCategoryByName(name).FirstOrDefault();
            var url = topic.Visibility == CategoryVisibility.All ? Links.CategoryDetail(topic) : "";
            return new
            {
                categoryNameAllowed = false,
                name,
                url,
                key = "nameIsTaken"
            };
        }

        if (topicNameAllowed.ForbiddenWords(name))
        {
            return new
            {
                categoryNameAllowed = false,
                name,
                key = "nameIsForbidden"
            };
        }

        return new
        {
            categoryNameAllowed = true
        };
    }

    public dynamic QuickCreate(string name, int parentTopicId, SessionUser sessionUser)
    {
        if (!LimitCheck.CanSavePrivateTopic(sessionUser))
        {
            return new
            {
                success = false,
                key = "cantSavePrivateTopic"
            }; 
        }

        var topic = new Category(name,_sessionUserId);
        new ModifyRelationsForCategory(_categoryRepository).AddParentCategory(topic, parentTopicId);

        topic.Creator = Sl.UserRepo.GetById(_sessionUserId);
        topic.Type = CategoryType.Standard;
        topic.Visibility = CategoryVisibility.Owner;
        _categoryRepository.Create(topic);

        return new
        {
            success = true,
            url = Links.CategoryDetail(topic),
            id = topic.Id
        };
    }

    public async Task<dynamic> SearchTopic(string term, int[] topicIdsToFilter = null)
    {
        var items = new List<SearchTopicItem>();
        var elements = await _search.GoAllCategories(term, topicIdsToFilter);

        if (elements.Categories.Any())
            SearchHelper.AddTopicItems(items, elements, _permissionCheck, _sessionUserId);

        return new
        {
            totalCount = elements.CategoriesResultCount,
            topics = items,
        };
    }

    public async Task<dynamic> SearchTopicInPersonalWiki(string term, int[] topicIdsToFilter = null)
    {
        var items = new List<SearchTopicItem>();
        var elements = await _search.GoAllCategories(term, topicIdsToFilter);

        if (elements.Categories.Any())
            SearchHelper.AddTopicItems(items, elements, _permissionCheck, _sessionUserId);

        var wikiChildren = EntityCache.GetAllChildren(_sessionUser.User.StartTopicId);
        items = items.Where(i => wikiChildren.Any(c => c.Id == i.Id)).ToList();

        return new
        {
            totalCount = elements.CategoriesResultCount,
            topics = items,
        };
    }

    public dynamic MoveChild(int childId, int parentIdToRemove, int parentIdToAdd)
    {
        if (childId == parentIdToRemove || childId == parentIdToAdd)
            return new
            {
                success = false,
                key = "loopLink"
            };
        if (parentIdToRemove == RootCategory.RootCategoryId && !_isInstallationAdmin || parentIdToAdd == RootCategory.RootCategoryId && !_isInstallationAdmin)
            return new
            {
                success = false,
                key = "parentIsRoot"
            };
        var json = AddChild(childId, parentIdToAdd, parentIdToRemove);
        RemoveParent(parentIdToRemove, childId, new int[] { parentIdToAdd, parentIdToRemove });
        return json;
    }

    public dynamic AddChild(int childId, int parentId, int parentIdToRemove = -1, bool redirectToParent = false, bool addIdToWikiHistory = false)
    {
        if (childId == parentId)
            return new
            {
                success = false,
                key = "loopLink"
            };
        if (parentId == RootCategory.RootCategoryId && !_isInstallationAdmin)
            return new
            {
                success = false,
                key = "parentIsRoot"
            };
        var children = EntityCache.GetAllChildren(parentId, true);
        var isChildrenLinked = children.Any(c => c.Id == childId) && children.All(c => c.Id != parentIdToRemove);

        if (isChildrenLinked)
            return new
            {
                success = false,
                key = "isAlreadyLinkedAsChild"
            };
        var selectedTopicIsParent = GraphService.GetAllParentsFromEntityCache(parentId)
            .Any(c => c.Id == childId);

        if (selectedTopicIsParent)
        {
            Logg.r().Error("Child is Parent ");
            return new
            {
                success = false,
                key = "childIsParent"
            };
        }

        if (addIdToWikiHistory)
            RecentlyUsedRelationTargets.Add(_sessionUserId, parentId);

        var child = EntityCache.GetCategory(childId);
        ModifyRelationsEntityCache.AddParent(child, parentId);
        JobScheduler.StartImmediately_ModifyCategoryRelation(childId, parentId);
        EntityCache.GetCategory(parentId).CachedData.AddChildId(childId);
        EntityCache.GetCategory(parentId).DirectChildrenIds = EntityCache.GetChildren(parentId).Select(cci => cci.Id).ToList();

        if (redirectToParent)
            return new
            {
                success = true,
                url = Links.CategoryDetail(EntityCache.GetCategory(parentId)),
                id = parentId
            };

        return new
        {
            success = true,
            url = Links.CategoryDetail(child),
            id = childId
        };
    }

    public dynamic RemoveParent(int parentIdToRemove, int childId, int[] affectedParentIdsByMove = null)
    {
        var parentHasBeenRemoved = new ModifyRelationsForCategory(_categoryRepository).RemoveChildCategoryRelation(parentIdToRemove, childId,_permissionCheck);
        if (!parentHasBeenRemoved)
            return new
            {
                success = false,
                key = "noRemainingParents",
            };

        var parent = _categoryRepository.GetById(parentIdToRemove);
        _categoryRepository.Update(parent, _sessionUser.User, type: CategoryChangeType.Relations);
        var child = _categoryRepository.GetById(childId);
        if (affectedParentIdsByMove != null)
            _categoryRepository.Update(child, _sessionUser.User, type: CategoryChangeType.Moved, affectedParentIdsByMove: affectedParentIdsByMove);
        else
            _categoryRepository.Update(child, _sessionUser.User, type: CategoryChangeType.Relations);
        EntityCache.GetCategory(parentIdToRemove).CachedData.RemoveChildId(childId);
        EntityCache.GetCategory(parentIdToRemove).DirectChildrenIds = EntityCache.GetChildren(parentIdToRemove).Select(cci => cci.Id).ToList();
        return new
        {
            success = true,
            key = "unlinked"
        };
    }
}