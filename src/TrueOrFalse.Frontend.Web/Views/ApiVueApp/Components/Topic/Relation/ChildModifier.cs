using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace VueApp;

public class ChildModifier(
    PermissionCheck _permissionCheck,
    SessionUser _sessionUser,
    PageRepository pageRepository,
    UserWritingRepo _userWritingRepo,
    IHttpContextAccessor _httpContextAccessor,
    IWebHostEnvironment _webHostEnvironment,
    PageRelationRepo pageRelationRepo)
{
    public readonly record struct AddChildResult(
        bool Success,
        string MessageKey,
        TinyPageItem Data);

    public readonly record struct TinyPageItem(int Id, string Name);

    public AddChildResult AddChild(
        int childId,
        int parentId,
        bool addIdToWikiHistory = false)
    {
        if (childId == parentId)
            return new AddChildResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Page.LoopLink
            };
        if (parentId == RootPage.RootCategoryId && !_sessionUser.IsInstallationAdmin)
            return new AddChildResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Page.ParentIsRoot
            };

        var parent = EntityCache.GetPage(parentId);

        if (parent.ChildRelations.Any(r => r.ChildId == childId))
            return new AddChildResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Page.IsAlreadyLinkedAsChild
            };
        var selectedPageIsParent = GraphService.Ascendants(parentId)
            .Any(c => c.Id == childId);

        if (selectedPageIsParent)
        {
            Logg.r.Error("Child is Parent ");
            return new AddChildResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Page.ChildIsParent
            };
        }

        if (addIdToWikiHistory)
            RecentlyUsedRelationTargets.Add(_sessionUser.UserId,
                parentId,
                _userWritingRepo,
                _httpContextAccessor,
                _webHostEnvironment);

        var modifyRelationsForCategory = new ModifyRelationsForCategory(pageRepository, pageRelationRepo);
        modifyRelationsForCategory.AddChild(parentId, childId, _sessionUser.UserId);

        return new AddChildResult
        {
            Success = true,
            Data = new TinyPageItem
            {
                Name = EntityCache.GetPage(parentId).Name,
                Id = childId
            }
        };
    }

    public readonly record struct RemoveParentResult(
        bool Success,
        string MessageKey,
        TinyPageItem Data);

    public RemoveParentResult RemoveParent(
        int parentIdToRemove,
        int childId)
    {
        if (!_permissionCheck.CanEditCategory(parentIdToRemove) &&
            !_permissionCheck.CanEditCategory(childId))
            return new RemoveParentResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Page.MissingRights
            };

        var modifyRelationsForCategory = new ModifyRelationsForCategory(pageRepository, pageRelationRepo);

        var parentHasBeenRemoved = ModifyRelationsEntityCache.RemoveParent(
            EntityCache.GetPage(childId),
            parentIdToRemove, _sessionUser.UserId, modifyRelationsForCategory,
            _permissionCheck);

        if (!parentHasBeenRemoved)
            return new RemoveParentResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Page.NoRemainingParents
            };

        var parent = pageRepository.GetById(parentIdToRemove);
        pageRepository.Update(parent, _sessionUser.UserId, type: PageChangeType.Relations);
        var child = pageRepository.GetById(childId);
        pageRepository.Update(child, _sessionUser.UserId, type: PageChangeType.Relations);

        return new RemoveParentResult
        {
            Success = true,
            MessageKey = FrontendMessageKeys.Success.Page.Unlinked
        };
    }
}