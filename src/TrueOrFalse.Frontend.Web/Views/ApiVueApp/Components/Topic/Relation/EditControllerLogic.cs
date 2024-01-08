using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Utilities.ScheduledJobs;

namespace VueApp;

public class EditControllerLogic : IRegisterAsInstancePerLifetime
{
    private readonly IGlobalSearch _search;
    private readonly PermissionCheck _permissionCheck;
    private readonly SessionUser _sessionUser;
    private readonly CategoryRepository _categoryRepository;
    private readonly ImageMetaDataReadingRepo _imageMetaDataReadingRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly UserWritingRepo _userWritingRepo;
    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly Logg _logg;
    private readonly QuestionReadingRepo _questionReadingRepo;

    public EditControllerLogic(IGlobalSearch search,
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
        QuestionReadingRepo questionReadingRepo)
    {
        _search = search;
        _permissionCheck = permissionCheck;
        _sessionUser = sessionUser;
        _categoryRepository = categoryRepository;
        _imageMetaDataReadingRepo = imageMetaDataReadingRepo;
        _userReadingRepo = userReadingRepo;
        _userWritingRepo = userWritingRepo;
        _actionContextAccessor = actionContextAccessor;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _logg = logg;
        _questionReadingRepo = questionReadingRepo;
    }

    public RequestResult ValidateName(string name)
    {
        var dummyTopic = new Category();
        dummyTopic.Name = name;
        dummyTopic.Type = CategoryType.Standard;
        var topicNameAllowed = new CategoryNameAllowed();
        if (topicNameAllowed.No(dummyTopic, _categoryRepository))
        {
            var topic = EntityCache.GetCategoryByName(name).FirstOrDefault();
            var url = topic.Visibility == CategoryVisibility.All ? new Links(_actionContextAccessor, _httpContextAccessor).CategoryDetail(topic) : "";
            return new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Category.NameIsTaken,
                data = new
                {
                    categoryNameAllowed = false,
                    name,
                    url
                }
            };
        }

        if (topicNameAllowed.ForbiddenWords(name))
        {
            return new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Category.NameIsForbidden,
                data = new
                {
                    categoryNameAllowed = false,
                    name,
                }
            };
        }

        return new RequestResult
        {
            success = true
        };
    }

    public async Task<dynamic> SearchTopic(string term, int[] topicIdsToFilter = null)
    {
        var items = new List<SearchTopicItem>();
        var elements = await _search.GoAllCategories(term, topicIdsToFilter);

        if (elements.Categories.Any())
            new SearchHelper(_imageMetaDataReadingRepo,
           _httpContextAccessor,
           _questionReadingRepo)
                .AddTopicItems(items, elements, _permissionCheck, _sessionUser.UserId);

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
            new SearchHelper(_imageMetaDataReadingRepo,
                _httpContextAccessor,
                _questionReadingRepo)
                .AddTopicItems(items, elements, _permissionCheck, _sessionUser.UserId);

        var wikiChildren = EntityCache.GetAllChildren(_sessionUser.User.StartTopicId);
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
        if (parentIdToRemove == RootCategory.RootCategoryId && !_sessionUser.IsInstallationAdmin || parentIdToAdd == RootCategory.RootCategoryId && !_sessionUser.IsInstallationAdmin)
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
        if (parentId == RootCategory.RootCategoryId && !_sessionUser.IsInstallationAdmin)
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
            RecentlyUsedRelationTargets.Add(_sessionUser.UserId,
                parentId,
                _userWritingRepo,
                _httpContextAccessor,
                _webHostEnvironment);

        var child = EntityCache.GetCategory(childId);
        ModifyRelationsEntityCache.AddParent(child, parentId);
        JobScheduler.StartImmediately_ModifyCategoryRelation(childId, parentId, _sessionUser.UserId);
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
        if (!_permissionCheck.CanEditCategory(parentIdToRemove) && !_permissionCheck.CanEditCategory(childId))
            return new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Category.MissingRights
            };

        var parentHasBeenRemoved = new ModifyRelationsForCategory(_categoryRepository).RemoveChildCategoryRelation(parentIdToRemove, childId, _permissionCheck);
        if (!parentHasBeenRemoved)
            return new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Category.NoRemainingParents
            };

        var parent = _categoryRepository.GetById(parentIdToRemove);
        _categoryRepository.Update(parent, _sessionUser.UserId, type: CategoryChangeType.Relations);
        var child = _categoryRepository.GetById(childId);
        if (affectedParentIdsByMove != null)
            _categoryRepository.Update(child, _sessionUser.UserId, type: CategoryChangeType.Moved, affectedParentIdsByMove: affectedParentIdsByMove);
        else
            _categoryRepository.Update(child, _sessionUser.UserId, type: CategoryChangeType.Relations);
        EntityCache.GetCategory(parentIdToRemove).DirectChildrenIds = EntityCache.GetChildren(parentIdToRemove).Select(cci => cci.Id).ToList();
        return new RequestResult
        {
            success = true,
            messageKey = FrontendMessageKeys.Success.Category.Unlinked
        };
    }
}