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
    SharesRepository _sharesRepository) : IRegisterAsInstancePerLifetime
{
    private DeletePageResult? ValidatePageDeletion(Page page, int pageToDeleteId)
    {
        if (page == null)
            throw new Exception(
                "Page couldn't be deleted. Page with specified Id cannot be found.");

        // Check permissions first
        var pageCacheItem = EntityCache.GetPage(pageToDeleteId);
        if (pageCacheItem == null)
            throw new Exception("Page cache item not found.");

        var canDeleteResult = _permissionCheck.CanDelete(pageCacheItem);
        if (!canDeleteResult.Allowed)
            return new DeletePageResult(
                Success: false,
                MessageKey: canDeleteResult.Reason);

        if (!CanDeleteItemBasedOnChildParentCount(page, _sessionUser.UserId))
            return new DeletePageResult(
                HasChildren: true,
                Success: false);

        return null;
    }

    private DeletePageResult? HandleQuestions(int pageToDeleteId, int? newParentForQuestionsId)
    {
        var hasQuestions = EntityCache.PageHasQuestion(pageToDeleteId);

        if (hasQuestions && newParentForQuestionsId != null)
            MoveQuestionsToParent(pageToDeleteId, (int)newParentForQuestionsId);
        else if (hasQuestions && newParentForQuestionsId == null || newParentForQuestionsId == 0)
            return new DeletePageResult(MessageKey: FrontendMessageKeys.Error.Page.PageNotSelected, Success: false);

        return null; // No error, continue with deletion
    }

    private int Run(Page page, int userId)
    {
        var pageCacheItem = EntityCache.GetPage(page.Id);

        if (pageCacheItem != null)
        {
            DeleteRelations(pageCacheItem, userId);
            EntityCache.Remove(pageCacheItem, userId);
        }

        var deleteChangeId = _pageChangeRepo.AddDeleteEntry(page, userId);
        _extendedUserCache.RemoveAllForPage(page.Id, _pageValuationWritingRepo);

        DeleteImages(page);

        Task.Run(async () => await new MeilisearchPageIndexer()
            .DeleteAsync(page));

        DeleteFromRepos(page.Id);

        return deleteChangeId;
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
        bool Success = false,
        RedirectPage? RedirectParent = null,
        string? MessageKey = null);

    public DeletePageResult DeletePage(int pageToDeleteId, int? newParentForQuestionsId)
    {
        var page = _pageRepo.GetById(pageToDeleteId)!;

        var validationError = ValidatePageDeletion(page, pageToDeleteId);
        if (validationError != null)
            return validationError.Value;

        var questionHandlingError = HandleQuestions(pageToDeleteId, newParentForQuestionsId);
        if (questionHandlingError != null)
            return questionHandlingError.Value;

        var redirectPage = GetRedirectPage(pageToDeleteId);

        var pageName = page.Name;
        var pageVisibility = page.Visibility;

        var parentIds = EntityCache.GetPage(pageToDeleteId)?
            .Parents()
            .Select(c => c.Id)
            .ToList();

        // Actually delete the page
        var deleteChangeId = Run(page, _sessionUser.UserId);

        // Update parent relations
        if (parentIds != null && parentIds.Any())
        {
            var parentPages = _pageRepo.GetByIds(parentIds);

            foreach (var parent in parentPages)
                _pageChangeRepo.AddDeletedChildPageEntry(parent, _sessionUser.UserId, deleteChangeId, pageName,
                    pageVisibility);
        }

        return new DeletePageResult(
            HasChildren: false,
            Success: true,
            RedirectParent: redirectPage,
            MessageKey: null);
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

        if (page.IsWiki)
        {
            var firstWiki = _sessionUser.User.GetWikis().First(wiki => wiki.Id != id);
            return new RedirectPage(firstWiki.Name, firstWiki.Id);
        }

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
        var startPage = _sessionUser.User.FirstWiki();
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
}
