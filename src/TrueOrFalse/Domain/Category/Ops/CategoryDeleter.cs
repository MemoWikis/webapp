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

    public HasDeleted Run(Category category)
    {
        var categoryCacheItem = EntityCache.GetCategoryCacheItem(category.Id, getDataFromEntityCache: true);
        var hasDeleted = new HasDeleted();

        if (categoryCacheItem.CachedData.ChildrenIds.Count != 0)
        {
            Logg.r().Error("Category can´t deleted it has children");
            hasDeleted.HasChildren = true;
            return hasDeleted;
        }

        if (!Sl.SessionUser.IsInstallationAdmin && Sl.CurrentUserId != categoryCacheItem.Creator.Id)
        {
            hasDeleted.IsNotCreatorOrAdmin = true;
            return hasDeleted;
        }

        _session.CreateSQLQuery("DELETE FROM relatedcategoriestorelatedcategories where Related_id = " + category.Id).ExecuteUpdate();
        _session.CreateSQLQuery("DELETE FROM relatedcategoriestorelatedcategories where Category_id = " + category.Id).ExecuteUpdate();
        _session.CreateSQLQuery("DELETE FROM categories_to_questions where Category_id = " + category.Id).ExecuteUpdate();
        _session.CreateSQLQuery("DELETE FROM categories_to_sets where Category_id = " + category.Id).ExecuteUpdate();

        Sl.UserActivityRepo.DeleteForCategory(category.Id); 
        Sl.CategoryRepo.Delete(category);

        Sl.CategoryChangeRepo.AddDeleteEntry(category);
        Sl.CategoryValuationRepo.DeleteCategoryValuation(category.Id);

        ModifyRelationsEntityCache.DeleteIncludetContentOf(categoryCacheItem);
        CategoryRepository.UpdateCachedData(categoryCacheItem, CategoryRepository.CreateDeleteUpdate.Delete);
        ModifyRelationsUserEntityCache.DeleteFromAllParents(categoryCacheItem);
        EntityCache.Remove(categoryCacheItem);
        UserCache.RemoveAllForCategory(category.Id);
        UserEntityCache.DeleteCategory(category.Id);
        hasDeleted.DeletedSuccessful = true;
        return hasDeleted; 
    }

    public class HasDeleted
    {
        public bool HasChildren { get;  set; }
        public bool IsNotCreatorOrAdmin { get; set; }
        public bool DeletedSuccessful { get; set; }
    }
}
