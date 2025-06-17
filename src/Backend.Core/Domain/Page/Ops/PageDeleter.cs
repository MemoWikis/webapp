using System.Collections.Concurrent;

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
        const int maxRetries = 3;
        const int retryDelayMs = 100;

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                // Use a single transaction for all operations to ensure consistency
                using var transaction = _pageRepo.Session.BeginTransaction();

                try
                {
                    // Execute deletions in consistent order to prevent deadlocks:
                    // 1. Child relationships first (pages_to_questions has Foreign Key to page)
                    // 2. User activities (has Foreign Key to page) 
                    // 3. Shares (has Foreign Key to page)
                    // 4. Page itself last (parent of all Foreign Keys)

                    _pageToQuestionRepo.DeleteByPageId(pageId);
                    _userActivityRepo.DeleteForPage(pageId);
                    _sharesRepository.DeleteAllForPageWithoutTransaction(pageId);
                    _pageRepo.Delete(pageId);

                    // Commit all operations together
                    transaction.Commit();
                    return; // Success - exit retry loop
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            catch (Exception ex) when (IsDeadlockException(ex) && attempt < maxRetries)
            {
                // Log the deadlock and retry after a short delay
                Log.Warning(
                    "Deadlock detected on page deletion attempt {attempt}/{maxRetries} for pageId {pageId}: {message}",
                    attempt, maxRetries, pageId, ex.Message);

                // Wait with exponential backoff before retry
                Thread.Sleep(retryDelayMs * attempt);
            }
        }
    }

    private static bool IsDeadlockException(Exception ex)
    {
        // Check if this is a MySQL deadlock error
        return ex.Message.Contains("Deadlock found") ||
               ex.Message.Contains("Lock wait timeout") ||
               ex.Message.Contains("DataReader already open");
    }

    private void DeleteRelations(PageCacheItem pageCacheItem, int userId)
    {
        var modifyRelationsForPage = new ModifyRelationsForPage(_pageRepo, _pageRelationRepo);

        ModifyRelationsEntityCache.RemoveRelationsForPageDeleter(
            pageCacheItem,
            userId,
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
        var page = _pageRepo.GetById(pageToDeleteId);
        if (page == null)
        {
            return new DeletePageResult(
                Success: false,
                MessageKey: FrontendMessageKeys.Error.Page.NotFound);
        }

        var pageCacheItem = EntityCache.GetPage(pageToDeleteId);

        // Lock page deletions per user to prevent race conditions 
        // (e.g., deleting multiple wikis simultaneously in different tabs)
        if (pageCacheItem?.IsWiki == true)
        {
            var userDeletionLock = AcquireUserDeletionLock(pageCacheItem.Creator.Id);
            lock (userDeletionLock.LockObject)
            {
                try
                {
                    return Run(page, pageToDeleteId, newParentForQuestionsId);
                }
                finally
                {
                    // Release the lock after deletion is complete
                    ReleaseUserDeletionLock(pageCacheItem.Creator.Id);
                }
            }
        }

        // For non-wiki pages, proceed without locking
        return Run(page, pageToDeleteId, newParentForQuestionsId);
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

        var pageName = page.Name;
        var pageVisibility = page.Visibility;

        var parentIds = EntityCache.GetPage(pageToDeleteId)?
            .Parents()
            .Select(c => c.Id)
            .ToList();

        // Actually delete the page
        var deleteChangeId = RunAndGetChangeId(page, _sessionUser.UserId);

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

    public record RedirectPage(string Name, int Id)
    {
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
            var firstWiki = _sessionUser.User.FirstWiki();
            return new RedirectPage(firstWiki);
        }

        var currentWiki = EntityCache.GetPage(_sessionUser.CurrentWikiId);
        if (currentWiki == null)
        {
            var featuredRootPage = FeaturedPage.GetRootPage;
            return new RedirectPage(featuredRootPage);
        }

        var lastBreadcrumbItem = _crumbtrailService
            .BuildCrumbtrail(page, currentWiki)
            .Items
            .LastOrDefault();

        if (lastBreadcrumbItem != null)
            return new RedirectPage(lastBreadcrumbItem.Page);

        return new RedirectPage(currentWiki);
    }
}