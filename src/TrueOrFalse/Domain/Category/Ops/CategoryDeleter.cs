using ISession = NHibernate.ISession;

public class CategoryDeleter(
    ISession _session,
    SessionUser _sessionUser,
    UserActivityRepo _userActivityRepo,
    CategoryRepository _categoryRepository,
    CategoryChangeRepo _categoryChangeRepo,
    CategoryValuationWritingRepo _categoryValuationWritingRepo,
    PermissionCheck _permissionCheck,
    SessionUserCache _sessionUserCache)
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

        ModifyRelationsEntityCache.DeleteIncludetContentOf(categoryCacheItem);
        EntityCache.Remove(categoryCacheItem, _permissionCheck, userId);
        _sessionUserCache.RemoveAllForCategory(category.Id, _categoryValuationWritingRepo);

        hasDeleted.DeletedSuccessful = true;
        return hasDeleted;
    }

    public class HasDeleted
    {
        public bool DeletedSuccessful { get; set; }
        public bool HasChildren { get; set; }
        public bool IsNotCreatorOrAdmin { get; set; }
    }
}