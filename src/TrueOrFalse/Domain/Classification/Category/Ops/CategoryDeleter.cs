﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using TrueOrFalse.Search;

namespace TrueOrFalse
{
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

            _searchIndexCategory.Delete(category);

            _session.CreateSQLQuery("DELETE FROM relatedcategoriestorelatedcategories where Related_id = " + category.Id).ExecuteUpdate();
            _session.CreateSQLQuery("DELETE FROM relatedcategoriestorelatedcategories where Category_id = " + category.Id).ExecuteUpdate();
            _session.CreateSQLQuery("DELETE FROM categoriestoquestions where Category_id = " + category.Id).ExecuteUpdate();
            _session.CreateSQLQuery("DELETE FROM category WHERE Id = " + category.Id).ExecuteUpdate();   
        }
    }
}