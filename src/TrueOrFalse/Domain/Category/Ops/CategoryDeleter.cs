using Microsoft.IdentityModel.Tokens;

using ISession = NHibernate.ISession;

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

        ModifyRelationsEntityCacheAndDb.RemoveRelationsForCategoryDeleter(categoryCacheItem, userId,
            modifyRelationsForCategory);

        EntityCache.Remove(categoryCacheItem, userId);
        _categoryToQuestionRepo.DeleteByCategoryId(category.Id);
        _userActivityRepo.DeleteForCategory(category.Id);

        _categoryChangeRepo.AddDeleteEntry(category, userId);
        _extendedUserCache.RemoveAllForCategory(category.Id, _categoryValuationWritingRepo);

        _categoryRepo.Delete(category.Id);


        hasDeleted.DeletedSuccessful = true;
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

    public record DeleteTopicResult(
        bool HasChildren,
        bool IsNotCreatorOrAdmin,
        bool Success,
        RedirectParent RedirectParent);

    public DeleteTopicResult DeleteTopic(int id, int parentId)
    {
        var redirectParent = GetRedirectTopic(id);
        var topic = _categoryRepo.GetById(id);

        if (topic == null)
            throw new Exception(
                "Category couldn't be deleted. Category with specified Id cannot be found.");

        var parentIds = EntityCache.GetCategory(id)?
            .Parents()
            .Select(c => c.Id)
            .ToList(); //if the parents are fetched directly from the category there is a problem with the flush

        MoveQuestionsToParent(id, parentId);
        var hasDeleted = Run(topic, _sessionUser.UserId);

        var parentTopics = _categoryRepo.GetByIds(parentIds);

        foreach (var parent in parentTopics)
            _categoryChangeRepo.AddUpdateEntry(_categoryRepo, parent, _sessionUser.UserId, false);

        return new DeleteTopicResult(
            hasDeleted.HasChildren,
            hasDeleted.IsNotCreatorOrAdmin,
            hasDeleted.DeletedSuccessful,
            redirectParent
        );
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
    }
}