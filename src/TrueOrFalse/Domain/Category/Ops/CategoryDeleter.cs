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
        if (category == null)
            return;
        
        if (!forSetMigration)
            ThrowIfNot_IsLoggedInUserOrAdmin.Run(category.Creator.Id);

        _session.CreateSQLQuery("DELETE FROM relatedcategoriestorelatedcategories where Related_id = " + category.Id).ExecuteUpdate();
        _session.CreateSQLQuery("DELETE FROM relatedcategoriestorelatedcategories where Category_id = " + category.Id).ExecuteUpdate();
        _session.CreateSQLQuery("DELETE FROM categories_to_questions where Category_id = " + category.Id).ExecuteUpdate();
        _session.CreateSQLQuery("DELETE FROM categories_to_sets where Category_id = " + category.Id).ExecuteUpdate();

        Sl.UserActivityRepo.DeleteForCategory(category.Id);
        if (forSetMigration)
            Sl.CategoryRepo.DeleteWithoutFlush(category);
        else
            Sl.CategoryRepo.Delete(category);
        Sl.CategoryChangeRepo.AddDeleteEntry(category);
        Sl.CategoryValuationRepo.DeleteCategoryValuation(category.Id);

        UserCache.RemoveAllForCategory(category.Id);
    }
}