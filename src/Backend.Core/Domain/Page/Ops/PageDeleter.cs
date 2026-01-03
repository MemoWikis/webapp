using System.Collections.Concurrent;
using System.Text.Json.Serialization;

public class PageDeleter(
    SessionUser _sessionUser,
    UserActivityRepo _userActivityRepo,
    PageChangeRepo _pageChangeRepo,
    PageValuationWritingRepo _pageValuationWritingRepo,
    ExtendedUserCache _extendedUserCache,
    CrumbtrailService _crumbtrailService,
    PageRepository _pageRepo,
    PermissionCheck _permissionCheck,
    PageToQuestionRepo _pageToQuestionRepo,
    SharesRepository _sharesRepository,
    ModifyRelationsForPage _modifyRelationsForPage) : IRegisterAsInstancePerLifetime
{
    // Static dictionary to coordinate page deletions to prevent race conditions
    private static readonly ConcurrentDictionary<int, UserDeletionLock> _userPageDeletionLocks = new();

    // Helper class to track lock usage with reference counting
    private class UserDeletionLock
    {
        public object LockObject { get; } = new object();
        public int ReferenceCount { get; set; } = 0;
    }

    private DeletePageResult? ValidatePageDeletion(Page page, int pageToDeleteId)
    {
        var pageCacheItem = EntityCache.GetPage(pageToDeleteId);
        if (pageCacheItem == null)
        {
            return new DeletePageResult(
                Success: false,
                MessageKey: FrontendMessageKeys.Error.Page.NotFound);
        }

        var canDeleteResult = _permissionCheck.CanDelete(pageCacheItem);
        if (!canDeleteResult.Allowed)
        {
            return new DeletePageResult(
                Success: false,
                MessageKey: canDeleteResult.Reason);
        }

        if (!CanDeleteItemBasedOnChildParentCount(page, _sessionUser.UserId))
        {
            return new DeletePageResult(
                MessageKey: FrontendMessageKeys.Error.Page.CannotDeletePageWithChildPage,
                HasChildren: true,
                Success: false);
        }

        return null;
    }

    private DeletePageResult? HandleQuestions(int pageToDeleteId, int? newParentForQuestionsId)
    {
        var hasQuestions = EntityCache.PageHasQuestion(pageToDeleteId);

        if (!hasQuestions)
            return null;

        if (hasQuestions && newParentForQuestionsId != null)
            MoveQuestionsToParent(pageToDeleteId, (int)newParentForQuestionsId);
        else if (hasQuestions && newParentForQuestionsId is null or 0)
            return new DeletePageResult(MessageKey: FrontendMessageKeys.Error.Page.PageNotSelected, Success: false);

        return null; // No error, continue with deletion
    }

    private int RunAndGetChangeId(Page page, int userId)
    {
        var pageCacheItem = page.GetPageCacheItem();

        if (pageCacheItem != null)
        {
            GraphService.RemoveDeletedPageViewsFromAscendants(pageCacheItem.Id, pageCacheItem.TotalViews);

            DeleteRelations(pageCacheItem, userId);
            SlidingCache.RemovePage(pageCacheItem.Id);
            EntityCache.Remove(pageCacheItem, userId);
        }

        var deleteChangeId = _pageChangeRepo.AddDeleteEntry(page, userId);
        _extendedUserCache.RemoveAllForPage(page.Id, _pageValuationWritingRepo);

        DeleteImages(page);

        new MeilisearchPageIndexer().Delete(page);
        DeleteFromRepos(page.Id);

        return deleteChangeId;
    }

    private void DeleteFromRepos(int pageId)
    {
        using var transaction = _pageRepo.Session.BeginTransaction();

        try
        {
            // Decided to keep page views from deleted pages
            // to keep data in global stats/analytics/metrics
            // The foreign key constraint has been removed from the database schema
            // to allow page deletion while preserving pageview records with their Page_id

            _pageToQuestionRepo.DeleteByPageId(pageId);
            _userActivityRepo.DeleteForPage(pageId);
            _sharesRepository.DeleteAllForPageWithoutTransaction(pageId);
            _pageRepo.Delete(pageId);

            // Commit all operations together
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception($"Failed to delete page {pageId}: {ex.Message}", ex);
        }
    }

    private void DeleteRelations(PageCacheItem pageCacheItem, int userId)
    {
        ModifyRelationsEntityCache.RemoveRelationsForPageDeleter(
            pageCacheItem,
            userId,
            _modifyRelationsForPage);
    }

    private void DeleteImages(Page page)
    {
        Log.Information("PageDeleter.DeleteImages: Deleting all images for page {PageId} '{PageName}'", page.Id, page.Name);
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
        var pageCacheItem = EntityCache.GetPage(pageToDeleteId);

        var userDeletionLock = AcquireUserDeletionLock(pageCacheItem.Creator.Id);
        lock (userDeletionLock.LockObject)
        {
            try
            {
                var page = _pageRepo.GetById(pageToDeleteId);
                if (page == null)
                {
                    return new DeletePageResult(
                        Success: false,
                        MessageKey: FrontendMessageKeys.Error.Page.NotFound);
                }

                return Run(page, pageToDeleteId, newParentForQuestionsId);
            }
            finally
            {
                ReleaseUserDeletionLock(pageCacheItem.Creator.Id);
            }
        }
    }

    private DeletePageResult Run(Page page, int pageToDeleteId, int? newParentForQuestionsId)
    {
        var validationError = ValidatePageDeletion(page, pageToDeleteId);
        if (validationError != null)
            return validationError.Value;

        var questionHandlingError = HandleQuestions(pageToDeleteId, newParentForQuestionsId);
        if (questionHandlingError != null)
            return questionHandlingError.Value;

        var redirectPage = GetRedirectPage(pageToDeleteId);
        var pageInfo = CapturePageInformationBeforeDeletion(page, pageToDeleteId);
        var deleteChangeId = RunAndGetChangeId(page, _sessionUser.UserId);
        UpdateParentRelationsAfterDeletion(pageInfo, deleteChangeId);

        return new DeletePageResult(
            HasChildren: false,
            Success: true,
            RedirectParent: redirectPage,
            MessageKey: null);
    }

    private PageDeletionInfo CapturePageInformationBeforeDeletion(Page page, int pageToDeleteId)
    {
        var parentIds = EntityCache.GetPage(pageToDeleteId)?
            .Parents()
            .Select(c => c.Id)
            .ToList() ?? new List<int>();

        return new PageDeletionInfo(
            PageName: page.Name,
            PageVisibility: page.Visibility,
            ParentIds: parentIds);
    }

    private void UpdateParentRelationsAfterDeletion(PageDeletionInfo pageInfo, int deleteChangeId)
    {
        if (!pageInfo.ParentIds.Any())
            return;

        var parentPages = _pageRepo.GetByIds(pageInfo.ParentIds);

        foreach (var parent in parentPages)
        {
            _pageChangeRepo.AddDeletedChildPageEntry(
                parent,
                _sessionUser.UserId,
                deleteChangeId,
                pageInfo.PageName,
                pageInfo.PageVisibility);
        }
    }

    private record PageDeletionInfo(
        string PageName,
        PageVisibility PageVisibility,
        List<int> ParentIds);

    private void MoveQuestionsToParent(int pageToDeleteId, int parentId)
    {
        if (parentId == 0)
            throw new NullReferenceException("parent is null");

        var parent = _pageRepo.GetById(parentId);
        if (parent == null)
            throw new Exception($"Parent page with ID {parentId} not found");

        var questionIdsFromPageToDelete = EntityCache.GetQuestionIdsForPage(pageToDeleteId);

        if (questionIdsFromPageToDelete.Any())
            _pageToQuestionRepo.AddQuestionsToPage(parentId, questionIdsFromPageToDelete);

        _pageRepo.Update(parent);
        EntityCache.AddQuestionsToPage(parentId, questionIdsFromPageToDelete);
    }

    private UserDeletionLock AcquireUserDeletionLock(int userId)
    {
        // Get or create the lock for this user
        var userDeletionLock = _userPageDeletionLocks.GetOrAdd(userId, _ => new UserDeletionLock());

        // Atomically increment the reference count
        lock (userDeletionLock.LockObject)
        {
            userDeletionLock.ReferenceCount++;
        }

        return userDeletionLock;
    }

    private void ReleaseUserDeletionLock(int userId)
    {
        // Try to get the existing lock for this user
        if (_userPageDeletionLocks.TryGetValue(userId, out var userDeletionLock))
        {
            lock (userDeletionLock.LockObject)
            {
                userDeletionLock.ReferenceCount--;

                // If no more references, remove the lock from the dictionary
                if (userDeletionLock.ReferenceCount <= 0)
                {
                    _userPageDeletionLocks.TryRemove(userId, out _);
                }
            }
        }
    }

    public record RedirectPage
    {
        public string Name { get; init; }
        public int Id { get; init; }

        [JsonConstructor]
        public RedirectPage(string Name, int Id)
        {
            this.Name = Name;
            this.Id = Id;
        }

        public RedirectPage(PageCacheItem page) : this(page.Name, page.Id) { }
    }

    private RedirectPage GetRedirectPage(int pageToDeleteId)
    {
        var page = EntityCache.GetPage(pageToDeleteId);
        if (page == null)
        {
            var featuredRootPage = FeaturedPage.GetRootPage;
            return new RedirectPage(featuredRootPage);
        }

        if (page.IsWiki || pageToDeleteId == _sessionUser.CurrentWikiId)
        {
            var firstWiki = _sessionUser.User.GetWikis().First(wiki => wiki.Id != pageToDeleteId);
            return new RedirectPage(firstWiki);
        }

        var currentWiki = EntityCache.GetPage(_sessionUser.CurrentWikiId);
        if (currentWiki == null)
        {
            var featuredRootPage = FeaturedPage.GetRootPage;
            return new RedirectPage(featuredRootPage);
        }

        var wikiToRedirect = currentWiki;

        var currentWikiIsAncestor = GraphService
            .VisibleAscendants(page.Id, _permissionCheck)
            .Any(p => p.Id == currentWiki.Id);

        if (!currentWikiIsAncestor)
            wikiToRedirect = _crumbtrailService.GetWiki(page, _sessionUser);

        var lastBreadcrumbItem = _crumbtrailService
            .BuildCrumbtrail(page, wikiToRedirect)
            .Items
            .LastOrDefault();

        if (lastBreadcrumbItem != null)
            return new RedirectPage(lastBreadcrumbItem.Page);

        return new RedirectPage(currentWiki);
    }
}