public class PageDeleter(
    SessionUser _sessionUser,
    UserActivityRepo _userActivityRepo,
    PageChangeRepo pageChangeRepo,
    PageValuationWritingRepo pageValuationWritingRepo,
    ExtendedUserCache _extendedUserCache,
    CrumbtrailService _crumbtrailService,
    PageRepository pageRepo,
    PageRelationRepo pageRelationRepo,
    PermissionCheck _permissionCheck,
    PageToQuestionRepo pageToQuestionRepo)
    : IRegisterAsInstancePerLifetime
{
    private HasDeleted Run(Page page, int userId)
    {
        var pageCacheItem = EntityCache.GetPage(page.Id);
        var hasDeleted = new HasDeleted();

        if (!_permissionCheck.CanDelete(page))
        {
            hasDeleted.IsNotCreatorOrAdmin = true;
            return hasDeleted;
        }

        if (!CanDeleteItemBasedOnChildParentCount(page, userId))
        {
            hasDeleted.HasChildren = true;
            return hasDeleted;
        }

        var modifyRelationsForPage =
            new ModifyRelationsForPage(pageRepo, pageRelationRepo);

        ModifyRelationsEntityCache.RemoveRelationsForPageDeleter(pageCacheItem, userId,
            modifyRelationsForPage);

        EntityCache.Remove(pageCacheItem, userId);
        pageToQuestionRepo.DeleteByPageId(page.Id);
        _userActivityRepo.DeleteForPage(page.Id);

        var deleteChangeId = pageChangeRepo.AddDeleteEntry(page, userId);
        _extendedUserCache.RemoveAllForPage(page.Id, pageValuationWritingRepo);

        var deleteImage = new DeleteImage();
        deleteImage.RemoveAllForPage(page.Id);

        pageRepo.Delete(page.Id);

        hasDeleted.DeletedSuccessful = true;
        hasDeleted.ChangeId = deleteChangeId;
        return hasDeleted;
    }

    private bool CanDeleteItemBasedOnChildParentCount(
        Page page,
        int userId)
    {
        var allChildren = GraphService.Children(page.Id);
        var visitedChildrenIds = new HashSet<int>();

        if (allChildren.Count > 0)
        {
            var allVisibleChildren =
                GraphService.VisibleChildren(page.Id, _permissionCheck, userId);

            if (allVisibleChildren.Count > 0)
            {
                foreach (var child in allVisibleChildren)
                {
                    visitedChildrenIds.Add(child.Id);

                    var visibleParents = GraphService
                        .VisibleParents(child.Id, _permissionCheck)
                        .Where(p => p.Id != page.Id)
                        .ToList();

                    if (visibleParents.Count < 1)
                        return false;
                }
            }

            var privateChildren =
                allChildren
                    .Where(c => !visitedChildrenIds.Contains(c.Id))
                    .ToList();

            if (privateChildren.Count > 0)
            {
                foreach (var child in privateChildren)
                {
                    var hasExtraParent = GraphService.ParentIds(child).Any(i => i != page.Id);

                    if (!hasExtraParent)
                        return false;
                }
            }
        }

        return true;
    }

    public record struct DeletePageResult(
        bool HasChildren = false,
        bool IsNotCreatorOrAdmin = false,
        bool Success = false,
        RedirectParent? RedirectParent = null,
        string? MessageKey = null);

    public DeletePageResult DeletePage(int pageToDeleteId, int? newParentForQuestionsId)
    {
        var redirectParent = GetRedirectPage(pageToDeleteId);
        var page = pageRepo.GetById(pageToDeleteId);

        if (page == null)
            throw new Exception(
                "Page couldn't be deleted. Page with specified Id cannot be found.");

        var pageName = page.Name;
        var pageVisibility = page.Visibility;

        var parentIds = EntityCache.GetPage(pageToDeleteId)?
            .Parents()
            .Select(c => c.Id)
            .ToList(); //if the parents are fetched directly from the page there is a problem with the flush

        var hasQuestions = EntityCache.PageHasQuestion(pageToDeleteId);
        if (hasQuestions && newParentForQuestionsId != null)
            MoveQuestionsToParent(pageToDeleteId, (int)newParentForQuestionsId);
        else if (hasQuestions && newParentForQuestionsId == null || newParentForQuestionsId == 0)
            return new DeletePageResult(MessageKey: FrontendMessageKeys.Error.Page.PageNotSelected, Success: false);

        var hasDeleted = Run(page, _sessionUser.UserId);

        if (parentIds != null && parentIds.Any())
        {
            var parentPages = pageRepo.GetByIds(parentIds);

            foreach (var parent in parentPages)
                pageChangeRepo.AddDeletedChildPageEntry(parent, _sessionUser.UserId, hasDeleted.ChangeId, pageName, pageVisibility);
        }

        return new DeletePageResult(
            HasChildren: hasDeleted.HasChildren,
            IsNotCreatorOrAdmin: hasDeleted.IsNotCreatorOrAdmin,
            Success: hasDeleted.DeletedSuccessful,
            RedirectParent: redirectParent);
    }

    private void MoveQuestionsToParent(int pageToDeleteId, int parentId)
    {
        if (parentId == 0)
        {
            throw new NullReferenceException("parent is null");
        }

        var parent = pageRepo.GetById(parentId);
        var questionIdsFromPageToDelete = EntityCache.GetQuestionIdsForPage(pageToDeleteId);

        if (questionIdsFromPageToDelete.Any())
            pageToQuestionRepo.AddQuestionsToPage(parentId, questionIdsFromPageToDelete);
        pageRepo.Update(parent);

        EntityCache.AddQuestionsToPage(parentId, questionIdsFromPageToDelete);
    }

    public record RedirectParent(string Name, int Id);

    private RedirectParent GetRedirectPage(int id)
    {
        var page = EntityCache.GetPage(id);
        var currentWiki = EntityCache.GetPage(_sessionUser.CurrentWikiId);

        var lastBreadcrumbItem = _crumbtrailService.BuildCrumbtrail(page, currentWiki).Items
            .LastOrDefault();

        if (lastBreadcrumbItem != null)
            return new RedirectParent(lastBreadcrumbItem.Page.Name,
                lastBreadcrumbItem.Page.Id);

        return new RedirectParent(currentWiki.Name, currentWiki.Id);
    }

    private class HasDeleted
    {
        public bool DeletedSuccessful { get; set; }
        public bool HasChildren { get; set; }
        public bool IsNotCreatorOrAdmin { get; set; }
        public int ChangeId { get; set; }
    }
}