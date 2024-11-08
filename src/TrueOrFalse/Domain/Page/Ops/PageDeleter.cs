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
        deleteImage.RemoveAllForTopic(page.Id);

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

    public record struct DeleteTopicResult(
        bool HasChildren = false,
        bool IsNotCreatorOrAdmin = false,
        bool Success = false,
        RedirectParent? RedirectParent = null,
        string? MessageKey = null);

    public DeleteTopicResult DeleteTopic(int topicToDeleteId, int? newParentForQuestionsId)
    {
        var redirectParent = GetRedirectTopic(topicToDeleteId);
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

        var hasQuestions = EntityCache.TopicHasQuestion(topicToDeleteId);
        if (hasQuestions && newParentForQuestionsId != null)
            MoveQuestionsToParent(topicToDeleteId, (int)newParentForQuestionsId);
        else if (hasQuestions && newParentForQuestionsId == null || newParentForQuestionsId == 0)
            return new DeleteTopicResult(MessageKey: FrontendMessageKeys.Error.Page.TopicNotSelected, Success: false);

        var hasDeleted = Run(topic, _sessionUser.UserId);

        if (parentIds != null && parentIds.Any())
        {
            var parentTopics = pageRepo.GetByIds(parentIds);

            foreach (var parent in parentTopics)
                pageChangeRepo.AddDeletedChildTopicEntry(parent, _sessionUser.UserId, hasDeleted.ChangeId, topicName, topicVisibility);
        }

        return new DeleteTopicResult(
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
        var questionIdsFromTopicToDelete = EntityCache.GetQuestionsIdsForCategory(topicToDeleteId);

        if (questionIdsFromTopicToDelete.Any())
            pageToQuestionRepo.AddQuestionsToCategory(parentId, questionIdsFromTopicToDelete);
        pageRepo.Update(parent);

        EntityCache.AddQuestionsToCategory(parentId, questionIdsFromTopicToDelete);
    }

    public record RedirectParent(string Name, int Id);

    private RedirectParent GetRedirectTopic(int id)
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