using Microsoft.AspNetCore.Hosting;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace VueApp;

public class ChildModifier(
    PermissionCheck _permissionCheck,
    SessionUser _sessionUser,
    CategoryRepository _categoryRepository,
    UserWritingRepo _userWritingRepo,
    IHttpContextAccessor _httpContextAccessor,
    IWebHostEnvironment _webHostEnvironment,
    CategoryRelationRepo _categoryRelationRepo)
{
    public readonly record struct AddChildResult(
        bool Success,
        string MessageKey,
        TinyTopicItem Data);

    public readonly record struct TinyTopicItem(int Id, string Name);

    public AddChildResult AddChild(
        int childId,
        int parentId,
        bool addIdToWikiHistory = false)
    {
        if (childId == parentId)
            return new AddChildResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Category.LoopLink
            };
        if (parentId == RootCategory.RootCategoryId && !_sessionUser.IsInstallationAdmin)
            return new AddChildResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Category.ParentIsRoot
            };

        var parent = EntityCache.GetCategory(parentId);

        if (parent.ChildRelations.Any(r => r.ChildId == childId))
            return new AddChildResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Category.IsAlreadyLinkedAsChild
            };
        var selectedTopicIsParent = GraphService.Ascendants(parentId)
            .Any(c => c.Id == childId);

        if (selectedTopicIsParent)
        {
            Logg.r.Error("Child is Parent ");
            return new AddChildResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Category.ChildIsParent
            };
        }

        if (addIdToWikiHistory)
            RecentlyUsedRelationTargets.Add(_sessionUser.UserId,
                parentId,
                _userWritingRepo,
                _httpContextAccessor,
                _webHostEnvironment);

        var modifyRelationsForCategory =
            new ModifyRelationsForCategory(_categoryRepository, _categoryRelationRepo);
        modifyRelationsForCategory.AddChild(parentId, childId);

        return new AddChildResult
        {
            Success = true,
            Data = new TinyTopicItem
            {
                Name = EntityCache.GetCategory(parentId).Name,
                Id = childId
            }
        };
    }

    public readonly record struct RemoveParentResult(
        bool Success,
        string MessageKey,
        TinyTopicItem Data);

    public RemoveParentResult RemoveParent(
        int parentIdToRemove,
        int childId,
        int[] affectedParentIdsByMove = null)
    {
        if (!_permissionCheck.CanEditCategory(parentIdToRemove) &&
            !_permissionCheck.CanEditCategory(childId))
            return new RemoveParentResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Category.MissingRights
            };

        var modifyRelationsForCategory =
            new ModifyRelationsForCategory(_categoryRepository, _categoryRelationRepo);
        var parentHasBeenRemoved = ModifyRelationsEntityCache.RemoveParent(
            EntityCache.GetCategory(childId),
            parentIdToRemove, _sessionUser.UserId, modifyRelationsForCategory,
            _permissionCheck);

        if (!parentHasBeenRemoved)
            return new RemoveParentResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Category.NoRemainingParents
            };

        var parent = _categoryRepository.GetById(parentIdToRemove);
        _categoryRepository.Update(parent, _sessionUser.UserId,
            type: CategoryChangeType.Relations);
        var child = _categoryRepository.GetById(childId);
        if (affectedParentIdsByMove != null)
            _categoryRepository.Update(child, _sessionUser.UserId,
                type: CategoryChangeType.Moved,
                affectedParentIdsByMove: affectedParentIdsByMove);
        else
            _categoryRepository.Update(child, _sessionUser.UserId,
                type: CategoryChangeType.Relations);

        return new RemoveParentResult
        {
            Success = true,
            MessageKey = FrontendMessageKeys.Success.Category.Unlinked
        };
    }
}