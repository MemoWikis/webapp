using NHibernate;
using TrueOrFalse.Search;

public class CategoryDeleter : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;

    public CategoryDeleter(
        ISession session,
        SearchIndexCategory searchIndexCategory)
    {
        _session = session;
    }

    public void Run(Category category, bool forSetMigration = false)
    {
        var categoryCacheItem = EntityCache.GetCategoryCacheItem(category.Id, getDataFromEntityCache: true);
        if (categoryCacheItem.CachedData.ChildrenIds.Count != 0)
        {
            Logg.r().Error("Category can´t deleted it has children");
            return;
        }
        _session.CreateSQLQuery("DELETE FROM relatedcategoriestorelatedcategories where Related_id = " + category.Id).ExecuteUpdate();
        _session.CreateSQLQuery("DELETE FROM relatedcategoriestorelatedcategories where Category_id = " + category.Id).ExecuteUpdate();
        _session.CreateSQLQuery("DELETE FROM categories_to_questions where Category_id = " + category.Id).ExecuteUpdate();
        _session.CreateSQLQuery("DELETE FROM categories_to_sets where Category_id = " + category.Id).ExecuteUpdate();


        ThrowIfNot_IsLoggedInUserOrAdmin.Run(category.Creator.Id);

        Sl.UserActivityRepo.DeleteForCategory(category.Id); 
        Sl.CategoryRepo.Delete(category);

        Sl.CategoryChangeRepo.AddDeleteEntry(category);
        Sl.CategoryValuationRepo.DeleteCategoryValuation(category.Id);

        ModifyRelationsEntityCache.DeleteIncludetContentOf(categoryCacheItem);
        CategoryRepository.UpdateCachedData(categoryCacheItem, CategoryRepository.CreateDeleteUpdate.Delete);
        ModifyRelationsUserEntityCache.Delete(categoryCacheItem);
        EntityCache.Remove(categoryCacheItem);
        UserCache.RemoveAllForCategory(category.Id);
    }
}