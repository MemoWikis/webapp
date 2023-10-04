using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TrueOrFalse.Domain;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Utilities.ScheduledJobs;

namespace VueApp;

public class EditControllerLogic(IGlobalSearch search,
    PermissionCheck permissionCheck,
    SessionUser sessionUser,
    CategoryRepository categoryRepository,
    ImageMetaDataReadingRepo imageMetaDataReadingRepo,
    UserReadingRepo userReadingRepo,
    UserWritingRepo userWritingRepo,
    IActionContextAccessor actionContextAccessor,
    IHttpContextAccessor httpContextAccessor,
    IWebHostEnvironment webHostEnvironment,
    Logg logg,
    QuestionReadingRepo questionReadingRepo) : IRegisterAsInstancePerLifetime
{
  


    public dynamic ValidateName(string name)
    {
        var dummyTopic = new Category();
        dummyTopic.Name = name;
        dummyTopic.Type = CategoryType.Standard;
        var topicNameAllowed = new CategoryNameAllowed();
        if (topicNameAllowed.No(dummyTopic, categoryRepository))
        {
            var topic = EntityCache.GetCategoryByName(name).FirstOrDefault();
            var url = topic.Visibility == CategoryVisibility.All ? new Links(actionContextAccessor, httpContextAccessor).CategoryDetail(topic) : "";
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
        if (!new LimitCheck(httpContextAccessor,
                webHostEnvironment, 
                logg, 
                sessionUser).CanSavePrivateTopic(true))
        {
            return new
            {
                success = false,
                key = "cantSavePrivateTopic"
            }; 
        }

        var topic = new Category(name, sessionUser.UserId);
        new ModifyRelationsForCategory(categoryRepository).AddParentCategory(topic, parentTopicId);

        topic.Creator = userReadingRepo.GetById(sessionUser.UserId);
        topic.Type = CategoryType.Standard;
        topic.Visibility = CategoryVisibility.Owner;
        categoryRepository.Create(topic);

        return new
        {
            success = true,
            name = topic.Name,
            id = topic.Id
        };
    }

    public async Task<dynamic> SearchTopic(string term, int[] topicIdsToFilter = null)
    {
        var items = new List<SearchTopicItem>();
        var elements = await search.GoAllCategories(term, topicIdsToFilter);

        if (elements.Categories.Any())
             new SearchHelper(imageMetaDataReadingRepo,
            actionContextAccessor,
        httpContextAccessor,
            webHostEnvironment,
            questionReadingRepo)
                 .AddTopicItems(items, elements, permissionCheck, sessionUser.UserId);

        return new
        {
            totalCount = elements.CategoriesResultCount,
            topics = items,
        };
    }

    public async Task<dynamic> SearchTopicInPersonalWiki(string term, int[] topicIdsToFilter = null)
    {
        var items = new List<SearchTopicItem>();
        var elements = await search.GoAllCategories(term, topicIdsToFilter);

        if (elements.Categories.Any())
            new SearchHelper(imageMetaDataReadingRepo,
                actionContextAccessor,
                httpContextAccessor,
                webHostEnvironment,
                questionReadingRepo)
                .AddTopicItems(items, elements, permissionCheck, sessionUser.UserId);

        var wikiChildren = EntityCache.GetAllChildren(sessionUser.User.StartTopicId);
        items = items.Where(i => wikiChildren.Any(c => c.Id == i.Id)).ToList();

        return new
        {
            totalCount = elements.CategoriesResultCount,
            topics = items,
        };
    }

    public RequestResult MoveChild(int childId, int parentIdToRemove, int parentIdToAdd)
    {
        if (childId == parentIdToRemove || childId == parentIdToAdd)
            return new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Category.LoopLink
            };
        if (parentIdToRemove == RootCategory.RootCategoryId && !sessionUser.IsInstallationAdmin || parentIdToAdd == RootCategory.RootCategoryId && !sessionUser.IsInstallationAdmin)
            return new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Category.ParentIsRoot
            };
        var json = AddChild(childId, parentIdToAdd, parentIdToRemove);
        RemoveParent(parentIdToRemove, childId, new int[] { parentIdToAdd, parentIdToRemove });
        return json;
    }

    public RequestResult AddChild(int childId, int parentId, int parentIdToRemove = -1, bool addIdToWikiHistory = false)
    {
        if (childId == parentId)
            return new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Category.LoopLink
            };
        if (parentId == RootCategory.RootCategoryId && !sessionUser.IsInstallationAdmin)
            return new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Category.ParentIsRoot
            };
        var parent = EntityCache.GetCategory(parentId);

        if (parent.DirectChildrenIds.Any(id => id == childId))
            return new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Category.IsAlreadyLinkedAsChild
            };
        var selectedTopicIsParent = GraphService.GetAllParentsFromEntityCache(parentId)
            .Any(c => c.Id == childId);

        if (selectedTopicIsParent)
        {
            Logg.r.Error("Child is Parent ");
            return new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Category.ChildIsParent
            };
        }

        if (addIdToWikiHistory)
            RecentlyUsedRelationTargets.Add(sessionUser.UserId, 
                parentId, 
                userWritingRepo,
                httpContextAccessor,
                webHostEnvironment);

        var child = EntityCache.GetCategory(childId);
        ModifyRelationsEntityCache.AddParent(child, parentId);
        JobScheduler.StartImmediately_ModifyCategoryRelation(childId, parentId);
        EntityCache.GetCategory(parentId).CachedData.AddChildId(childId);
        EntityCache.GetCategory(parentId).DirectChildrenIds = EntityCache.GetChildren(parentId).Select(cci => cci.Id).ToList();

        return new RequestResult
        {
            success = true,
            data = new
            {
                name = EntityCache.GetCategory(parentId).Name,
                id = childId
            }
        };
    }

    public RequestResult RemoveParent(int parentIdToRemove, int childId, int[] affectedParentIdsByMove = null)
    {
        var parentHasBeenRemoved = new ModifyRelationsForCategory(categoryRepository).RemoveChildCategoryRelation(parentIdToRemove, childId,permissionCheck);
        if (!parentHasBeenRemoved)
            return new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Category.NoRemainingParents
            };

        var parent = categoryRepository.GetById(parentIdToRemove);
        categoryRepository.Update(parent, sessionUser.User, type: CategoryChangeType.Relations);
        var child = categoryRepository.GetById(childId);
        if (affectedParentIdsByMove != null)
            categoryRepository.Update(child, sessionUser.User, type: CategoryChangeType.Moved, affectedParentIdsByMove: affectedParentIdsByMove);
        else
            categoryRepository.Update(child, sessionUser.User, type: CategoryChangeType.Relations);
        EntityCache.GetCategory(parentIdToRemove).CachedData.RemoveChildId(childId);
        EntityCache.GetCategory(parentIdToRemove).DirectChildrenIds = EntityCache.GetChildren(parentIdToRemove).Select(cci => cci.Id).ToList();
        return new RequestResult
        {
            success = true,
            messageKey = FrontendMessageKeys.Success.Category.Unlinked
        };
    }
}