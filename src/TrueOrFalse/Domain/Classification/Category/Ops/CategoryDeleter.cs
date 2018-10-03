﻿using NHibernate;
using TrueOrFalse.Search;

public class CategoryDeleter : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;
    private readonly SearchIndexCategory _searchIndexCategory;

    public CategoryDeleter(
        ISession session,
        SearchIndexCategory searchIndexCategory)
    {
        _session = session;
        _searchIndexCategory = searchIndexCategory;
    }

    public void Run(Category category)
    {
        if (category == null)
            return;

        ThrowIfNot_IsLoggedInUserOrAdmin.Run(category.Creator.Id);

        _session.CreateSQLQuery("DELETE FROM relatedcategoriestorelatedcategories where Related_id = " + category.Id).ExecuteUpdate();
        _session.CreateSQLQuery("DELETE FROM relatedcategoriestorelatedcategories where Category_id = " + category.Id).ExecuteUpdate();
        _session.CreateSQLQuery("DELETE FROM categories_to_questions where Category_id = " + category.Id).ExecuteUpdate();
        _session.CreateSQLQuery("DELETE FROM categories_to_sets where Category_id = " + category.Id).ExecuteUpdate();
        Sl.R<UserActivityRepo>().DeleteForCategory(category.Id);

        Sl.R<CategoryRepository>().Delete(category);
    }
}