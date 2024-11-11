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
        var categoryCacheItem = EntityCache.GetPage(page.Id);
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

        var modifyRelationsForCategory =
            new ModifyRelationsForCategory(pageRepo, pageRelationRepo);

        ModifyRelationsEntityCache.RemoveRelationsForCategoryDeleter(categoryCacheItem, userId,
            modifyRelationsForCategory);

        EntityCache.Remove(categoryCacheItem, userId);
        pageToQuestionRepo.DeleteByPageId(page.Id);
        _userActivityRepo.DeleteForCategory(page.Id);

        var deleteChangeId = pageChangeRepo.AddDeleteEntry(page, userId);
        _extendedUserCache.RemoveAllForCategory(page.Id, pageValuationWritingRepo);

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

    public DeletePageResult DeletePage(int topicToDeleteId, int? newParentForQuestionsId)
    {
        var redirectParent = GetRedirectPage(topicToDeleteId);
        var topic = pageRepo.GetById(topicToDeleteId);

        if (topic == null)
            throw new Exception(
                "Category couldn't be deleted. Category with specified Id cannot be found.");

        var topicName = topic.Name;
        var topicVisibility = topic.Visibility;

        var parentIds = EntityCache.GetPage(topicToDeleteId)?
            .Parents()
            .Select(c => c.Id)
            .ToList(); //if the parents are fetched directly from the category there is a problem with the flush

        var hasQuestions = EntityCache.PageHasQuestion(topicToDeleteId);
        if (hasQuestions && newParentForQuestionsId != null)
            MoveQuestionsToParent(topicToDeleteId, (int)newParentForQuestionsId);
        else if (hasQuestions && newParentForQuestionsId == null || newParentForQuestionsId == 0)
            return new DeletePageResult(MessageKey: FrontendMessageKeys.Error.Page.PageNotSelected, Success: false);

        var hasDeleted = Run(topic, _sessionUser.UserId);

        if (parentIds != null && parentIds.Any())
        {
            var parentPages = pageRepo.GetByIds(parentIds);

            foreach (var parent in parentPages)
                pageChangeRepo.AddDeletedChildPageEntry(parent, _sessionUser.UserId, hasDeleted.ChangeId, topicName, topicVisibility);
        }

        return new DeletePageResult(
            HasChildren: hasDeleted.HasChildren,
            IsNotCreatorOrAdmin: hasDeleted.IsNotCreatorOrAdmin,
            Success: hasDeleted.DeletedSuccessful,
            RedirectParent: redirectParent);
    }

    private void MoveQuestionsToParent(int topicToDeleteId, int parentId)
    {
        if (parentId == 0)
        {
            throw new NullReferenceException("parent is null");
        }

        var parent = pageRepo.GetById(parentId);
        var questionIdsFromPageToDelete = EntityCache.GetQuestionsIdsForCategory(topicToDeleteId);

        if (questionIdsFromPageToDelete.Any())
            pageToQuestionRepo.AddQuestionsToCategory(parentId, questionIdsFromPageToDelete);
        pageRepo.Update(parent);

        EntityCache.AddQuestionsToCategory(parentId, questionIdsFromPageToDelete);
    }

    public record RedirectParent(string Name, int Id);

    private RedirectParent GetRedirectPage(int id)
    {
        var topic = EntityCache.GetPage(id);
        var currentWiki = EntityCache.GetPage(_sessionUser.CurrentWikiId);

        var lastBreadcrumbItem = _crumbtrailService.BuildCrumbtrail(topic, currentWiki).Items
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