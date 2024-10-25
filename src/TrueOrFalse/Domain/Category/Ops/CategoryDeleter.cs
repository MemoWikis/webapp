public class CategoryDeleter(
    SessionUser _sessionUser,
    UserActivityRepo _userActivityRepo,
    CategoryChangeRepo _categoryChangeRepo,
    CategoryValuationWritingRepo _categoryValuationWritingRepo,
    ExtendedUserCache _extendedUserCache,
    CrumbtrailService _crumbtrailService,
    CategoryRepository _categoryRepo,
    CategoryRelationRepo _categoryRelationRepo,
    PermissionCheck _permissionCheck,
    CategoryToQuestionRepo _categoryToQuestionRepo)
    : IRegisterAsInstancePerLifetime
{
    private HasDeleted Run(Category category, int userId)
    {
        var categoryCacheItem = EntityCache.GetCategory(category.Id);
        var hasDeleted = new HasDeleted();

        if (!_permissionCheck.CanDelete(category))
        {
            hasDeleted.IsNotCreatorOrAdmin = true;
            return hasDeleted;
        }

        if (!CanDeleteItemBasedOnChildParentCount(category, userId))
        {
            hasDeleted.HasChildren = true;
            return hasDeleted;
        }

        var modifyRelationsForCategory =
            new ModifyRelationsForCategory(_categoryRepo, _categoryRelationRepo);

        ModifyRelationsEntityCache.RemoveRelationsForCategoryDeleter(categoryCacheItem, userId,
            modifyRelationsForCategory);

        EntityCache.Remove(categoryCacheItem, userId);
        _categoryToQuestionRepo.DeleteByCategoryId(category.Id);
        _userActivityRepo.DeleteForCategory(category.Id);

        var deleteChangeId = _categoryChangeRepo.AddDeleteEntry(category, userId);
        _extendedUserCache.RemoveAllForCategory(category.Id, _categoryValuationWritingRepo);

        var deleteImage = new DeleteImage();
        deleteImage.RemoveAllForTopic(category.Id);

        _categoryRepo.Delete(category.Id);

        hasDeleted.DeletedSuccessful = true;
        hasDeleted.ChangeId = deleteChangeId;
        return hasDeleted;
    }

    private bool CanDeleteItemBasedOnChildParentCount(
        Category category,
        int userId)
    {
        var allChildren = GraphService.Children(category.Id);
        var visitedChildrenIds = new HashSet<int>();

        if (allChildren.Count > 0)
        {
            var allVisibleChildren =
                GraphService.VisibleChildren(category.Id, _permissionCheck, userId);

            if (allVisibleChildren.Count > 0)
            {
                foreach (var child in allVisibleChildren)
                {
                    visitedChildrenIds.Add(child.Id);

                    var visibleParents = GraphService
                        .VisibleParents(child.Id, _permissionCheck)
                        .Where(p => p.Id != category.Id)
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
                    var hasExtraParent = GraphService.ParentIds(child).Any(i => i != category.Id);

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
        var topic = _categoryRepo.GetById(topicToDeleteId);

        if (topic == null)
            throw new Exception(
                "Category couldn't be deleted. Category with specified Id cannot be found.");

        var topicName = topic.Name;
        var topicVisibility = topic.Visibility;

        var parentIds = EntityCache.GetCategory(topicToDeleteId)?
            .Parents()
            .OrderBy(c => c.DateCreated)
            .Select(c => c.Id)
            .ToList(); //if the parents are fetched directly from the category there is a problem with the flush

        var hasQuestions = EntityCache.TopicHasQuestion(topicToDeleteId);
        if (hasQuestions && newParentForQuestionsId != null)
            MoveQuestionsToParent(topicToDeleteId, (int)newParentForQuestionsId);
        else if (hasQuestions && newParentForQuestionsId == null || newParentForQuestionsId == 0)
            return new DeleteTopicResult(MessageKey: FrontendMessageKeys.Error.Category.TopicNotSelected, Success: false);

        var hasDeleted = Run(topic, _sessionUser.UserId);

        if (parentIds != null && parentIds.Any())
        {
            var parentsToUpdateWithDeleteEntry = new HashSet<int>(parentIds);

            var parents = EntityCache.GetCategories(parentIds);

            var allAscendantIds = new HashSet<int>();
            foreach (var parent in parents)
            {
                var ascendantIds = GraphService.Ascendants(parent.Id).Select(p => p.Id);
                foreach (var id in ascendantIds)
                {
                    allAscendantIds.Add(id);
                }
            }

            parentsToUpdateWithDeleteEntry.ExceptWith(allAscendantIds);

            var parentTopics = _categoryRepo.GetByIds(parentsToUpdateWithDeleteEntry.ToList());

            foreach (var parent in parentTopics)
            {
                _categoryChangeRepo.AddDeletedChildTopicEntry(parent, _sessionUser.UserId, hasDeleted.ChangeId, topicName, topicVisibility);
            }
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

        var parent = _categoryRepo.GetById(parentId);
        var questionIdsFromTopicToDelete = EntityCache.GetQuestionsIdsForCategory(topicToDeleteId);

        if (questionIdsFromTopicToDelete.Any())
            _categoryToQuestionRepo.AddQuestionsToCategory(parentId, questionIdsFromTopicToDelete);
        _categoryRepo.Update(parent);

        EntityCache.AddQuestionsToCategory(parentId, questionIdsFromTopicToDelete);
    }

    public record RedirectParent(string Name, int Id);

    private RedirectParent GetRedirectTopic(int id)
    {
        var topic = EntityCache.GetCategory(id);
        var currentWiki = EntityCache.GetCategory(_sessionUser.CurrentWikiId);

        var lastBreadcrumbItem = _crumbtrailService.BuildCrumbtrail(topic, currentWiki).Items
            .LastOrDefault();

        if (lastBreadcrumbItem != null)
            return new RedirectParent(lastBreadcrumbItem.Category.Name,
                lastBreadcrumbItem.Category.Id);

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