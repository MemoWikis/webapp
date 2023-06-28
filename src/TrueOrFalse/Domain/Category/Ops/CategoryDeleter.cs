using System.Linq;
using NHibernate;
using TrueOrFalse.Search;

public class CategoryDeleter : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;
    private readonly SessionUser _sessionUser;
    private readonly UserActivityRepo _userActivityRepo;
    private readonly CategoryRepository _categoryRepository;
    private readonly CategoryChangeRepo _categoryChangeRepo;
    private readonly CategoryValuationRepo _categoryValuationRepo;
    private readonly PermissionCheck _permissionCheck;

    public CategoryDeleter(
        ISession session,

        SessionUser sessionUser,
        UserActivityRepo userActivityRepo,
        CategoryRepository categoryRepository,
        CategoryChangeRepo categoryChangeRepo,
        CategoryValuationRepo categoryValuationRepo,
        PermissionCheck permissionCheck)
    {
        _session = session;
        _sessionUser = sessionUser;
        _userActivityRepo = userActivityRepo;
        _categoryRepository = categoryRepository;
        _categoryChangeRepo = categoryChangeRepo;
        _categoryValuationRepo = categoryValuationRepo;
        _permissionCheck = permissionCheck;
    }

    public HasDeleted Run(Category category, int userId, bool isTestCase = false)
    {
        var categoryCacheItem = EntityCache.GetCategory(category.Id);
        var hasDeleted = new HasDeleted();

        if (categoryCacheItem.CachedData.ChildrenIds.Count != 0)
        {
            Logg.r().Error("Category can´t deleted it has children");
            hasDeleted.HasChildren = true;
            return hasDeleted;
        }

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
        _categoryRepository.Delete(category);
        _categoryChangeRepo.AddDeleteEntry(category, userId);
       _categoryValuationRepo.DeleteCategoryValuation(category.Id);

        ModifyRelationsEntityCache.DeleteIncludetContentOf(categoryCacheItem);
        EntityCache.UpdateCachedData(categoryCacheItem, CategoryRepository.CreateDeleteUpdate.Delete);
        var parentIds = EntityCache.ParentCategories(category.Id, _permissionCheck).Select(cci => cci.Id).ToList();
        foreach (var parentId in parentIds)
        {
            EntityCache.GetCategory(parentId).CachedData.RemoveChildId(categoryCacheItem.Id);
        }

        EntityCache.Remove(categoryCacheItem, _permissionCheck, userId);
        SessionUserCache.RemoveAllForCategory(category.Id);
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