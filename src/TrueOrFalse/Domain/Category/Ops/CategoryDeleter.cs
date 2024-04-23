using ISession = NHibernate.ISession;

public class CategoryDeleter(
    ISession _session,
    SessionUser _sessionUser,
    UserActivityRepo _userActivityRepo,
    CategoryRepository _categoryRepository,
    CategoryChangeRepo _categoryChangeRepo,
    CategoryValuationWritingRepo _categoryValuationWritingRepo,
    SessionUserCache _sessionUserCache,
    CrumbtrailService _crumbtrailService,
    CategoryRepository _categoryRepo,
    CategoryRelationRepo _categoryRelationRepo,
    PermissionCheck _permissionCheck)
    : IRegisterAsInstancePerLifetime
{

    ///todo:(DaMa)  Revise: Wrong place for SQL commands.
    private HasDeleted Run(Category category, int userId, bool isTestCase = false)
    {
        var categoryCacheItem = EntityCache.GetCategory(category.Id);
        var hasDeleted = new HasDeleted();

        if (!_permissionCheck.CanDelete(category))
        {
            hasDeleted.IsNotCreatorOrAdmin = true;
            return hasDeleted;
        }

        if (GraphService.Descendants(category.Id).Count > 0)
        {
            hasDeleted.HasChildren = true;
            return hasDeleted;
        }

        if (!isTestCase)
        {
            _session.CreateSQLQuery(
                "DELETE FROM relatedcategoriestorelatedcategories where Related_id = " + category.Id).ExecuteUpdate();
            _session.CreateSQLQuery("DELETE FROM relatedcategoriestorelatedcategories where Category_id = " +
                                    category.Id).ExecuteUpdate();
            _session.CreateSQLQuery("DELETE FROM categories_to_questions where Category_id = " + category.Id)
                .ExecuteUpdate();
        }

        _userActivityRepo.DeleteForCategory(category.Id);

        _categoryChangeRepo.AddDeleteEntry(category, userId);
        _categoryValuationWritingRepo.DeleteCategoryValuation(category.Id);
        _categoryRepository.Delete(category);

        var modifyRelationsForCategory = new ModifyRelationsForCategory(_categoryRepository, _categoryRelationRepo);
        ModifyRelationsEntityCache.RemoveRelationsForCategoryDeleter(categoryCacheItem, userId, modifyRelationsForCategory);
        EntityCache.Remove(categoryCacheItem, userId);
        _sessionUserCache.RemoveAllForCategory(category.Id, _categoryValuationWritingRepo);

        hasDeleted.DeletedSuccessful = true;
        return hasDeleted;
    }

    public record DeleteTopicResult(bool HasChildren, bool IsNotCreatorOrAdmin, bool Success, RedirectParent RedirectParent);

    public DeleteTopicResult DeleteTopic(int id)
    {
        var redirectParent = GetRedirectTopic(id);
        var topic = _categoryRepo.GetById(id);
        if (topic == null)
            throw new Exception("Category couldn't be deleted. Category with specified Id cannot be found.");

        var parentIds = EntityCache.GetCategory(id)
            .Parents()
            .Select(c => c.Id)
            .ToList(); //if the parents are fetched directly from the category there is a problem with the flush
        var parentTopics = _categoryRepo.GetByIds(parentIds);

        var hasDeleted = Run(topic, _sessionUser.UserId);
        foreach (var parent in parentTopics)
        {
            _categoryChangeRepo.AddUpdateEntry(_categoryRepo, parent, _sessionUser.UserId, false);
        }

        return new DeleteTopicResult(
            hasDeleted.HasChildren,
            hasDeleted.IsNotCreatorOrAdmin,
            hasDeleted.DeletedSuccessful,
            redirectParent
        );
    }

    public  record RedirectParent(string Name, int Id);

    private RedirectParent GetRedirectTopic(int id)
    {
        var topic = EntityCache.GetCategory(id);
        var currentWiki = EntityCache.GetCategory(_sessionUser.CurrentWikiId);
        var lastBreadcrumbItem = _crumbtrailService.BuildCrumbtrail(topic, currentWiki).Items.LastOrDefault();

        if (lastBreadcrumbItem != null)
            return new RedirectParent(lastBreadcrumbItem.Category.Name, lastBreadcrumbItem.Category.Id);

        return new RedirectParent(currentWiki.Name, currentWiki.Id); 
    }

    private class HasDeleted
    {
        public bool DeletedSuccessful { get; set; }
        public bool HasChildren { get; set; }
        public bool IsNotCreatorOrAdmin { get; set; }
    }
}