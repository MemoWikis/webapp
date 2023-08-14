using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using NHibernate;

public class CategoryDeleter : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;
    private readonly SessionUser _sessionUser;
    private readonly UserActivityRepo _userActivityRepo;
    private readonly CategoryRepository _categoryRepository;
    private readonly CategoryChangeRepo _categoryChangeRepo;
    private readonly CategoryValuationWritingRepo _categoryValuationWritingRepo;
    private readonly CategoryValuationReadingRepo _categoryValuationReading;
    private readonly IMemoryCache _cache;
    private readonly PermissionCheck _permissionCheck;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly QuestionValuationRepo _questionValuationRepo;

    public CategoryDeleter(
        ISession session,
        SessionUser sessionUser,
        UserActivityRepo userActivityRepo,
        CategoryRepository categoryRepository,
        CategoryChangeRepo categoryChangeRepo,
        CategoryValuationWritingRepo categoryValuationWritingRepo,
        PermissionCheck permissionCheck,
        UserReadingRepo userReadingRepo,
        QuestionValuationRepo questionValuationRepo,
        CategoryValuationReadingRepo categoryValuationReading,
        IMemoryCache cache)
    {
        _session = session;
        _sessionUser = sessionUser;
        _userActivityRepo = userActivityRepo;
        _categoryRepository = categoryRepository;
        _categoryChangeRepo = categoryChangeRepo;
        _categoryValuationWritingRepo = categoryValuationWritingRepo;
        _permissionCheck = permissionCheck;
        _userReadingRepo = userReadingRepo;
        _questionValuationRepo = questionValuationRepo;
        _categoryValuationReading = categoryValuationReading;
        _cache = cache;
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
       _categoryValuationWritingRepo.DeleteCategoryValuation(category.Id);

        ModifyRelationsEntityCache.DeleteIncludetContentOf(categoryCacheItem);
        EntityCache.UpdateCachedData(categoryCacheItem, CategoryRepository.CreateDeleteUpdate.Delete);
        var parentIds = EntityCache.ParentCategories(category.Id, _permissionCheck).Select(cci => cci.Id).ToList();
        foreach (var parentId in parentIds)
        {
            EntityCache.GetCategory(parentId).CachedData.RemoveChildId(categoryCacheItem.Id);
        }

        EntityCache.Remove(categoryCacheItem, _permissionCheck, userId);
        SessionUserCache.RemoveAllForCategory(category.Id, 
            _categoryValuationReading, 
            _categoryValuationWritingRepo,
            _userReadingRepo, 
            _questionValuationRepo,
            _cache); 

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