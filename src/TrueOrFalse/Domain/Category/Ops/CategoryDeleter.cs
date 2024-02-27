using ISession = NHibernate.ISession;

public class CategoryDeleter(
    ISession _session,
    SessionUser _sessionUser,
    UserActivityRepo _userActivityRepo,
    CategoryRepository _categoryRepository,
    CategoryChangeRepo _categoryChangeRepo,
    CategoryValuationWritingRepo _categoryValuationWritingRepo,
    PermissionCheck _permissionCheck,
    SessionUserCache _sessionUserCache,
    CrumbtrailService crumbtrailService,
    CategoryRepository categoryRepo)
    : IRegisterAsInstancePerLifetime
{

    ///todo:(DaMa)  Revise: Wrong place for SQL commands.
    public HasDeleted Run(Category category, int userId, bool isTestCase = false)
    {
        var categoryCacheItem = EntityCache.GetCategory(category.Id);
        var hasDeleted = new HasDeleted();

        if (!_sessionUser.IsInstallationAdmin && _sessionUser.UserId != categoryCacheItem.Creator.Id)
        {
            hasDeleted.IsNotCreatorOrAdmin = true;
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

        ModifyRelationsEntityCache.RemoveRelations(categoryCacheItem);
        EntityCache.Remove(categoryCacheItem, _permissionCheck, userId);
        _sessionUserCache.RemoveAllForCategory(category.Id, _categoryValuationWritingRepo);

        hasDeleted.DeletedSuccessful = true;
        return hasDeleted;
    }

    public RequestResult DeleteTopic(int id)
    {
        var redirectParent = GetRedirectTopic(id);
        var topic = categoryRepo.GetById(id);
        if (topic == null)
            throw new Exception("Category couldn't be deleted. Category with specified Id cannot be found.");

        var parentIds =
            EntityCache.GetCategory(id).ParentCategories().Select(c => c.Id)
                .ToList(); //if the parents are fetched directly from the category there is a problem with the flush
        var parentTopics = categoryRepo.GetByIds(parentIds);

        var hasDeleted = Run(topic, _sessionUser.UserId);
        foreach (var parent in parentTopics)
        {
            _categoryChangeRepo.AddUpdateEntry(categoryRepo, parent, _sessionUser.UserId, false);
        }

        return new RequestResult
        {
            success = true,
            data = new
            {
                hasChildren = hasDeleted.HasChildren,
                isNotCreatorOrAdmin = hasDeleted.IsNotCreatorOrAdmin,
                success = hasDeleted.DeletedSuccessful,
                redirectParent = redirectParent
            }
        };
    }


    private dynamic GetRedirectTopic(int id)
    {
        var topic = EntityCache.GetCategory(id);
        var currentWiki = EntityCache.GetCategory(_sessionUser.CurrentWikiId);
        var lastBreadcrumbItem = crumbtrailService.BuildCrumbtrail(topic, currentWiki).Items.LastOrDefault();

        if (lastBreadcrumbItem != null)
            return new
            {
                name = lastBreadcrumbItem.Category.Name,
                id = lastBreadcrumbItem.Category.Id
            };

        return new
        {
            name = currentWiki.Name,
            id = currentWiki.Id
        };
    }

    public class HasDeleted
    {
        public bool DeletedSuccessful { get; set; }
        public bool HasChildren { get; set; }
        public bool IsNotCreatorOrAdmin { get; set; }
    }
}