public class PageDeleter(
    SessionUser _sessionUser,
    UserActivityRepo _userActivityRepo,
    PageChangeRepo _pageChangeRepo,
    PageValuationWritingRepo _pageValuationWritingRepo,
    ExtendedUserCache _extendedUserCache,
    CrumbtrailService _crumbtrailService,
    PageRepository _pageRepo,
    PageRelationRepo _pageRelationRepo,
    PermissionCheck _permissionCheck,
    PageToQuestionRepo _pageToQuestionRepo,
    SharesRepository _sharesRepository)
    : IRegisterAsInstancePerLifetime
{
    private HasDeleted Run(Page page, int userId)
    {
        var pageCacheItem = EntityCache.GetPage(page.Id);
        var hasDeleted = new HasDeleted();

        if (!_permissionCheck.CanDelete(pageCacheItem))
        {
            hasDeleted.IsNotCreatorOrAdmin = true;
            return hasDeleted;
        }

        if (!CanDeleteItemBasedOnChildParentCount(page, userId))
        {
            hasDeleted.HasChildren = true;
            return hasDeleted;
        }

        DeleteRelations(pageCacheItem, userId);

        EntityCache.Remove(pageCacheItem, userId);

        var deleteChangeId = _pageChangeRepo.AddDeleteEntry(page, userId);
        _extendedUserCache.RemoveAllForPage(page.Id, _pageValuationWritingRepo);

        DeleteImages(page);

        Task.Run(async () => await new MeilisearchPageIndexer()
            .DeleteAsync(page));

        DeleteFromRepos(page.Id);

        hasDeleted.DeletedSuccessful = true;
        hasDeleted.ChangeId = deleteChangeId;
        return hasDeleted;
    }

    private void DeleteFromRepos(int pageId)
    {
        _pageToQuestionRepo.DeleteByPageId(pageId);
        _userActivityRepo.DeleteForPage(pageId);
        _sharesRepository.DeleteAllForPage(pageId);
        _pageRepo.Delete(pageId);
    }

    private void DeleteRelations(PageCacheItem pageCacheItem, int userId)
    {
        var modifyRelationsForPage =
            new ModifyRelationsForPage(_pageRepo, _pageRelationRepo);

        ModifyRelationsEntityCache.RemoveRelationsForPageDeleter(pageCacheItem, userId,
            modifyRelationsForPage);
    }

    private void DeleteImages(Page page)
    {
        var deleteImage = new DeleteImage();
        deleteImage.RemoveAllForPage(page.Id);
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
        RedirectPage? RedirectParent = null,
        string? MessageKey = null);

    public DeletePageResult DeletePage(int pageToDeleteId, int? newParentForQuestionsId)
    {
        var redirectPage = GetRedirectPage(pageToDeleteId);
        var page = _pageRepo.GetById(pageToDeleteId);

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
            var parentPages = _pageRepo.GetByIds(parentIds);

            foreach (var parent in parentPages)
                _pageChangeRepo.AddDeletedChildPageEntry(parent, _sessionUser.UserId, hasDeleted.ChangeId, pageName, pageVisibility);
        }

        return new DeletePageResult(
            HasChildren: hasDeleted.HasChildren,
            IsNotCreatorOrAdmin: hasDeleted.IsNotCreatorOrAdmin,
            Success: hasDeleted.DeletedSuccessful,
            RedirectParent: redirectPage);
    }

    private void MoveQuestionsToParent(int pageToDeleteId, int parentId)
    {
        if (parentId == 0)
        {
            throw new NullReferenceException("parent is null");
        }

        var parent = _pageRepo.GetById(parentId);
        var questionIdsFromPageToDelete = EntityCache.GetQuestionIdsForPage(pageToDeleteId);

        if (questionIdsFromPageToDelete.Any())
            _pageToQuestionRepo.AddQuestionsToPage(parentId, questionIdsFromPageToDelete);
        _pageRepo.Update(parent);

        EntityCache.AddQuestionsToPage(parentId, questionIdsFromPageToDelete);
    }

    public record RedirectPage(string Name, int Id);

    private RedirectPage GetRedirectPage(int id)
    {
        var page = EntityCache.GetPage(id);
        var currentWiki = EntityCache.GetPage(_sessionUser.CurrentWikiId);

        var lastBreadcrumbItem = _crumbtrailService
            .BuildCrumbtrail(page, currentWiki)
            .Items
            .LastOrDefault();

        if (lastBreadcrumbItem != null)
            return new RedirectPage(lastBreadcrumbItem.Page.Name, lastBreadcrumbItem.Page.Id);

        if (id == currentWiki.Id)
            return FindAlternativePageWhenDeletingCurrentWiki(id);

        return new RedirectPage(currentWiki.Name, currentWiki.Id);
    }

    private RedirectPage FindAlternativePageWhenDeletingCurrentWiki(int id)
    {
        var startPage = EntityCache.GetPage(_sessionUser.User.StartPageId);
        if (startPage != null && id != startPage.Id && startPage.IsWiki)
            return new RedirectPage(startPage.Name, startPage.Id);

        var wikis = _sessionUser.User.GetWikis();
        if (wikis.Any())
            return new RedirectPage(wikis.First().Name, wikis.First().Id);

        var favorites = _sessionUser.User.GetFavorites();
        if (favorites.Any())
        {
            var firstPossibleFavorite = favorites.FirstOrDefault(page => page.Id != id);
            if (firstPossibleFavorite != null)
                return new RedirectPage(firstPossibleFavorite.Name, firstPossibleFavorite.Id);
        }

        var userCacheItem = _extendedUserCache.GetUser(_sessionUser.UserId);
        var recentPages = userCacheItem.RecentPages?.GetRecentPages();

        if (recentPages != null && recentPages.Any())
        {
            var firstPossiblePage = recentPages.FirstOrDefault(page => page.Id != id);
            if (firstPossiblePage != null)
                return new RedirectPage(firstPossiblePage.Name, firstPossiblePage.Id);
        }

        var featuredRootPage = FeaturedPage.GetRootPage;
        return new RedirectPage(featuredRootPage.Name, featuredRootPage.Id);
    }

    private class HasDeleted
    {
        public bool DeletedSuccessful { get; set; }
        public bool HasChildren { get; set; }
        public bool IsNotCreatorOrAdmin { get; set; }
        public int ChangeId { get; set; }
    }
}